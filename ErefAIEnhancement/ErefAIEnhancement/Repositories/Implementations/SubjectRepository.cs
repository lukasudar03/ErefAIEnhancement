using ErefAIEnhancement.Data;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ErefAIEnhancement.Repositories.Implementations
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;

        public SubjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _context.Subjects
                .Include(s => s.Professor)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(Guid id)
        {
            return await _context.Subjects
                .Include(s => s.Professor)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Subject>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Subjects
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<List<Subject>> GetAvailableAsync()
        {
            return await _context.Subjects
                .Include(s => s.Professor)
                .ThenInclude(p => p.User)
                .Where(s => s.ProfessorId == null)
                .ToListAsync();
        }

        public async Task<Subject?> GetByNameAndProfessorIdAsync(string name, Guid professorId)
        {
            return await _context.Subjects
                .Include(s => s.Professor)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(s => s.Name == name && s.ProfessorId == professorId);
        }

        public async Task AddAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
        }

        public void Update(Subject subject)
        {
            _context.Subjects.Update(subject);
        }

        public void Delete(Subject subject)
        {
            _context.Subjects.Remove(subject);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}