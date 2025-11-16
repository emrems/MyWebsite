using FluentValidation;
using MyWebsite.Dtos.MessageDtos;

namespace MyWebsite.Validator.Message
{
    public class MessageDtosValidator : AbstractValidator<MessageDtos>
    {
        public MessageDtosValidator()
        {
            // Email Kuralı
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş olamaz.")
                .MaximumLength(30).WithMessage("Email alanı en fazla 30 karakter olabilir.");

            // Name Kuralı: MinimumLength(1) eklendi
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim alanı boş olamaz.")
                .MinimumLength(1).WithMessage("İsim alanı en az 1 karakter olmalıdır.") // Yeni Kural
                .MaximumLength(30).WithMessage("İsim alanı en fazla 30 karakter olabilir.");

            // Content Kuralı: MinimumLength(1) eklendi
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik alanı boş olamaz.")
                .MinimumLength(1).WithMessage("İçerik alanı en az 1 karakter olmalıdır.") // Yeni Kural
                .MaximumLength(100).WithMessage("İçerik alanı en fazla 100 karakter olabilir.");

        }
    }
}