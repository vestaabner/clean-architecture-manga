namespace WebApi.UseCases.V1
{
    using System.Linq;
    using Application.Services;
    using Microsoft.AspNetCore.Mvc;

    public static class PresenterUtils
    {
        public static IActionResult Invalid(Notification notification)
        {
            var errorMessages = notification
                .ErrorMessages
                .ToDictionary(item => item.Key, item => item.Value.ToArray());

            var problemDetails = new ValidationProblemDetails(errorMessages);
            return new BadRequestObjectResult(problemDetails);
        }

        public static IActionResult NotFound() => new NotFoundObjectResult("Resource not found.");

        public static IActionResult Ok(object value) => new OkObjectResult(value);

        public static IActionResult Created(object routeValues, object value) =>
            new CreatedAtRouteResult(routeValues, value);
    }
}
