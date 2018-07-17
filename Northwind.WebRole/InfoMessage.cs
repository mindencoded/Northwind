using System.Runtime.Serialization;

namespace Northwind.WebRole
{
    [DataContract]
    public class InfoMessage
    {
        public InfoMessage(string message)
        {
            Message = message;
        }

        [DataMember] public string Message { get; set; }
    }
}