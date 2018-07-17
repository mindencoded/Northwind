using System;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.WebService
{
    [DataContract]
    public class ErrorMessage
    {
        public ErrorMessage()
        {
            Message = "An error occurred.";
        }

        public ErrorMessage(Exception error)
        {
            Message = error.Message;
            StackTrace = error.StackTrace;
            Exception = error.GetType().Name;
        }

        [DataMember] public string StackTrace { get; set; }

        [DataMember] public string Message { get; set; }

        [DataMember] public string Exception { get; set; }
    }
}