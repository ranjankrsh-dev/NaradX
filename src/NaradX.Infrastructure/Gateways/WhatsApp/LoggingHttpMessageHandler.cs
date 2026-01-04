using Microsoft.Extensions.Logging;

namespace NaradX.Infrastructure.Gateways.WhatsApp;

public class LoggingHttpMessageHandler : HttpClientHandler
{
    private readonly ILogger<LoggingHttpMessageHandler> _logger;

    public LoggingHttpMessageHandler(ILogger<LoggingHttpMessageHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log request
        var requestContent = request.Content != null 
            ? await request.Content.ReadAsStringAsync(cancellationToken) 
            : "No content";

        _logger.LogInformation(
            "WhatsApp API Request:\nMethod: {Method}\nUrl: {Url}\nHeaders: {Headers}\nBody: {Body}",
            request.Method,
            request.RequestUri,
            string.Join(", ", request.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}")),
            requestContent);

        // Send the request
        var response = await base.SendAsync(request, cancellationToken);

        // Log response
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogInformation(
            "WhatsApp API Response:\nStatusCode: {StatusCode}\nReasonPhrase: {ReasonPhrase}\nBody: {Body}",
            response.StatusCode,
            response.ReasonPhrase,
            responseContent);

        return response;
    }
}
