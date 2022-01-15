using System;
using System.Runtime.CompilerServices;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ILogger
    {
        void Debug(string msg, [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0);
        void Write(string msg, [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0);
        void Write(Exception msg, [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFilePath = "",
            [CallerLineNumber] int callingFileLineNumber = 0);
    }
}
