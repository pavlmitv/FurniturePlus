using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurniturePlus.Data
{
    public class FurniturePlusDbContext : IdentityDbContext
    {
        public FurniturePlusDbContext(DbContextOptions<FurniturePlusDbContext> options)
            : base(options)
        {
        }
    }
}
