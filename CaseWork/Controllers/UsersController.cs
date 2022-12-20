using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseWork.Data;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services;
using CaseWork.Services.Users;
using Microsoft.AspNetCore.Authorization;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [Route("update/profile")]
        public async Task<ActionResult<User>> Update([FromBody] UserUpdate userUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(ModelState.Values.SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage).First());
                return await _usersService.UpdateInfo(userUpdate, 
                    User.FindFirstValue(ClaimTypes.Email)!);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileData>> GetProfileData()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _usersService.GetByEmail(email);
            if (user == null) return NotFound();
            return _mapper.Map<UserProfileData>(user);
        }
    }
}
