using FluentValidation;

public class BookValidator : AbstractValidator<BookRequest>
{
    public BookValidator() {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
    }
}