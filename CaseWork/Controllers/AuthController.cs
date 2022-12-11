using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CaseWork.Models.Dto;
using CaseWork.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        
        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }
        
        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<string>> Signup([FromBody] UserSignup userSignup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage).First());
            
            Models.User user = _mapper.Map<Models.User>(userSignup);
            
            if (await _authService.EmailExists(user.Email))
                return BadRequest("Email already exists");

            return await _authService.Create(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLogin userLogin)
        {
            try
            {
                return await _authService.Login(userLogin);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
