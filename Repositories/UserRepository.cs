using EchoBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EchoBlog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _authDbContext;

        public UserRepository(AuthDbContext context)
        {
            _authDbContext = context;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users =  await _authDbContext.Users.ToListAsync();

            var superAdminUser = await _authDbContext.Users.FirstOrDefaultAsync(u => u.Email == "superadmin@echo.com");

            if(superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }
            return users;
        }
    }
}