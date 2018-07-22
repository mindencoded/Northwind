using System.Runtime.Serialization;

namespace Northwind.Shared
{
    [DataContract]
    public class RoleDetailDto : SerializableDynamicObject
    {
        [DataMember] public int UserId { get; set; }

        [DataMember] public int RoleId { get; set; }

        [DataMember] public bool Active { get; set; }
    }
}