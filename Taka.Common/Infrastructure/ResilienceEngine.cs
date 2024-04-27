using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using Serilog;

namespace Taka.Common.Infrastructure
{
    public class ResilienceEngine
    {
        private readonly AsyncPolicyWrap _policyWrapper;

        public ResilienceEngine()
        {
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    Log.Warning("Request failed with exception {ExceptionMessage} on retry {RetryAttempt} after waiting {WaitTime} seconds.", outcome.Message, retryAttempt + 1, timespan.TotalSeconds);
                });

            AsyncCircuitBreakerPolicy circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1),
                    onBreak: (exception, timeSpan) =>
                    {
                        Log.Warning("Circuit breaker opened. Exception: {Exception}.", exception.Message);
                    },
                    onReset: () => Log.Information("Circuit breaker reset."),
                    onHalfOpen: () => Log.Information("Circuit breaker half-open."));

            _policyWrapper = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        public async Task ExecuteOperationAsync(Func<Task> operation)
        {
            Log.Information("Attempting operation.");
            await ExecuteAsync(operation);
        }

        public async Task ExecuteOperationAsync<TInput>(Func<TInput, Task> operation, TInput input)
        {
            Log.Information("Attempting operation.");
            await ExecuteAsync(() => operation(input));
        }

        public async Task ExecuteOperationAsync<TInput1, TInput2>(Func<TInput1, TInput2, Task> operation, TInput1 input1, TInput2 input2)
        {
            Log.Information("Attempting operation.");
            await ExecuteAsync(() => operation(input1, input2));
        }

        public async Task ExecuteOperationAsync<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, Task> operation, TInput1 input1, TInput2 input2, TInput3 input3)
        {
            Log.Information("Attempting operation.");
            await ExecuteAsync(() => operation(input1, input2, input3));
        }


        public async Task<TResult> ExecuteOperationAsync<TResult>(Func<Task<TResult>> operation)
        {
            Log.Information("Attempting operation : {Method}", operation.Method.Name);
            return await ExecuteAsync(operation);
        }

        public async Task<TResult> ExecuteOperationAsync<TInput, TResult>(Func<TInput, Task<TResult>> operation, TInput input)
        {
            Log.Information("Attempting operation : {Method}", operation.Method.Name);
            return await ExecuteAsync(() => operation(input));
        }

        public async Task<TResult> ExecuteOperationAsync<TInput1, TInput2, TResult>(Func<TInput1, TInput2, Task<TResult>> operation, TInput1 input1, TInput2 input2)
        {
            Log.Information("Attempting operation : {Method}", operation.Method.Name);
            return await ExecuteAsync(() => operation(input1, input2));
        }

        public async Task<TResult> ExecuteOperationAsync<TInput1, TInput2, TInput3, TResult>(Func<TInput1, TInput2, TInput3, Task<TResult>> operation, TInput1 input1, TInput2 input2, TInput3 input3)
        {
            Log.Information("Attempting operation : {Method}", operation.Method.Name);
            return await ExecuteAsync(() => operation(input1, input2, input3));
        }                

        private async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation)
        {
            try
            {
                TResult result = await _policyWrapper.ExecuteAsync(operation);
                Log.Information("{Method}. Operation succeeded.", operation.Method.Name);
                return result;
            }
            catch (Exception ex)
            {                
                Log.Error(ex, "{Method}. Operation ultimately failed after retries and circuit breaker actions.", operation.Method.Name);
                throw;
            }
        }

        private async Task ExecuteAsync(Func<Task> operation)
        {
            try
            {
                await _policyWrapper.ExecuteAsync(operation);
                Log.Information("Operation succeeded.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation ultimately failed after retries and circuit breaker actions.");
                throw;
            }
        }
    }
}