using Application.Services;
using Application.Services.Contracts;
using Application.Validator;
using Domain.Dtos;
using FluentValidation;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Shared;
using Infrastructure.Repositories.Shared.Contracts;

namespace Presentation;

public static class ServiceInjection
{
    public static WebApplicationBuilder InjectServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ITodoItemService, TodoItemService>();
        builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        builder.Services.AddScoped<IValidator<CreateTodoItemDto>, TodoItemValidator>();
        return builder;
    }
}
