using System.IO;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;

namespace S3K.RealTimeOnline.Core
{
    public abstract class BaseService
    {
        public string ResponseDataToString(dynamic data)
        {
            var s = JsonSerializer.Create();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                s.Serialize(sw, data);
            }
            return sb.ToString();
        } 

        public Stream JsonStream(string response)
        {
            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.ContentType =
                    "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(response));
        }
    }
}
