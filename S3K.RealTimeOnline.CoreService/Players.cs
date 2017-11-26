using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.WebRole
{
    [DataContract]
    public class Players
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Sports { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }
    }
}