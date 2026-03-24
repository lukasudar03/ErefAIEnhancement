using ErefAIEnhancement.DTOs.StudentDto;
using FluentValidation;

namespace ErefAIEnhancement.Validators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId je obavezan.");

            RuleFor(x => x.IndexNumber)
                .NotEmpty().WithMessage("Index number je obavezan.")
                .MaximumLength(30);

            RuleFor(x => x.YearOfStudy)
                .IsInEnum().WithMessage("YearOfStudy nije validan.");

            RuleFor(x => x.Department)
                .IsInEnum().WithMessage("Department nije validan.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).WithMessage("Date of birth mora biti u prošlosti.");
        }
    }
}