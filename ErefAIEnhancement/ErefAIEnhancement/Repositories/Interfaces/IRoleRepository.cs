using ErefAIEnhancement.Models;

namespace ErefAIEnhancement.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string roleName);
        Task AddAsync(Role role);
        void Update(Role role);
        void Delete(Role role);
        Task SaveChangesAsync();
        Task<bool> HasUsersAsync(Guid roleId);
    }
}