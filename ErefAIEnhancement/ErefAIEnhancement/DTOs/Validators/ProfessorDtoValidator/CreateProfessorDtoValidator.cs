using ErefAIEnhancement.DTOs.ProfessorDtos;
using FluentValidation;

namespace ErefAIEnhancement.Validators.ProfessorValidators
{
    public class CreateProfessorDtoValidator : AbstractValidator<CreateProfessorDto>
    {
        public CreateProfessorDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.SubjectIds)
                .NotNull().WithMessage("SubjectIds list is required.");
        }
    }
}