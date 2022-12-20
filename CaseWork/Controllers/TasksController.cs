using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CaseWork.Data;
using CaseWork.Exceptions;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services.Invites;
using CaseWork.Services.Tasks;
using CaseWork.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly IUsersService _usersService;

        public TasksController(ITasksService tasksService, IUsersService usersService)
        {
            _tasksService = tasksService;
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize]
        [Route("get/id/{id}")]
        public async Task<ActionResult<Models.Task>> GetById(int id)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email)!;
                return await _tasksService.GetByIdWithVerifies(id, userEmail);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.Unauthorized) return Unauthorized(e.Message);
                if (e.Code == HttpStatusCode.NotFound) return NotFound();
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Route("get/filter/{skip?}/{take?}")]
        public async Task<IEnumerable<Models.Task>> GetTasksByFilter([FromBody] TasksByFilterAr tasksByFilterAr,
            int skip = 0, int take = 10)
        {
            return await _tasksService.GetByFilter(
                tasksByFilterAr.TasksTypeFilter,
                tasksByFilterAr.TasksAccessFilter,
                User.FindFirstValue(ClaimTypes.Email)!,
                skip, take);
        }

        [HttpGet]
        [Authorize]
        [Route("get/nonaccepted")]
        public async Task<IEnumerable<Models.Task>> GetNonAcceptedTasks()
        {
            return await _tasksService
                .GetNonAcceptedTasks(User.FindFirstValue(ClaimTypes.Email)!);
        }

        [HttpGet]
        [Authorize]
        [Route("complete/{id}")]
        public async Task<ActionResult<Models.Task>> ToComplete(int id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email)!;
                return await _tasksService.ToComplete(email, id);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("create/{inviteTo}")]
        public async Task<ActionResult<Models.Task>> Create(TaskCreate taskCreate, string? inviteTo)
        {
            var userCreator = await _usersService.GetByEmail(User.FindFirstValue(ClaimTypes.Email)!);
            if (userCreator == null) return Unauthorized();
            inviteTo ??= userCreator.Email;
            
            var invitedUser = await _usersService.GetByEmail(inviteTo);
            if (invitedUser == null) return NotFound("User not found!");
            
            return await _tasksService.Create(taskCreate, userCreator, invitedUser);
        }
    }
}
