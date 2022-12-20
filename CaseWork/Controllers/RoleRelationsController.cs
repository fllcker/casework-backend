using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseWork.Data;
using CaseWork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleRelationsController : ControllerBase
    {
        private readonly CaseWorkContext _dbContext;
        
        public RoleRelationsController(CaseWorkContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("set/{email}/{roleName}")]
        public async Task<ActionResult<RoleRelation>> SetRole(string email, string roleName)
        {
            var candidate = await _dbContext.Users.SingleOrDefaultAsync(v => v.Email == email);
            if (candidate == null) return NotFound("User not found");
            Role? role = await _dbContext.Roles.FirstOrDefaultAsync(v => v.Title == roleName);
            if (role == null)
            {
                role = new Role()
                {
                    Title = roleName,
                    Priority = 0
                };
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
            }

            var relation = new RoleRelation()
            {
                Role = role,
                User = candidate
            };
            _dbContext.RoleRelations.Add(relation);
            await _dbContext.SaveChangesAsync();
            return relation;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("remove/{email}/{roleName}")]
        public async Task<ActionResult> RemoveRole(string email, string roleName)
        {
            try
            {
                var roles = await _dbContext.RoleRelations
                    .Include(v => v.Role)
                    .Include(v => v.User)
                    .Where(v => v.User.Email == email)
                    .Where(v => v.Role.Title == roleName)
                    .ToListAsync();
                _dbContext.RemoveRange(roles);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
