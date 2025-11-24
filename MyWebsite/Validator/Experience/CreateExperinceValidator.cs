using FluentValidation;
using MyWebsite.Dtos.Experince;

namespace MyWebsite.Validator.Experience
{
    public class CreateExperinceValidator : AbstractValidator<CreateExperinceDtos>
    {
        public CreateExperinceValidator()
        {
            RuleFor(x => x.Company)
                .NotEmpty().WithMessage("Şirket adı boş olamaz.")
                .MaximumLength(100).WithMessage("Şirket adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Pozisyon (role) boş olamaz.")
                .MaximumLength(100).WithMessage("Pozisyon en fazla 100 karakter olabilir.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Başlangıç tarihi boş olamaz.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Başlangıç tarihi gelecekte olamaz.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("Bitiş tarihi başlangıç tarihinden büyük olmalıdır.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.")
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");
        }
    }
}
