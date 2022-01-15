using Rhyous.WebFramework.Services.Models;
using System;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services.Providers
{
    public interface IExecResponseProvider
    {
        Task<ExecResponse> AsyncTryCatchFinallyWrap(Func<Task> code, Action finallyCode, int errorCode, string message);
        Task<ExecResponse<T>> AsyncTryCatchFinallyWrap<T>(Func<Task<T>> code, Action finallyCode, int errorCode, string message);
        Task<ExecResponse> AsyncTryCatchWrap(Func<Task> code, int errorCode, string message);
        ExecResponse TryCatchFinallyWrap(Action code, Action finallyCode, int errorCode, string message);
        ExecResponse<T> TryCatchFinallyWrap<T>(Func<T> code, Action finallyCode, int errorCode, string message);
        ExecResponse TryCatchWrap(Action code, int errorCode, string message);
        ExecResponse<TResult> TryCatchWrap<TResult>(Func<TResult> code, int errorCode, string message);
        ExecResponse<TResult> TryCatchWrap<T1, TResult>(Func<T1, TResult> code, T1 input1, int errorCode, string message);
        ExecResponse<TResult> TryCatchWrap<T1, T2, TResult>(Func<T1, T2, TResult> code, T1 input1, T2 input2, int errorCode, string message);
        Task<ExecResponse<T>> TryCatchWrapAsync<T>(Func<Task<T>> code, int errorCode, string message);
        Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> code, T1 input1, T2 input2, T3 input3, int errorCode, string message);
        Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> code, T1 input1, T2 input2, T3 input3, int errorCode, string message, string messageTemplate);
        Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, T2, TResult>(Func<T1, T2, Task<TResult>> code, T1 input1, T2 input2, int errorCode, string message);
        Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, TResult>(Func<T1, Task<TResult>> code, T1 input1, int errorCode, string message);
    }
}