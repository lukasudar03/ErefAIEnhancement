using ErefAIEnhancement.DTOs.SubjectDtos;
using ErefAIEnhancement.Exceptions;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;

namespace ErefAIEnhancement.Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IServiceProvider _serviceProvider;

        public SubjectService(
            ISubjectRepository subjectRepository,
            IProfessorRepository professorRepository,
            IServiceProvider serviceProvider)
        {
            _subjectRepository = subjectRepository;
            _professorRepository = professorRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<SubjectResponseDto>> GetAllAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return subjects.Select(MapToDto).ToList();
        }

        public async Task<SubjectResponseDto> GetByIdAsync(Guid id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
                throw new NotFoundException("Subject not found");

            return MapToDto(subject);
        }

        public async Task<SubjectResponseDto> CreateAsync(CreateSubjectDto dto)
        {
            await ValidateAsync(dto);

            var subject = new Subject
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                YearOfStudy = dto.YearOfStudy,
                Department = dto.Department
            };

            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();

            var createdSubject = await _subjectRepository.GetByIdAsync(subject.Id);
            return MapToDto(createdSubject!);
        }

        public async Task<SubjectResponseDto> UpdateAsync(Guid id, UpdateSubjectDto dto)
        {
            await ValidateAsync(dto);

            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
                throw new NotFoundException("Subject not found");

            subject.Name = dto.Name;
            subject.YearOfStudy = dto.YearOfStudy;
            subject.Department = dto.Department;

            _subjectRepository.Update(subject);
            await _subjectRepository.SaveChangesAsync();

            var updatedSubject = await _subjectRepository.GetByIdAsync(id);
            return MapToDto(updatedSubject!);
        }

        public async Task DeleteAsync(Guid id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
                throw new NotFoundException("Subject not found");

            _subjectRepository.Delete(subject);
            await _subjectRepository.SaveChangesAsync();
        }

        private static SubjectResponseDto MapToDto(Subject subject)
        {
            return new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                ProfessorId = subject.ProfessorId ?? null,
                YearOfStudy = subject.YearOfStudy,
                Department = subject.Department,
                ProfessorName = subject.Professor?.User?.Name ?? string.Empty,
                ProfessorEmail = subject.Professor?.User?.Email ?? string.Empty
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