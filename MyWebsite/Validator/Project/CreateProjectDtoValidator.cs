using FluentValidation;
using MyWebsite.Dtos.ProjectDtos;

namespace MyWebsite.Validator.Project
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDtos>
    {
        public CreateProjectDtoValidator()
        {
           
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Proje başlığı boş olamaz.")
                .MaximumLength(100).WithMessage("Proje başlığı en fazla 100 karakter olabilir.");

    
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama alanı boş olamaz.")
                .MaximumLength(1000).WithMessage("Açıklama alanı en fazla 1000 karakter olabilir.");

         
            RuleFor(x => x.Technologies)
                .NotEmpty().WithMessage("Kullanılan teknolojiler boş olamaz.")
                .MaximumLength(300).WithMessage("Kullanılan teknolojiler en fazla 300 karakter olabilir.");


            RuleFor(x => x.GithubUrl)
                .NotEmpty().WithMessage("Github adresi boş olamaz.")
                .MaximumLength(500).WithMessage("Github adresi en fazla 500 karakter olabilir.");
               

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Kategori seçimi zorunludur.")
                .GreaterThan(0).WithMessage("Kategori Id 0'dan büyük olmalıdır.");
        }

        
    }
}
