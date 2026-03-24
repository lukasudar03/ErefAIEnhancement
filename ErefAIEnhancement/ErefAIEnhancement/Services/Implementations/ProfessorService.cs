using ErefAIEnhancement.DTOs.ProfessorDtos;
using ErefAIEnhancement.Exceptions;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;

namespace ErefAIEnhancement.Services.Implementations
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IServiceProvider _serviceProvider;

        public ProfessorService(
            IProfessorRepository professorRepository,
            IUserRepository userRepository,
            ISubjectRepository subjectRepository,
            IServiceProvider serviceProvider)
        {
            _professorRepository = professorRepository;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
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
                throw new BadRequestException("Only users with role 'Professor' can be added.");

            var existingProfessor = await _professorRepository.GetByUserIdAsync(dto.UserId);
            if (existingProfessor != null)
                throw new BadRequestException("This user is already a professor.");

            var professor = new Professor
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId
            };

            await _professorRepository.AddAsync(professor);
            await _professorRepository.SaveChangesAsync();

            if (dto.SubjectIds.Any())
            {
                var selectedSubjects = await _subjectRepository.GetByIdsAsync(dto.SubjectIds);

                if (selectedSubjects.Count != dto.SubjectIds.Count)
                    throw new NotFoundException("One or more subjects not found.");

                foreach (var subject in selectedSubjects)
                {
                    subject.ProfessorId = professor.Id;
                    _subjectRepository.Update(subject);
                }

                await _subjectRepository.SaveChangesAsync();
            }

            var createdProfessor = await _professorRepository.GetByIdAsync(professor.Id);
            return MapToDto(createdProfessor!);
        }

        public async Task<ProfessorResponseDto> UpdateAsync(Guid id, UpdateProfessorDto dto)
        {
            await ValidateAsync(dto);

            var professor = await _professorRepository.GetByIdAsync(id);
            if (professor == null)
                throw new NotFoundException("Professor not found");

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (user.Role == null || user.Role.RoleName != "Professor")
                throw new BadRequestException("Only users with role 'Professor' can be assigned.");

            var existingProfessorForUser = await _professorRepository.GetByUserIdAsync(dto.UserId);
            if (existingProfessorForUser != null && existingProfessorForUser.Id != id)
                throw new BadRequestException("This user is already connected to another professor.");

            professor.UserId = dto.UserId;

            _professorRepository.Update(professor);
            await _professorRepository.SaveChangesAsync();

            var allSubjects = await _subjectRepository.GetAllAsync();

            var currentProfessorSubjects = allSubjects
                .Where(s => s.ProfessorId == professor.Id)
                .ToList();

            foreach (var subject in currentProfessorSubjects)
            {
                if (!dto.SubjectIds.Contains(subject.Id))
                {
                    subject.ProfessorId = null;
                    _subjectRepository.Update(subject);
                }
            }

            var newSubjects = allSubjects
                .Where(s => dto.SubjectIds.Contains(s.Id))
                .ToList();

            if (newSubjects.Count != dto.SubjectIds.Count)
                throw new NotFoundException("One or more subjects not found.");

            foreach (var subject in newSubjects)
            {
                subject.ProfessorId = professor.Id;
                _subjectRepository.Update(subject);
            }

            await _subjectRepository.SaveChangesAsync();

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
                Email = professor.User?.Email ?? string.Empty,
                Subjects = professor.Subjects
                    .Select(s => new ProfessorSubjectDto
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToList()
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