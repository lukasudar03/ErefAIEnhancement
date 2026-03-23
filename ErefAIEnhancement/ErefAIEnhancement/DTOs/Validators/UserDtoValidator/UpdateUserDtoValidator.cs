using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.UserDtos;
using FluentValidation;

namespace ErefAIEnhancement.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name je obavezan.")
                .MaximumLength(100).WithMessage("Name ne sme biti duži od 100 karaktera.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan.")
                .EmailAddress().WithMessage("Email nije u ispravnom formatu.")
                .MaximumLength(150).WithMessage("Email ne sme biti duži od 150 karaktera.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password je obavezan.")
                .MinimumLength(6).WithMessage("Password mora imati najmanje 6 karaktera.")
                .MaximumLength(100).WithMessage("Password ne sme biti duži od 100 karaktera.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId je obavezan.");
        }
    }
}