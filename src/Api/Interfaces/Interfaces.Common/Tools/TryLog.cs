using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class TryLog
    {
        private readonly ILogger _Logger;

        public TryLog(ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Action (which is void)
        public void Try(Action method)
        {
            if (method == null)
                return;
            try { method?.Invoke(); }
            catch (Exception e) { _Logger.Write(e); }
        }

        public void Try<TInput>(Action<TInput> method, TInput t)
        {
            if (method == null)
                return;
            try { method?.Invoke(t); }
            catch (Exception e) { _Logger.Write(e); }
        }

        public void Try<TInput1, TInput2>(Action<TInput1, TInput2> method, TInput1 t1, TInput2 t2)
        {
            if (method == null)
                return;
            try { method?.Invoke(t1, t2); }
            catch (Exception e) { _Logger.Write(e); }
        }

        public void Try<TInput1, TInput2, TInput3>(Action<TInput1, TInput2, TInput3> method, TInput1 t1, TInput2 t2, TInput3 t3)
        {
            if (method == null)
                return;
            try { method?.Invoke(t1, t2, t3); }
            catch (Exception e) { _Logger.Write(e); }
        }

        public void Try<TInput1, TInput2, TInput3, TInput4>(Action<TInput1, TInput2, TInput3, TInput4> method, TInput1 t1, TInput2 t2, TInput3 t3, TInput4 t4)
        {
            if (method == null)
                return;
            try { method?.Invoke(t1, t2, t3, t4); }
            catch (Exception e) { _Logger.Write(e); }
        }

        public void Try<TInput1, TInput2, TInput3, TInput4, TInput5>(Action<TInput1, TInput2, TInput3, TInput4, TInput5> method, TInput1 t1, TInput2 t2, TInput3 t3, TInput4 t4, TInput5 t5)
        {
            if (method == null)
                return;
            try { method?.Invoke(t1, t2, t3, t4, t5); }
            catch (Exception e) { _Logger.Write(e); }
        }
        #endregion

        #region Func (which return a value)
        public TResult Try<TResult>(Func<TResult> method)
        {
            if (method != null)
            {
                try { return method.Invoke(); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public TResult Try<TInput, TResult>(Func<TInput, TResult> method, TInput t)
        {
            if (method != null)
            {
                try { return method.Invoke(t); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public TResult Try<TInput1, TInput2, TResult>(Func<TInput1, TInput2, TResult> method, TInput1 t1, TInput2 t2)
        {
            if (method != null)
            {
                try { return method.Invoke(t1, t2); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public TResult Try<TInput1, TInput2, TInput3, TResult>(Func<TInput1, TInput2, TInput3, TResult> method, TInput1 t1, TInput2 t2, TInput3 t3)
        {
            if (method != null)
            {
                try { return method.Invoke(t1, t2, t3); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public TResult Try<TInput1, TInput2, TInput3, TInput4, TResult>(Func<TInput1, TInput2, TInput3, TInput4, TResult> method, TInput1 t1, TInput2 t2, TInput3 t3, TInput4 t4)
        {
            if (method != null)
            {
                try { return method.Invoke(t1, t2, t3, t4); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }
        public TResult Try<TInput1, TInput2, TInput3, TInput4, TInput5, TResult>(Func<TInput1, TInput2, TInput3, TInput4, TInput5, TResult> method, TInput1 t1, TInput2 t2, TInput3 t3, TInput4 t4, TInput5 t5)
        {
            if (method != null)
            {
                try { return method.Invoke(t1, t2, t3, t4, t5); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }
        #endregion
        
        #region Func async 
        public async Task<TResult> TryAsync<TResult>(Func<Task<TResult>> method)
        {
            if (method != null)
            {
                try { return await method?.Invoke(); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public async Task<TResult> TryAsync<TInput, TResult>(Func<TInput, Task<TResult>> method, TInput t)
        {
            if (method != null)
            {
                try { return await method?.Invoke(t); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public async Task<TResult> TryAsync<TInput1, TInput2, TResult>(Func<TInput1, TInput2, Task<TResult>> method, TInput1 t1, TInput2 t2)
        {
            if (method != null)
            {
                try { return await method?.Invoke(t1, t2); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public async Task<TResult> TryAsync<TInput1, TInput2, TInput3, TResult>(Func<TInput1, TInput2, TInput3, Task<TResult>> method, TInput1 t1, TInput2 t2, TInput3 t3)
        {
            if (method != null)
            {
                try { return await method?.Invoke(t1, t2, t3); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }

        public async Task<TResult> TryAsync<TInput1, TInput2, TInput3, TInput4, TResult>(Func<TInput1, TInput2, TInput3, TInput4, Task<TResult>> method, TInput1 t1, TInput2 t2, TInput3 t3, TInput4 t4)
        {
            if (method != null)
            {
                try { return await method?.Invoke(t1, t2, t3, t4); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }
        public async Task<TResult> TryAsync<TInput1, TInput2, TInput3, TInput4, TInput5, TResult>(Func<TInput1, TInput2, TInput3, TInput4, TInput5, Task<TResult>> method, TInput1 t1, TInput2 t2, TInput3 t3, TInput4 t4, TInput5 t5)
        {
            if (method != null)
            {
                try { return await method?.Invoke(t1, t2, t3, t4, t5); }
                catch (Exception e) { _Logger.Write(e); }
            }
            return default(TResult);
        }
        #endregion
    }
}
