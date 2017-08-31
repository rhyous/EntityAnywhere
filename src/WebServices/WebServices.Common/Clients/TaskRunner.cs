using System;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class TaskRunner
    {
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

        // If you need more parameters than three, just add another one here.
    }
}
