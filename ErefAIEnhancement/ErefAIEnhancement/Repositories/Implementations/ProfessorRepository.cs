using ErefAIEnhancement.Data;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ErefAIEnhancement.Repositories.Implementations
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly AppDbContext _context;

        public ProfessorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Professor>> GetAllAsync()
        {
            return await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Subjects)
                .ToListAsync();
        }

        public async Task<Professor?> GetByIdAsync(Guid id)
        {
            return await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Subjects)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Professor?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Professors
                .Include(p => p.User)
                .Include(p => p.Subjects)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(Professor professor)
        {
            await _context.Professors.AddAsync(professor);
        }

        public void Update(Professor professor)
        {
            _context.Professors.Update(professor);
        }

        public void Delete(Professor professor)
        {
            _context.Professors.Remove(professor);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}