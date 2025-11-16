using FluentValidation;
using MyWebsite.Dtos.UserDtos;

namespace MyWebsite.Validator.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(opt => opt.UserName)
                .NotEmpty().WithMessage("UserName boş olamaz.")
                .MaximumLength(30).WithMessage("UserName en fazla 30 karakter olabilir.");

            RuleFor(opt => opt.UserEmail)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.") 
                .MaximumLength(20).WithMessage("Email en fazla 20 karakter olabilir.");
        }
    }
}
