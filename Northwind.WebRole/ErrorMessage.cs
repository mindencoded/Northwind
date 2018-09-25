using System;
using System.Runtime.Serialization;

namespace Northwind.WebRole
{
    [DataContract]
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;
        }

        public ErrorMessage(Exception exception)
        {
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Exception = exception.GetType().Name;
        }

        public ErrorMessage(string message, Exception exception)
        {
            Message = message;
            StackTrace = exception.StackTrace;
            Exception = exception.GetType().Name;
        }


        [DataMember] public string StackTrace { get; }

        [DataMember] public string Message { get; }

        [DataMember] public string Exception { get; }
    }
}