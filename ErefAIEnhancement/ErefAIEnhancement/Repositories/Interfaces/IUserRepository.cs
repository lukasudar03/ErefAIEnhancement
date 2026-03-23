using ErefAIEnhancement.Models;

namespace ErefAIEnhancement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task SaveChangesAsync();
        Task<bool> RoleExistsAsync(Guid roleId);
    }
}