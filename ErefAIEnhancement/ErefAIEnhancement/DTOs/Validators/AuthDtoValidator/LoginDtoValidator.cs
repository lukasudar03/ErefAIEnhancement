using ErefAIEnhancement.DTOs.AuthDtos;
using FluentValidation;

namespace ErefAIEnhancement.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan.")
                .EmailAddress().WithMessage("Email nije u ispravnom formatu.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password je obavezan.");
        }
    }
}