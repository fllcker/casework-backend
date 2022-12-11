using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CaseWork.Models;

namespace CaseWork.Data
{
    public class CaseWorkContext : DbContext
    {
        public CaseWorkContext (DbContextOptions<CaseWorkContext> options)
            : base(options)
        {
        }

        public DbSet<CaseWork.Models.User> Users { get; set; } = default!;
        public DbSet<CaseWork.Models.Task> Tasks { get; set; } = default!;
        public DbSet<CaseWork.Models.Company> Companies { get; set; } = default!;
        public DbSet<CaseWork.Models.Role> Roles { get; set; } = default!;
        public DbSet<CaseWork.Models.RoleRelation> RoleRelations { get; set; } = default!;
    }
}
