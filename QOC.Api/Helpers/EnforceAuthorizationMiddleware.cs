namespace QOC.Api.Helpers
{
    public class EnforceAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public EnforceAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Skip authorization for these specific paths
            var skipAuthPaths = new[]
            {
            "/api/auth/login",
            "/api/auth/register"
        };

            if ((HttpMethods.IsPost(context.Request.Method) ||
                 HttpMethods.IsPut(context.Request.Method) ||
                 HttpMethods.IsDelete(context.Request.Method))
                && !context.User.Identity.IsAuthenticated
                && !skipAuthPaths.Contains(path))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }
}
