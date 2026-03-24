using ErefAIEnhancement.DTOs.ProfessorDtos;
using FluentValidation;

namespace ErefAIEnhancement.Validators.ProfessorValidators
{
    public class UpdateProfessorDtoValidator : AbstractValidator<UpdateProfessorDto>
    {
        public UpdateProfessorDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.SubjectIds)
                .NotNull().WithMessage("SubjectIds list is required.");
        }
    }
}