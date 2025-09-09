namespace Payment.API.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];

        using (logger.BeginScope(new Dictionary<string, object> { ["RequestId"] = requestId }))
        {
            // Log request
            await LogRequestAsync(context, requestId);

            // Capture original response stream
            var originalResponseStream = context.Response.Body;
            using var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            try
            {
                await next(context);
                stopwatch.Stop();

                // Log successful response
                await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds);

                // Copy response back to original stream
                responseStream.Seek(0, SeekOrigin.Begin);
                await responseStream.CopyToAsync(originalResponseStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                context.Response.Body = originalResponseStream;

                // Log error
                logger.LogError(ex,
                    "[{RequestId}] ERROR | {Method} {Path} | Duration: {Duration}ms | Exception: {ExceptionType}",
                    requestId, context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds, ex.GetType().Name);

                throw;
            }
        }
    }

    private async Task LogRequestAsync(HttpContext context, string requestId)
    {
        var request = context.Request;
        var requestBody = string.Empty;

        if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
        }

        logger.LogInformation(
            "[{RequestId}] REQUEST | {Method} {Path}{QueryString} | Headers: {Headers} | Body: {Body}",
            requestId,
            request.Method,
            request.Path,
            request.QueryString,
            string.Join(", ", request.Headers.Select(h => $"{h.Key}:{string.Join(",", [.. h.Value])}")),
            string.IsNullOrEmpty(requestBody) ? "empty" : requestBody
        );
    }

    private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMilliseconds)
    {
        var response = context.Response;
        var responseBody = string.Empty;

        if (response.Body.CanSeek)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            responseBody = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
        }

        var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
        var logMessage = response.StatusCode >= 400 ? "ERROR RESPONSE" : "SUCCESS";

        logger.Log(logLevel,
            "[{RequestId}] {LogType} | {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | Response: {Response}",
            requestId,
            logMessage,
            context.Request.Method,
            context.Request.Path,
            response.StatusCode,
            elapsedMilliseconds,
            string.IsNullOrEmpty(responseBody) ? "empty" : responseBody
        );
    }
}
