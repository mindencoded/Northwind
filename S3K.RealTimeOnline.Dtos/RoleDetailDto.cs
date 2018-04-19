using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class RoleDetailDto : SerializableDynamicObject
    {
        [DataMember] public int UserId { get; set; }

        [DataMember] public int RoleId { get; set; }

        [DataMember] public bool Active { get; set; }
    }
}