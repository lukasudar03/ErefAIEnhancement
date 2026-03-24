using ErefAIEnhancement.Models;

public interface IProfessorRepository
{
    Task<List<Professor>> GetAllAsync();
    Task<Professor?> GetByIdAsync(Guid id);
    Task<Professor?> GetByUserIdAsync(Guid userId);
    Task AddAsync(Professor professor);
    void Update(Professor professor);
    void Delete(Professor professor);
    Task<int> SaveChangesAsync();
}