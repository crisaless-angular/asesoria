using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web.BA002.Data
{
    public partial class BA002IdentityContext : IdentityDbContext<IdentityUser>
    {
        public BA002IdentityContext()
        {
        }

        public BA002IdentityContext(DbContextOptions<BA002IdentityContext> DefaultConnection)
            : base(DefaultConnection)
        {
        }
    }
}
