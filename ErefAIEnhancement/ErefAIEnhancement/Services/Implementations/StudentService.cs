using ErefAIEnhancement.DTOs.StudentDto;
using ErefAIEnhancement.DTOs.StudentSubjectDtos;
using ErefAIEnhancement.Exceptions;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;

namespace ErefAIEnhancement.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IServiceProvider _serviceProvider;

        public StudentService(
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            ISubjectRepository subjectRepository,
            IServiceProvider serviceProvider)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<StudentResponseDto>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(MapToDto).ToList();
        }

        public async Task<StudentResponseDto> GetByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException("Student not found");

            return MapToDto(student);
        }

        public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto)
        {
            await ValidateAsync(dto);

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (user.Role == null || user.Role.RoleName != "Student")
                throw new BadRequestException("Only users with role 'Student' can be added to Students table.");

            var existingStudent = await _studentRepository.GetByUserIdAsync(dto.UserId);
            if (existingStudent != null)
                throw new BadRequestException("This user is already in Students table.");

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

        public async Task<StudentResponseDto> UpdateAsync(Guid id, UpdateStudentDto dto)
        {
            await ValidateAsync(dto);

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException("Student not found");

            student.IndexNumber = dto.IndexNumber;
            student.YearOfStudy = dto.YearOfStudy;
            student.DateOfBirth = dto.DateOfBirth;
            student.Department = dto.Department;

            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();

            var updatedStudent = await _studentRepository.GetByIdAsync(id);
            return MapToDto(updatedStudent!);
        }

        public async Task DeleteAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException("Student not found");

            _studentRepository.Delete(student);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task<List<StudentSubjectSelectionItemDto>> GetSubjectsAsync(Guid studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException("Student not found");

            var studentSubjects = await _studentRepository.GetStudentSubjectsAsync(studentId);
            var allSubjects = await _subjectRepository.GetAllAsync();

            return allSubjects
                .Select(subject =>
                {
                    var existingStudentSubject = studentSubjects
                        .FirstOrDefault(ss => ss.SubjectId == subject.Id);

                    var isRequiredForStudent =
                        subject.Required &&
                        subject.Department == student.Department &&
                        subject.YearOfStudy == student.YearOfStudy;

                    return new StudentSubjectSelectionItemDto
                    {
                        SubjectId = subject.Id,
                        SubjectName = subject.Name,
                        YearOfStudy = subject.YearOfStudy,
                        Department = subject.Department,
                        Required = subject.Required,
                        Selected = isRequiredForStudent || (existingStudentSubject?.Selected ?? false)
                    };
                })
                .OrderBy(x => x.YearOfStudy)
                .ThenBy(x => x.Department)
                .ThenByDescending(x => x.Required)
                .ThenBy(x => x.SubjectName)
                .ToList();
        }

        public async Task<List<StudentSubjectSelectionItemDto>> UpdateSubjectsAsync(Guid studentId, UpdateStudentSubjectsDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException("Student not found");

            var requestedSubjectIds = dto.SubjectIds
                .Distinct()
                .ToList();

            var validSubjects = await _subjectRepository.GetByIdsAsync(requestedSubjectIds);
            if (validSubjects.Count != requestedSubjectIds.Count)
                throw new BadRequestException("One or more selected subjects do not exist.");

            var requiredSubjectIds = (await _subjectRepository.GetAllAsync())
                .Where(subject =>
                    subject.Required &&
                    subject.Department == student.Department &&
                    subject.YearOfStudy == student.YearOfStudy)
                .Select(subject => subject.Id)
                .ToHashSet();

            var optionalSelectedSubjects = validSubjects
                .Where(subject => !requiredSubjectIds.Contains(subject.Id))
                .ToList();

            var existingStudentSubjects = await _studentRepository.GetStudentSubjectsAsync(studentId);

            if (existingStudentSubjects.Any())
            {
                _studentRepository.RemoveStudentSubjects(existingStudentSubjects);
                await _studentRepository.SaveChangesAsync();
            }

            var newStudentSubjects = optionalSelectedSubjects
                .Select(subject => new StudentSubject
                {
                    StudentId = studentId,
                    SubjectId = subject.Id,
                    Selected = true
                })
                .ToList();

            if (newStudentSubjects.Any())
            {
                await _studentRepository.AddStudentSubjectsAsync(newStudentSubjects);
                await _studentRepository.SaveChangesAsync();
            }

            return await GetSubjectsAsync(studentId);
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

        private async Task ValidateAsync<T>(T dto)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            if (validator == null)
                return;

            var result = await validator.ValidateAsync(dto);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }
}