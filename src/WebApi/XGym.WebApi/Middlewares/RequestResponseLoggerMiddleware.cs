using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using XGym.Application.Common.Shared;

namespace XGym.WebApi.Middlewares
{
    public class RequestResponseLoggerMiddleware : IMiddleware
    {
        private readonly ILogger<RequestResponseLoggerMiddleware> _logger;

        public RequestResponseLoggerMiddleware(ILogger<RequestResponseLoggerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalResponseBody = context.Response.Body;

            using var tempResponseBody = new MemoryStream();
            context.Response.Body = tempResponseBody;

            context.Request.EnableBuffering();
            var requestBody = string.Empty;

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            requestBody = UpdateBodyToHidePassword(requestBody);

            try
            {
                await next(context);

                var userId = context.User.Identity?.IsAuthenticated == true ? context.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value : null;

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

                _logger.LogInformation($"Request : {context.Request.Method} {context.Request.Path} {context.Request.QueryString}" +
                $"\tBody: {requestBody}" +
                $"\tAcceptLanguage: {context.Request.Headers.AcceptLanguage}" +
                $"\tSchema: {context.Request.Scheme}" +
                $"\tHost: {context.Request.Host}" +
                $"\tResponseStatusCode: {context.Response.StatusCode}" +
                $"\tContentType: {context.Response.ContentType}" +
                $"\tHeaders: {FormatHeaders(context.Response.Headers)}" +
                $"\tResponseBody: {responseBody}" +
                $"\tUserId: {userId}\n");

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await context.Response.Body.CopyToAsync(originalResponseBody);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }

        }
        private Task HandleException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var routeData = httpContext.GetRouteData();
            var controllerName = routeData?.Values["controller"]?.ToString() ?? "Unknown";
            var actionName = routeData?.Values["action"]?.ToString() ?? "Unknown";

            var stackTrace = ex.StackTrace ?? "Stack trace not available";

            _logger.LogError(ex, $"[ERROR] {httpContext.Request.Method} {httpContext.Request.Path} - {controllerName}/{actionName}: {ex.Message} | StackTrace: {stackTrace}");

            var ErrorMessage = JsonSerializer.Serialize(new
            {
                error = "An unexpected error occurred. Please try again later.",
                details = new
                {
                    message = ex.Message,
                    controller = controllerName,
                    action = actionName,
                    stackTrace = stackTrace
                }
            }, new JsonSerializerOptions { WriteIndented = true });

            var result = new Response(ResponseCode.Fail, ErrorMessage);
            var serializedResult = JsonSerializer.Serialize(result);
            return httpContext.Response.WriteAsync(serializedResult);
        }
        private string UpdateBodyToHidePassword(string jsonString)
        {
            if (!string.IsNullOrEmpty(jsonString))
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    JsonObject root = JsonSerializer.Deserialize<JsonObject>(doc.RootElement.GetRawText());

                    if (root.ContainsKey("Password"))
                    {
                        root["Password"] = "*****";
                    }

                    return root.ToString();
                }
            }
            return jsonString;
        }
        private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
    }
}
