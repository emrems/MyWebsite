using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyWebsite.Dtos.Comment;

namespace MyWebsite.Validator.Comment
{
    public class CreateCommentValidator : AbstractValidator<createCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(x=>x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz")
                 .MinimumLength(1).WithMessage("İçerik alanı en az 1 karakter olmalıdır.") 
                .MaximumLength(30).WithMessage("İçerik alanı en fazla 30 karakter olabilir.");
            RuleFor(x => x.ArticleId)
                .NotEmpty().WithMessage("ArticleId boş olamaz");

            RuleFor(x => x.UserId)
               .NotEmpty().WithMessage("UserId boş olamaz");
        }
    }
}
