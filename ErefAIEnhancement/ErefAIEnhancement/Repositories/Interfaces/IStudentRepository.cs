using ErefAIEnhancement.Models;

namespace ErefAIEnhancement.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(Guid id);
        Task<Student?> GetByUserIdAsync(Guid userId);

        Task<List<StudentSubject>> GetStudentSubjectsAsync(Guid studentId);
        Task<Student?> GetByIdWithStudentSubjectsAsync(Guid id);
        Task AddStudentSubjectsAsync(IEnumerable<StudentSubject> studentSubjects);
        void RemoveStudentSubjects(IEnumerable<StudentSubject> studentSubjects);

        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task SaveChangesAsync();
    }
}