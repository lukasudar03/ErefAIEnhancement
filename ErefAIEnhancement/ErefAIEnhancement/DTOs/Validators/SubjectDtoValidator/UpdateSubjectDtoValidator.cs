using ErefAIEnhancement.DTOs.SubjectDtos;
using FluentValidation;

namespace ErefAIEnhancement.Validators.SubjectValidators
{
    public class UpdateSubjectDtoValidator : AbstractValidator<UpdateSubjectDto>
    {
        public UpdateSubjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Subject name is required.")
                .MaximumLength(100).WithMessage("Subject name must not exceed 100 characters.");

            RuleFor(x => x.YearOfStudy)
                .IsInEnum().WithMessage("Invalid YearOfStudy value.");

            RuleFor(x => x.Department)
                .IsInEnum().WithMessage("Invalid Department value.");
        }
    }
}