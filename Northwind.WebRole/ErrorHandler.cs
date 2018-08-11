using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Northwind.WebRole
{
    public class ErrorHandler : IErrorHandler
    {
        private static readonly TraceSource Trace = new TraceSource(Assembly.GetExecutingAssembly().GetName().Name);
        /// <summary>
        /// The method that's get invoked if any unhandled exception raised in service
        /// Here you can do what ever logic you would like to. 
        /// For example logging the exception details
        /// Here the return value indicates that the exception was handled or not
        /// Return true to stop exception propagation and system considers 
        /// that the exception was handled properly
        /// else return false to abort the session
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            return true;
        }

        /// <summary>
        /// If you want to communicate the exception details to the service client 
        /// as proper fault message
        /// here is the place to do it
        /// If we want to suppress the communication about the exception, 
        /// set fault to null
        /// </summary>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            StringBuilder sb = new StringBuilder(error.Message);
            Exception innerException = error.InnerException;
            while (innerException != null)
            {
                sb.Append(" | ").Append(innerException.Message);
                innerException = innerException.InnerException;
            }
            Trace.TraceEvent(TraceEventType.Error, 9000, sb.ToString());
            //if (error is FaultException)
            //{               
            //    FaultException faultException = error as FaultException;
            //    FaultCode faultCode = faultException.Code;
            //    MessageVersion messageVersion = OperationContext.Current.IncomingMessageVersion;
            //    fault = Message.CreateMessage(messageVersion, faultCode, string.Format("Method: {0} | Message: {1}", error.TargetSite.Name, error.Message), faultException.Action);
            //}  
        }
    }
}