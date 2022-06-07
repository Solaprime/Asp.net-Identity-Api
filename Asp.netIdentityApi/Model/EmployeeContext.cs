using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Model
{
    // instead of inheriting fro Db Context we inheridet from
    //IdentityDbContext, this class will create some defalut table
    // for us table realted to login, credentilan clasims and the Likes
    public class EmployeeContext : IdentityDbContext
    {
        public EmployeeContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
