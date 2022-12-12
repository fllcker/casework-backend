using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CaseWork.Models.Dto;
using CaseWork.Services.Tasks;
using CaseWork.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
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
    }
}
