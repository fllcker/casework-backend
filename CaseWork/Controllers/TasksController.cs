using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CaseWork.Data;
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [Authorize]
        [Route("get/incompleted/executor")]
        public async Task<IEnumerable<Models.Task>> GetInCompletedTasks()
            => await _tasksService
                .GetInCompletedTasks(User.FindFirstValue(ClaimTypes.Email)!);
        
        [HttpGet]
        [Authorize]
        [Route("get/incompleted/employer")]
        public async Task<IEnumerable<Models.Task>> GetInCompletedTasksByEmployer()
            => await _tasksService
                .GetInCompletedTasksByEmployer(User.FindFirstValue(ClaimTypes.Email)!);
        
        [HttpGet]
        [Authorize]
        [Route("get/all/executor")]
        public async Task<IEnumerable<Models.Task>> GetAllTasksByExecutor()
            => await _tasksService
                .GetAllTasks(User.FindFirstValue(ClaimTypes.Email)!, GetBy.Executor);
        
        [HttpGet]
        [Authorize]
        [Route("get/all/employer")]
        public async Task<IEnumerable<Models.Task>> GetAllTasksByEmployer()
            => await _tasksService
                .GetAllTasks(User.FindFirstValue(ClaimTypes.Email)!, GetBy.Employer);

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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("create/{inviteTo}")]
        public async Task<ActionResult<Models.Task>> Create(TaskCreate taskCreate, string inviteTo)
        {
            var userCreator = await _usersService.GetByEmail(User.FindFirstValue(ClaimTypes.Email)!);
            if (userCreator == null) return Unauthorized();
            
            var invitedUser = await _usersService.GetByEmail(inviteTo);
            if (invitedUser == null) return BadRequest("User not found!");
            
            return await _tasksService.Create(taskCreate, userCreator, invitedUser);
        }
    }
}
