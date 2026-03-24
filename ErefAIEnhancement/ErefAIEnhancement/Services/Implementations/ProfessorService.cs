using ErefAIEnhancement.DTOs.ProfessorDtos;
using ErefAIEnhancement.DTOs.StudentDto;
using ErefAIEnhancement.Exceptions;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Implementations;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;

namespace ErefAIEnhancement.Services.Implementations
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;

        public ProfessorService(
            IProfessorRepository professorRepository,
            IUserRepository userRepository,
            IServiceProvider serviceProvider)
        {
            _professorRepository = professorRepository;
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<ProfessorResponseDto>> GetAllAsync()
        {
            var professors = await _professorRepository.GetAllAsync();
            return professors.Select(MapToDto).ToList();
        }

        public async Task<ProfessorResponseDto> GetByIdAsync(Guid id)
        {
            var professor = await _professorRepository.GetByIdAsync(id);
            if (professor == null)
                throw new NotFoundException("Professor not found");

            return MapToDto(professor);
        }

        public async Task<ProfessorResponseDto> CreateAsync(CreateProfessorDto dto)
        {
            await ValidateAsync(dto);

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (user.Role == null || user.Role.RoleName != "Professor")
                throw new BadRequestException("Only users with role 'Professor' can be added to Professors table.");

            var existingProfessor = await _professorRepository.GetByUserIdAsync(dto.UserId);
            if (existingProfessor != null)
                throw new BadRequestException("This user is already in Professors table.");

            var professor = new Professor
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                //AcademicTitle = dto.AcademicTitle,
                //Department = dto.Department,
                //DateOfBirth = dto.DateOfBirth
            };

            await _professorRepository.AddAsync(professor);
            await _professorRepository.SaveChangesAsync();

            var createdProfessor = await _professorRepository.GetByUserIdAsync(dto.UserId);
            return MapToDto(createdProfessor!);
        }

        public async Task<ProfessorResponseDto> UpdateAsync(Guid id, UpdateProfessorDto dto)
        {
            await ValidateAsync(dto);

            var professor = await _professorRepository.GetByIdAsync(id);
            if (professor == null)
                throw new NotFoundException("Student not found");

            //student.IndexNumber = dto.IndexNumber;
            //student.YearOfStudy = dto.YearOfStudy;
            //student.DateOfBirth = dto.DateOfBirth;
            //student.Department = dto.Department;

            _professorRepository.Update(professor);
            await _professorRepository.SaveChangesAsync();

            var updatedProfessor = await _professorRepository.GetByIdAsync(id);
            return MapToDto(updatedProfessor!);
        }

        public async Task DeleteAsync(Guid id)
        {
            var professor = await _professorRepository.GetByIdAsync(id);
            if (professor == null)
                throw new NotFoundException("Professor not found");

            _professorRepository.Delete(professor);
            await _professorRepository.SaveChangesAsync();
        }

        private static ProfessorResponseDto MapToDto(Professor professor)
        {
            return new ProfessorResponseDto
            {
                Id = professor.Id,
                UserId = professor.UserId,
                Name = professor.User?.Name ?? string.Empty,
                Email = professor.User?.Email ?? string.Empty
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