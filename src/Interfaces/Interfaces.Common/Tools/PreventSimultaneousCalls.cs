using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Tools
{
    public abstract class PreventSimultaneousCallsBase<TActionOrFunc, TTask>
    {
        internal protected readonly ConcurrentDictionary<TActionOrFunc, TTask> RunningTasks = new ConcurrentDictionary<TActionOrFunc, TTask>();
        internal protected readonly ConcurrentDictionary<TActionOrFunc, bool> RunningMethods = new ConcurrentDictionary<TActionOrFunc, bool>();
    }

    #region Action

    public interface IPreventSimultaneousActionCalls
    {
        void Call(Action method); 
        Task CallAsync(Action method);
    }
    public class PreventSimultaneousActionCalls : PreventSimultaneousCallsBase<Action, Task>, IPreventSimultaneousActionCalls
    {
        public void Call(Action method)
        {
            if (RunningMethods.TryGetValue(method, out bool _))
                return;

            if (RunningMethods.TryAdd(method, true))
            {
                method();
                RunningMethods.TryRemove(method, out bool _);
            }
        }

        public Task CallAsync(Action method)
        {
            if (RunningTasks.TryGetValue(method, out Task task))
                return task;

            task = new Task(() =>
            {
                method();
                RunningTasks.TryRemove(method, out Task _);
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }

            return RunningTasks[method];

        }
    }

    public interface IPreventSimultaneousActionCalls<T1>
    {
        void Call(Action<T1> method, T1 intput1);
        Task CallAsync(Action<T1> method, T1 intput1);
    }
    public class PreventSimultaneousActionCalls<T1> : PreventSimultaneousCallsBase<Action<T1>, Task>, IPreventSimultaneousActionCalls<T1>
    {
        public void Call(Action<T1> method, T1 input1)
        {
            if (RunningMethods.TryGetValue(method, out bool _))
                return;

            if (RunningMethods.TryAdd(method, true))
            {
                method(input1);
                RunningMethods.TryRemove(method, out bool _);
            }
        }

        public Task CallAsync(Action<T1> method, T1 input1)
        {
            if (RunningTasks.TryGetValue(method, out Task task))
                return task;

            task = new Task(() =>
            {
                method(input1);
                RunningTasks.TryRemove(method, out Task _);
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }

    public interface IPreventSimultaneousActionCalls<T1, T2>
    {
        void Call(Action<T1, T2> method, T1 intput1, T2 input2);
        Task CallAsync(Action<T1, T2> method, T1 intput1, T2 input2);
    }
    public class PreventSimultaneousActionCalls<T1, T2> : PreventSimultaneousCallsBase<Action<T1, T2>, Task>, IPreventSimultaneousActionCalls<T1, T2>
    {
        public void Call(Action<T1, T2> method, T1 input1, T2 input2)
        {
            if (RunningMethods.TryGetValue(method, out bool _))
                return;

            if (RunningMethods.TryAdd(method, true))
            {
                method(input1, input2);
                RunningMethods.TryRemove(method, out bool _);
            }
        }

        public Task CallAsync(Action<T1, T2> method, T1 input1, T2 input2)
        {
            if (RunningTasks.TryGetValue(method, out Task task))
                return task;

            task = new Task(() =>
            {
                method(input1, input2);
                RunningTasks.TryRemove(method, out Task _);
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }

    public interface IPreventSimultaneousActionCalls<T1, T2, T3>
    {
        void Call(Action<T1, T2, T3> method, T1 intput1, T2 input2, T3 input3);
        Task CallAsync(Action<T1, T2, T3> method, T1 intput1, T2 input2, T3 input3);
    }
    public class PreventSimultaneousActionCalls<T1, T2, T3> : PreventSimultaneousCallsBase<Action<T1, T2, T3>, Task>, IPreventSimultaneousActionCalls<T1, T2, T3>
    {
        public void Call(Action<T1, T2, T3> method, T1 input1, T2 input2, T3 input3)
        {
            if (RunningMethods.TryGetValue(method, out bool _))
                return;
            if (RunningMethods.TryAdd(method, true))
            {
                method(input1, input2, input3);
                RunningMethods.TryRemove(method, out bool _);
            }
        }

        public Task CallAsync(Action<T1, T2, T3> method, T1 input1, T2 input2, T3 input3)
        {
            if (RunningTasks.TryGetValue(method, out Task task))
                return task;
            task = new Task(() =>
            {
                method(input1, input2, input3);
                RunningTasks.TryRemove(method, out Task _);
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }
    public interface IPreventSimultaneousActionCalls<T1, T2, T3, T4>
    {
        void Call(Action<T1, T2, T3, T4> method, T1 intput1, T2 input2, T3 input3, T4 input4);
        Task CallAsync(Action<T1, T2, T3, T4> method, T1 intput1, T2 input2, T3 input3, T4 input4);
    }
    public class PreventSimultaneousActionCalls<T1, T2, T3, T4> : PreventSimultaneousCallsBase<Action<T1, T2, T3, T4>, Task>,
                                                                  IPreventSimultaneousActionCalls<T1, T2, T3, T4>
    {
        public void Call(Action<T1, T2, T3, T4> method, T1 input1, T2 input2, T3 input3, T4 input4)
        {
            if (RunningMethods.TryGetValue(method, out bool _))
                return;
            if (RunningMethods.TryAdd(method, true))
            {
                method(input1, input2, input3, input4);
                RunningMethods.TryRemove(method, out bool _);
            }
        }

        public Task CallAsync(Action<T1, T2, T3, T4> method, T1 input1, T2 input2, T3 input3, T4 input4)
        {
            if (RunningTasks.TryGetValue(method, out Task task))
                return task;
            task = new Task(() =>
            {
                method(input1, input2, input3, input4);
                RunningTasks.TryRemove(method, out Task _);
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }
    public interface IPreventSimultaneousActionCalls<T1, T2, T3, T4, T5>
    {
        void Call(Action<T1, T2, T3, T4, T5> method, T1 intput1, T2 input2, T3 input3, T4 input4, T5 input5);
        Task CallAsync(Action<T1, T2, T3, T4, T5> method, T1 intput1, T2 input2, T3 input3, T4 input4, T5 input5);
    }
    public class PreventSimultaneousActionCalls<T1, T2, T3, T4, T5> : PreventSimultaneousCallsBase<Action<T1, T2, T3, T4, T5>, Task>
    {
        public void Call(Action<T1, T2, T3, T4, T5> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5)
        {
            if (RunningMethods.TryGetValue(method, out bool _))
                return;
            if (RunningMethods.TryAdd(method, true))
            {
                method(input1, input2, input3, input4, input5);
                RunningMethods.TryRemove(method, out bool _);
            }
        }

        public Task CallAsync(Action<T1, T2, T3, T4, T5> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5)
        {
            if (RunningTasks.TryGetValue(method, out Task task))
                return task;
            task = new Task(() =>
            {
                method(input1, input2, input3, input4, input5);
                RunningTasks.TryRemove(method, out Task _);
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }
    #endregion

    #region Func

    public interface IPreventSimultaneousFuncCalls<TResult>
    {
        TResult Call(Func<TResult> method);
        Task<TResult> CallAsync(Func<TResult> method1);
    }
    public class PreventSimultaneousFuncCalls<TResult> : PreventSimultaneousCallsBase<Func<TResult>, Task<TResult>>,
                                                         IPreventSimultaneousFuncCalls<TResult>
    {
        public TResult Call(Func<TResult> method)
        {
            return CallAsync(method).Result;
        }

        public Task<TResult> CallAsync(Func<TResult> method)
        {
            if (RunningTasks.TryGetValue(method, out Task<TResult> task))
                return task;

            task = new Task<TResult>(() =>
            {
                var result = method();
                RunningTasks.TryRemove(method, out Task<TResult> _);
                return result;
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }

    public interface IPreventSimultaneousFuncCalls<T1, TResult>
    {
        TResult Call(Func<T1, TResult> method, T1 intput1);
        Task<TResult> CallAsync(Func<T1, TResult> method, T1 intput1);
    }
    public class PreventSimultaneousFuncCalls<T1, TResult> : PreventSimultaneousCallsBase<Func<T1, TResult>, Task<TResult>>,
                                                             IPreventSimultaneousFuncCalls<T1, TResult>
    {
        public TResult Call(Func<T1, TResult> method, T1 input1)
        {
            return CallAsync(method, input1).Result;
        }

        public Task<TResult> CallAsync(Func<T1, TResult> method, T1 input1)
        {
            if (RunningTasks.TryGetValue(method, out Task<TResult> task))
                return task;

            task = new Task<TResult>(() =>
            {
                var result = method(input1);
                RunningTasks.TryRemove(method, out Task<TResult> _);
                return result;
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }

    public interface IPreventSimultaneousFuncCalls<T1, T2, TResult>
    {
        TResult Call(Func<T1, T2,  TResult> method, T1 intput1, T2 input2);
        Task<TResult> CallAsync(Func<T1, T2, TResult> method, T1 intput1, T2 input2);
    }
    public class PreventSimultaneousFuncCalls<T1, T2, TResult> : PreventSimultaneousCallsBase<Func<T1, T2, TResult>, Task<TResult>>,
                                                                 IPreventSimultaneousFuncCalls<T1, T2, TResult>
    {
        public TResult Call(Func<T1, T2, TResult> method, T1 input1, T2 input2)
        {
            return CallAsync(method, input1, input2).Result;
        }

        public Task<TResult> CallAsync(Func<T1, T2, TResult> method, T1 input1, T2 input2)
        {
            if (RunningTasks.TryGetValue(method, out Task<TResult> task))
                return task;

            task = new Task<TResult>(() =>
            {
                var result = method(input1, input2);
                RunningTasks.TryRemove(method, out Task<TResult> _);
                return result;
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }

    public interface IPreventSimultaneousFuncCalls<T1, T2, T3, TResult>
    {
        TResult Call(Func<T1, T2, T3, TResult> method, T1 intput1, T2 input2, T3 input3);
        Task<TResult> CallAsync(Func<T1, T2, T3, TResult> method, T1 intput1, T2 input2, T3 input3);
    }
    public class PreventSimultaneousFuncCalls<T1, T2, T3, TResult> : PreventSimultaneousCallsBase<Func<T1, T2, T3, TResult>, Task<TResult>>,
                                                                     IPreventSimultaneousFuncCalls<T1, T2, T3, TResult>
    {
        public TResult Call(Func<T1, T2, T3, TResult> method, T1 input1, T2 input2, T3 input3)
        {
            return CallAsync(method, input1, input2, input3).Result;
        }

        public Task<TResult> CallAsync(Func<T1, T2, T3, TResult> method, T1 input1, T2 input2, T3 input3)
        {
            if (RunningTasks.TryGetValue(method, out Task<TResult> task))
                return task;

            task = new Task<TResult>(() =>
            {
                var result = method(input1, input2, input3);
                RunningTasks.TryRemove(method, out Task<TResult> _);
                return result;
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }
    public interface IPreventSimultaneousFuncCalls<T1, T2, T3, T4, TResult>
    {
        TResult Call(Func<T1, T2, T3, T4, TResult> method, T1 intput1, T2 input2, T3 input3, T4 input4);
        Task<TResult> CallAsync(Func<T1, T2, T3, T4, TResult> method, T1 intput1, T2 input2, T3 input3, T4 input4);
    }
    public class PreventSimultaneousFuncCalls<T1, T2, T3, T4, TResult> : PreventSimultaneousCallsBase<Func<T1, T2, T3, T4, TResult>, Task<TResult>>,
                                                                         IPreventSimultaneousFuncCalls<T1, T2, T3, T4, TResult>
    {
        public TResult Call(Func<T1, T2, T3, T4, TResult> method, T1 input1, T2 input2, T3 input3, T4 input4)
        {
            return CallAsync(method, input1, input2, input3, input4).Result;
        }

        public Task<TResult> CallAsync(Func<T1, T2, T3, T4, TResult> method, T1 input1, T2 input2, T3 input3, T4 input4)
        {
            if (RunningTasks.TryGetValue(method, out Task<TResult> task))
                return task;

            task = new Task<TResult>(() =>
            {
                var result = method(input1, input2, input3, input4);
                RunningTasks.TryRemove(method, out Task<TResult> _);
                return result;
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }

    public interface IPreventSimultaneousFuncCalls<T1, T2, T3, T4, T5, TResult>
    {
        TResult Call(Func<T1, T2, T3, T4, T5, TResult> method, T1 intput1, T2 input2, T3 input3, T4 input4, T5 input5);
        Task<TResult> CallAsync(Func<T1, T2, T3, T4, T5, TResult> method, T1 intput1, T2 input2, T3 input3, T4 input4, T5 input5);
    }
    public class PreventSimultaneousFuncCalls<T1, T2, T3, T4, T5, TResult> : PreventSimultaneousCallsBase<Func<T1, T2, T3, T4, T5, TResult>, Task<TResult>>,
                                                                             IPreventSimultaneousFuncCalls<T1, T2, T3, T4, T5, TResult>
    {
        public TResult Call(Func<T1, T2, T3, T4, T5, TResult> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5)
        {
            return CallAsync(method, input1, input2, input3, input4, input5).Result;
        }

        public Task<TResult> CallAsync(Func<T1, T2, T3, T4, T5, TResult> method, T1 input1, T2 input2, T3 input3, T4 input4, T5 input5)
        {
            if (RunningTasks.TryGetValue(method, out Task<TResult> task))
                return task;

            task = new Task<TResult>(() =>
            {
                var result = method(input1, input2, input3, input4, input5);
                RunningTasks.TryRemove(method, out Task<TResult> _);
                return result;
            });
            if (RunningTasks.TryAdd(method, task))
            {
                task.Start();
                return task;
            }
            else
            {
                return RunningTasks[method];
            }
        }
    }
    #endregion
}