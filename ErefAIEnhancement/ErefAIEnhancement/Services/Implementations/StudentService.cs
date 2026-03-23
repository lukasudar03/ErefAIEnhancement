using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.StudentDto;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;

namespace ErefAIEnhancement.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;

        public StudentService(
            IStudentRepository studentRepository,
            IUserRepository userRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
        }

        public async Task<List<StudentResponseDto>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(MapToDto).ToList();
        }

        public async Task<StudentResponseDto?> GetByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return null;

            return MapToDto(student);
        }

        public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found.");

            if (user.Role == null || user.Role.RoleName != "Student")
                throw new Exception("Only users with role 'Student' can be added to Students table.");

            var existingStudent = await _studentRepository.GetByUserIdAsync(dto.UserId);
            if (existingStudent != null)
                throw new Exception("This user is already in Students table.");

            var student = new Student
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                IndexNumber = dto.IndexNumber,
                YearOfStudy = dto.YearOfStudy,
                DateOfBirth = dto.DateOfBirth,
                Department = dto.Department
            };

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            var createdStudent = await _studentRepository.GetByUserIdAsync(dto.UserId);
            return MapToDto(createdStudent!);
        }

        public async Task<StudentResponseDto?> UpdateAsync(Guid id, UpdateStudentDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return null;

            student.IndexNumber = dto.IndexNumber;
            student.YearOfStudy = dto.YearOfStudy;
            student.DateOfBirth = dto.DateOfBirth;
            student.Department = dto.Department;

            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();

            var updatedStudent = await _studentRepository.GetByIdAsync(id);
            return MapToDto(updatedStudent!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                return false;

            _studentRepository.Delete(student);
            await _studentRepository.SaveChangesAsync();

            return true;
        }

        private static StudentResponseDto MapToDto(Student student)
        {
            return new StudentResponseDto
            {
                Id = student.Id,
                UserId = student.UserId,
                IndexNumber = student.IndexNumber,
                YearOfStudy = student.YearOfStudy,
                DateOfBirth = student.DateOfBirth,
                Department = student.Department,
                Name = student.User?.Name ?? string.Empty,
                Email = student.User?.Email ?? string.Empty
            };
        }
    }
}