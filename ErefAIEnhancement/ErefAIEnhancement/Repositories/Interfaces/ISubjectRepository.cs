using ErefAIEnhancement.Models;
using Microsoft.EntityFrameworkCore;

namespace ErefAIEnhancement.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(Guid id);
        Task<List<Subject>> GetByIdsAsync(List<Guid> ids);
        Task<Subject?> GetByNameAndProfessorIdAsync(string name, Guid professorId);
        Task AddAsync(Subject subject);
        void Update(Subject subject);
        void Delete(Subject subject);
        Task<int> SaveChangesAsync();
    }
}