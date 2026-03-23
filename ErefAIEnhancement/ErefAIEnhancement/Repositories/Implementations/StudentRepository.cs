using ErefAIEnhancement.Data;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ErefAIEnhancement.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public void Update(Student student)
        {
            _context.Students.Update(student);
        }

        public void Delete(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}