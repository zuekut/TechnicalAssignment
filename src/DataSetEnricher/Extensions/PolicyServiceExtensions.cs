using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

namespace CardanoAssignment.Extensions;

public static class PolicyServiceExtensions
{
    public static void AddPolicyRegistries(this IServiceCollection services)
    {
        var logger = Log.Logger;
        var registry = services.AddPolicyRegistry();
        var incrementalWaitTimesRequests = new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5)
        };
        registry.Add("DefaultWaitAndRetryStrategy",
            HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(incrementalWaitTimesRequests, (responseMessage, timeSpan, retryCount, _) =>
                {
                    if (responseMessage.Result != null)
                    {
                        var serverResponse = responseMessage.Result.Content.ReadAsStringAsync().Result;
                        logger.Warning("Request failed with Status: {StatusCode}. Waiting {WaitTime} seconds before next retry. Retry attempt {RetryAttempt}" +
                                       "\nServerResponse:{Response}" +
                                       "\nException:{@Exception}", responseMessage.Result.StatusCode, timeSpan.TotalSeconds, retryCount, serverResponse, responseMessage.Exception);
                    }
                    else
                    {
                        logger.Error("Request failed unexpectedly (Possible timeout). Waiting {WaitTime} seconds before next retry. Retry attempt {RetryAttempt}" +
                                     "\nException:{@Exception}", timeSpan.TotalSeconds, retryCount, responseMessage.Exception);
                    }
                }));

        //Further requests are blocked for 30 seconds if five failed attempts occur sequentially.
        registry.Add("DefaultCircuitBreaker",
            HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30), (responseMessage, span) =>
                {
                    if (responseMessage.Result != null)
                    {
                        var serverResponse = responseMessage.Result.Content.ReadAsStringAsync().Result;
                        logger.Error("Request failed with Status: {StatusCode}. The circuit was broken and will remain open for the next {WaitTime} seconds." +
                                     "\nServerResponse:{Response}" +
                                     "\nException:{@Exception}", responseMessage.Result.StatusCode, span, serverResponse, responseMessage.Exception);
                    }
                    else
                    {
                        logger.Error("Request failed unexpectedly (Possible timeout). The circuit was broken and will remain open for the next {WaitTime} seconds." +
                                     "\nException:{@Exception}", span, responseMessage.Exception);
                    }
                }, null));
    }
}