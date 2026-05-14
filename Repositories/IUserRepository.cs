using Microsoft.AspNetCore.Identity;

namespace EchoBlog.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}