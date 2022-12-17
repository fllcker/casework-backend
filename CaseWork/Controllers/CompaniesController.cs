using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CaseWork.Models;
using CaseWork.Services.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("get/members/{companyName}")]
        public async Task<IEnumerable<User>> GetAllMembers(string companyName)
        {
            return await _companiesService
                .GetAllMembers(companyName, User.FindFirstValue(ClaimTypes.Email)!);
        }
    }
}
