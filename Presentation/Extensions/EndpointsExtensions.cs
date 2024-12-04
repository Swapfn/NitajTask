using Presentation.Endpoints.TodoItems;

namespace Presentation.Extensions;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapTodoItemsEndpoints();
        return app;
    }
}