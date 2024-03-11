using Rhyous.EntityAnywhere.Interfaces.Tools;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Tools
{
    /// <summary>
    /// This class runs asynchronous tasks synchronously
    /// </summary>
    public class TaskRunner
    {
        #region Task
        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="ignoreException">A bool value that ignores the exception.</param>
        public static void RunSynchonously(Func<Task> method, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method();
                task.Wait();
            }
            catch (Exception e) { ExceptionUnwrapper.HandleException<AggregateException>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input">The input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception.</param>
        public static void RunSynchonously<T>(Func<T, Task> method, T input, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input);
                task.Wait();
            }
            catch (Exception e) { ExceptionUnwrapper.HandleException<AggregateException>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception.</param>
        public static void RunSynchonously<T1, T2>(Func<T1, T2, Task> method, T1 input1, T2 input2, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2);
                task.Wait();
            }
            catch (Exception e) { ExceptionUnwrapper.HandleException<AggregateException>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception.</param>
        public static void RunSynchonously<T1, T2, T3>(Func<T1, T2, T3, Task> method, T1 input1, T2 input2, T3 input3, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3);
                task.Wait();
            }
            catch (Exception e) { ExceptionUnwrapper.HandleException<AggregateException>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="input4">The fourth input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception.</param>
        public static void RunSynchonously<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> method, T1 input1, T2 input2, T3 input3, T4 input4, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3, input4);
                task.Wait();
            }
            catch (Exception e) { ExceptionUnwrapper.HandleException<AggregateException>(e, ignoreException); }
        }
        // If you need more parameters than four, just add another overload with a T5 generic here.
        #endregion

        #region Task<TResult>
        /// <summary>
        /// Runs an asynchronous task with no parameters synchronously.
        /// </summary>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="ignoreException">A bool value that ignores the exception.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<TResult>(Func<Task<TResult>> method, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method();
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with one parameter synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input">The input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T, TResult>(Func<T, Task<TResult>> method, T input, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T2 is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, TResult>(Func<T1, T2, Task<TResult>> method, T1 input1, T2 input2, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T3 is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> method, T1 input1, T2 input2, T3 input3, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="T3">The type of the fourth input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="input4">The fourth input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T4 is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, Task<TResult>> method, T1 input1, T2 input2, T3 input3, T4 input4, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3, input4);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
        /// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="input4">The fourth input parameter value.</param>
        /// <param name="input5">The fifth input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T4 is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, Task<TResult>> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3, input4, input5);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
        /// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
        /// <typeparam name="T6">The type of the sixth input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="input4">The fourth input parameter value.</param>
        /// <param name="input5">The fifth input parameter value.</param>
        /// <param name="input6">The sixth input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T4 is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, Task<TResult>> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5, T6 input6, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3, input4, input5, input6);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        /// <summary>
        /// Runs an asynchronous task with two parameters synchronously.
        /// </summary>
        /// <typeparam name="T1">The type of the first input parameter.</typeparam>
        /// <typeparam name="T2">The type of the second input parameter.</typeparam>
        /// <typeparam name="T3">The type of the third input parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth input parameter.</typeparam>
        /// <typeparam name="T5">The type of the fifth input parameter.</typeparam>
        /// <typeparam name="T6">The type of the sixth input parameter.</typeparam
        /// <typeparam name="T7">The type of the seventh input parameter.</typeparam>
        /// <typeparam name="TResult">The return type of the asynchronous call. Not Task{T} but just T.</typeparam>
        /// <param name="method">The asynchronous method.</param>
        /// <param name="input1">The first input parameter value.</param>
        /// <param name="input2">The second input parameter value.</param>
        /// <param name="input3">The third input parameter value.</param>
        /// <param name="input4">The fourth input parameter value.</param>
        /// <param name="input5">The fifth input parameter value.</param>
        /// <param name="input6">The sixth input parameter value.</param>
        /// <param name="input7">The seventh input parameter value.</param>
        /// <param name="ignoreException">A bool value that ignores the exception. If T4 is also bool, then you have to provide this.</param>
        /// <returns>The result of the asynchonous method.</returns>
        public static TResult RunSynchonously<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5, T6 input6, T7 input7, bool ignoreException = false)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            try
            {
                var task = method(input1, input2, input3, input4, input5, input6, input7);
                task.Wait();
                return task.Result;
            }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<AggregateException, TResult>(e, ignoreException); }
        }

        // If you need more parameters than five, just add another overload with a T5 generic here.
        #endregion

    }
}