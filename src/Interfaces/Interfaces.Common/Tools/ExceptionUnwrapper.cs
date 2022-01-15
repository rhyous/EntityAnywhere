using System;
using System.Runtime.ExceptionServices;

namespace Rhyous.EntityAnywhere.Interfaces.Tools
{
    public class ExceptionUnwrapper
    {
        public static TResult HandleException<TException, TResult>(Exception e, bool ignoreException = false)
            where TException : Exception
        {
            if (ignoreException)
                return default(TResult);

            if (e is TException && e.InnerException != null)
            {
                var inner = e;
                while (inner is TException)
                    inner = inner.InnerException;
                ExceptionDispatchInfo.Capture(inner).Throw();
            }
            throw e;
        }

        public static void HandleException<TException>(Exception e, bool ignoreException = false)
            where TException : Exception
        {
            if (ignoreException)
                return;

            if (e is TException && e.InnerException != null)
            {
                var inner = e;
                while (inner is TException)
                    inner = inner.InnerException;
                ExceptionDispatchInfo.Capture(inner).Throw();
            }
            throw e;
        }
    }
}
