using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class RoleDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}