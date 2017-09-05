using System;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// This class runs asynchronous tasks synchronously
    /// </summary>
    public class TaskRunner
    {
        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<TResult>(Func<Task<TResult>> method)
        {
            try
            {
                var task = method();
                task.Wait();
                return task.Result;
            }
            catch { return default(TResult); }
        }

        /// <summary>
        /// Runs an asynchronous task with one parameter synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input">The input parameter value.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T, TResult>(Func<T, Task<TResult>> method, T input)
        {
            try
            {
                var task = method(input);
                task.Wait();
                return task.Result;
            }
            catch { return default(TResult); }
        }
        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input">The input parameter value.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, TResult>(Func<T1, T2, Task<TResult>> method, T1 input1, T2 input2)
        {
            try
            {
                var task = method(input1, input2);
                task.Wait();
                return task.Result;
            }
            catch { return default(TResult); }
        }
        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input">The input parameter value.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> method, T1 input1, T2 input2, T3 input3)
        {
            try
            {
                var task = method(input1, input2, input3);
                task.Wait();
                return task.Result;
            }
            catch { return default(TResult); }
        }

        // If you need more parameters than three, just add another overload with a T4 generic here.
    }
}
