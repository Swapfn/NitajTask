using Domain.Dtos;
using FluentValidation;

namespace Application.Validator;
public class TodoItemValidator : AbstractValidator<CreateTodoItemDto>
{
    public TodoItemValidator()
    {
        RuleFor(x => x.Title).NotEmpty().NotNull();
    }
}
