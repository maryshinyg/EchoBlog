using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EchoBlog.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext(options)
    {
        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //seed Roles(User, Admin, SuperAdmin)
            var AdminRoldId = "5420268f-c374-478a-b20b-72a56dfaa408";
            var superAdminRoleId = "87cd69ce-be42-44f0-9530-6af30351eaa7";
            var userRoleId = "7119e1dd-53bc-46a8-a48c-688201a194dd";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                { 
                    Name = "Admin", 
                    NormalizedName = "Admin" ,
                    Id = AdminRoldId,
                    ConcurrencyStamp = AdminRoldId
                },
                new IdentityRole 
                { 
                    Name = "SuperAdmin", 
                    NormalizedName = "SuperAdmin" ,
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole 
                { 
                    Name = "User", 
                    NormalizedName = "User",
                    Id =  userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            //seed SuperAdiminUser
            var superAdminId = "6a5b8bc5-06fa-44f9-92f7-3c4550523ad4";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@echo.com",
                Email = "superadmin@echo.com",
                NormalizedUserName = "SUPERADMIN@ECHO.COM",
                NormalizedEmail = "SUPERADMIN@ECHO.COM",
                Id = superAdminId,
                ConcurrencyStamp = "b4b0688f-a364-4b1d-9c5b-0ba17b53010e",
                SecurityStamp = "7f851ddd-9d1f-4eea-87b9-149e05779b09",
                PasswordHash = "AQAAAAIAAYagAAAAEJ0l/x7s4APJ5v0pJB5qWfUpGajkiCjPoYeXNVAr1jyv66TWJNt+j97YbTx5gOc2LA=="
            };
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Map all the roles to superadminuser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = AdminRoldId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}