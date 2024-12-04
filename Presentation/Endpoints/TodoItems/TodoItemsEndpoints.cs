using Application.Services.Contracts;
using Domain.Dtos;
using Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints.TodoItems;

public static class TodoItemsEndpoints
{
    public const string Name = "TodoItems";
    public const string Url = "/api/todo";
    public const string CompleteUrl = $"{Url}/{{id}}/complete";
    public const string PendingUrl = $"{Url}/pending";

    public static IEndpointRouteBuilder MapTodoItemsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(Url, async Task<Results<Ok<Result>, NotFound>> ([FromBody] CreateTodoItemDto model, ITodoItemService service) =>
        {
            var result = await service.AddTodoItem(model);
            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }).WithTags(Name);

        app.MapPut(CompleteUrl, async Task<Results<Ok<Result>, NotFound>> (int id, ITodoItemService service) =>
        {
            var result = await service.Complete(id);
            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }).WithTags(Name);
        app.MapGet(Url, async Task<Results<Ok<Result>, NotFound>> (ITodoItemService service) =>
        {
            var result = await service.GetTodoItems();
            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }).WithTags(Name);
        app.MapGet(PendingUrl, async Task<Results<Ok<Result>, NotFound>> (ITodoItemService service) =>
        {
            var result = await service.GetPendingTodoItems();
            return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
        }).WithTags(Name);
        return app;
    }
}
