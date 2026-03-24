using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.RoleDtos;
using FluentValidation;

namespace ErefAIEnhancement.Validators
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName je obavezan.")
                .MaximumLength(50).WithMessage("RoleName ne sme biti duži od 50 karaktera.");
        }
    }
}