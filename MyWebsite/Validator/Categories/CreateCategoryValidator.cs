using FluentValidation;
using MyWebsite.Dtos.CategoryDtos;

namespace MyWebsite.Validator.Categories
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDtos>
    {
        public CreateCategoryValidator()
        {
            RuleFor(c => c).NotNull().WithMessage("Kategori bilgileri boş olamaz");
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MaximumLength(15).WithMessage("Kategori adı en fazla 100 karakter olabilir.");
            RuleFor(c => c.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz.")
                .MaximumLength(10).WithMessage("Slug en fazla 100 karakter olabilir.");

        }
    }
}
