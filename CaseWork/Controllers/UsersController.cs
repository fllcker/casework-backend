using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseWork.Data;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services;
using Microsoft.AspNetCore.Authorization;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CaseWorkContext _context;
        private readonly IUsersService _usersService;

        public UsersController(CaseWorkContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        [HttpPost]
        [Route("update/profile")]
        public async Task<ActionResult<User>> Update([FromBody] UserUpdate userUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(ModelState.Values.SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage).First());
                return await _usersService.UpdateInfo(userUpdate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
