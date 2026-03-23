using ErefAIEnhancement.Data;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ErefAIEnhancement.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(Guid id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }

        public void Delete(Role role)
        {
            _context.Roles.Remove(role);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUsersAsync(Guid roleId)
        {
            return await _context.Users.AnyAsync(u => u.RoleId == roleId);
        }
    }
}