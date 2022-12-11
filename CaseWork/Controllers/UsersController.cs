using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaseWork.Data;
using CaseWork.Models;

namespace CaseWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CaseWorkContext _context;

        public UsersController(CaseWorkContext context)
        {
            _context = context;
        }
    }
}
