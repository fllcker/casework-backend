using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CaseWork.Exceptions;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services.Invites;
using CaseWork.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Task = System.Threading.Tasks.Task;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitesController : ControllerBase
    {
        private readonly IInvitesService _invitesService;
        
        public InvitesController(IInvitesService invitesService)
        {
            _invitesService = invitesService;
        }

        [HttpPost]
        [Authorize]
        [Route("create/{targetEmail}")]
        public async Task<ActionResult<Invite>> Create(string targetEmail, [FromBody] InviteCreate inviteCreate)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email)!;
                return await _invitesService.Create(email, targetEmail, inviteCreate);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("deny/{id}")]
        public async Task<ActionResult<Invite>> DenyAnswer(int id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email)!;
                return await _invitesService.DenyInvite(id, email);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                if (e.Code == HttpStatusCode.Unauthorized) return Unauthorized(e.Message);
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [Authorize]
        [Route("accept/{id}")]
        public async Task<ActionResult<Invite>> AcceptAnswer(int id)
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email)!;
                return await _invitesService.AcceptInvite(id, email);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                if (e.Code == HttpStatusCode.Unauthorized) return Unauthorized(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("get/my")]
        public async Task<IEnumerable<Invite>> GetUserInvites()
        {
            return await _invitesService
                .GetUserInvites(User.FindFirstValue(ClaimTypes.Email)!);
        }

        [HttpGet]
        [Authorize]
        [Route("get/{taskId}")]
        public async Task<ActionResult<Invite>> GetInviteByTask(int taskId)
        {
            try
            {
                return await _invitesService.GetInviteByTask(taskId);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
