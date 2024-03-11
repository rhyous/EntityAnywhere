using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Tools
{
    /// <summary>
    /// This class runs code and if that code fails, it tries again. 
    /// By default it will retry three times with a default 150 millescond delay between tries.
    /// Both the retries and the delay are parameters.
    /// </summary>
    public class Retry : ILogProperty
    {
        public const int DefaultRetries = 2;
        public const int DefaultRetryDelay = 150;

        public Retry() { }
        public Retry(ILogger logger) { Logger = logger; }

        public ILogger Logger { get; set; }

        /// <summary>
        /// Run code and if it fails, retry it as many as three times. If it continues to fail,
        /// throw the exception.
        /// </summary>
        /// <param name="method">The method to retry.</param>
        /// <param name="retries">The number of times to retry.</param>
        /// <param name="delay">The number of miliseconds to delay between retries.</param>
        /// <returns>The task if not awaited, void if awaited.</returns>
        public async Task RetryAsync(Func<Task> method, int retries = DefaultRetries, int delay = DefaultRetryDelay)
        {
            try
            {
                if (method == null)
                {
                    Logger?.Debug("RetryAsync cannot be called with a null method");
                    throw new ArgumentNullException("method");
                }
                await method();
            }
            catch (Exception e)
            {
                Logger?.Debug(e.Message);
                if (retries > 0)
                {
                    Logger?.Debug("Retrying . . .");
                    await Task.Delay(delay);
                    await RetryAsync(method, --retries);
                    return;
                }
                throw;
            }
        }

        public async Task<TResult> RetryAsync<TResult>(Func<Task<TResult>> method, int retries = DefaultRetries, int delay = DefaultRetryDelay)
        {
            try
            {
                if (method == null)
                {
                    Logger?.Debug("RetryAsync cannot be called with a null method");
                    throw new ArgumentNullException("method");
                }
                return await method();
            }
            catch (Exception e)
            {
                Logger?.Debug(e.Message);
                if (retries > 0)
                {
                    Logger?.Debug("Retrying . . .");
                    await Task.Delay(delay);
                    return await RetryAsync(method, --retries);
                }
                throw;
            }
        }

        public async Task<TResult> RetryAsync<T, TResult>(Func<T, Task<TResult>> method, T input, int retries = DefaultRetries, int delay = DefaultRetryDelay)
        {
            try
            {
                if (method == null)
                {
                    Logger?.Debug("RetryAsync cannot be called with a null method");
                    throw new ArgumentNullException("method");
                }
                return await method(input);
            }
            catch (Exception e)
            {
                Logger?.Debug(e.Message);
                if (retries > 0)
                {
                    Logger?.Debug("Retrying . . .");
                    await Task.Delay(delay);
                    return await RetryAsync(method, input, --retries);
                }
                throw;
            }
        }

        public async Task<TResult> RetryAsync<T1, T2, TResult>(Func<T1, T2, Task<TResult>> method, T1 input1, T2 input2, int retries = DefaultRetries, int delay = DefaultRetryDelay)
        {
            try
            {
                if (method == null)
                {
                    Logger?.Debug("RetryAsync cannot be called with a null method");
                    throw new ArgumentNullException("method");
                }
                return await method(input1, input2);
            }
            catch (Exception e)
            {
                Logger?.Debug(e.Message);
                if (retries > 0)
                {
                    Logger?.Debug("Retrying . . .");
                    await Task.Delay(delay);
                    return await RetryAsync(method, input1, input2, --retries);
                }
                throw;
            }
        }

        public async Task<TResult> RetryAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> method, T1 input1, T2 input2, T3 input3, int retries = DefaultRetries, int delay = DefaultRetryDelay)
        {
            try
            {
                if (method == null)
                {
                    Logger?.Debug("RetryAsync cannot be called with a null method");
                    throw new ArgumentNullException("method");
                }
                return await method(input1, input2, input3);
            }
            catch (Exception e)
            {
                Logger?.Debug(e.Message);
                if (retries > 0)
                {
                    Logger?.Debug("Retrying . . .");
                    await Task.Delay(delay);
                    return await RetryAsync(method, input1, input2, input3, --retries);
                }
                throw;
            }
        }

        public async Task<TResult> RetryAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, Task<TResult>> method, T1 input1, T2 input2, T3 input3, T4 input4, int retries = DefaultRetries, int delay = DefaultRetryDelay)
        {
            try
            {
                if (method == null)
                {
                    Logger?.Debug("RetryAsync cannot be called with a null method");
                    throw new ArgumentNullException("method");
                }
                return await method(input1, input2, input3, input4);
            }
            catch (Exception e)
            {
                Logger?.Debug(e.Message);
                if (retries > 0)
                {
                    Logger?.Debug("Retrying . . .");
                    await Task.Delay(delay);
                    return await RetryAsync(method, input1, input2, input3, input4, --retries);
                }
                throw;
            }
        }

        // If you need more parameters than four, just add another overload with a T5 generic here.
    }
}
