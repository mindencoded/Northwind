using System;

namespace S3K.RealTimeOnline.Commons
{
    public class ExceptionManager
    {
        public static string ConcatMessages(Exception exception)
        {
            var message = "";
            while (true)
            {
                message += exception.Message + Environment.NewLine;
                if (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    continue;
                }
                break;
            }
            return message;
        }
    }
}