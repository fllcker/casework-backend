using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CaseWork.Exceptions;
using CaseWork.Models;
using CaseWork.Services.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesService _companiesService;

        public CompaniesController(ICompaniesService companiesService)
        {
            _companiesService = companiesService;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<ActionResult<Company>> Create([FromBody] Company company)
        {
            try
            {
                var accessEmail = User.FindFirstValue(ClaimTypes.Email)!;
                return await _companiesService.Create(company, accessEmail);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("get/members/{companyId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllMembers(int companyId)
        {
            try
            {
                return Ok(await _companiesService
                    .GetAllMembers(companyId, User.FindFirstValue(ClaimTypes.Email)!));
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.Unauthorized) return Unauthorized(e.Message);
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("get/company")]
        public async Task<ActionResult<Company>> GetUserCompany()
        {
            try
            {
                return await _companiesService
                    .GetUserCompany(User.FindFirstValue(ClaimTypes.Email)!);
            }
            catch (ErrorResponse e)
            {
                if (e.Code == HttpStatusCode.Unauthorized) return Unauthorized(e.Message);
                if (e.Code == HttpStatusCode.NotFound) return NotFound(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
