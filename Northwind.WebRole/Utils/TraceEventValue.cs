
namespace Northwind.WebRole.Utils
{
    public enum TraceEventValue
    {
        Critical = 1, //Fatal error or application crash
        Error = 2, //Recoverable error
        Warning = 4, //Non-critical problem—may be an indication of more serious problems to come
        Information = 8, //Informational message
        Verbose = 10, //Debugging trace (such as detailed execution flow information, parameters, and so forth)
        Start = 100, //Starting of a logical operation
        Stop = 200, //Stopping of a logical operation
        Suspend = 400, //Suspension of a logical operation
        Resume = 800, //Resuming a logical operation
        Transfer = 1000 //Transfer to a new activity
    }
}