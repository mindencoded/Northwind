using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S3K.RealTimeOnline.CommonUtils
{
    public class CustomValidationException : ValidationException
    {
        public CustomValidationException()
        {
        }

        public CustomValidationException(string errorMessage) : base(errorMessage)
        {
        }

        public IList<string> Errors { get; set; }
    }
}