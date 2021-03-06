﻿using System;

namespace Northwind.WebRole.Utils
{
    public class ExceptionManager
    {
        public static string ConcatMessages(Exception exception)
        {
            string message = "";
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