using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.StudentDto;
using FluentValidation;

namespace ErefAIEnhancement.Validators
{
    public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
    {
        public UpdateStudentDtoValidator()
        {
            RuleFor(x => x.IndexNumber)
                .NotEmpty().WithMessage("IndexNumber je obavezan.")
                .MaximumLength(30).WithMessage("IndexNumber ne sme biti duži od 30 karaktera.");

            RuleFor(x => x.YearOfStudy)
                .IsInEnum().WithMessage("YearOfStudy nije validna vrednost.");

            RuleFor(x => x.Department)
                .IsInEnum().WithMessage("Department nije validna vrednost.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).WithMessage("DateOfBirth mora biti u prošlosti.");
        }
    }
}