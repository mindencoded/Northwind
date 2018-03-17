
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class UserDto : SerializableDynamicObject
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [MaxLength(50)]
        [Required(AllowEmptyStrings = false)]
     
        public string Username { get; set; }

        [DataMember]
        [MaxLength(32)]
        [Required(AllowEmptyStrings = false)]
       
        public string Password { get; set; }

        [DataMember]
        [Required]
        public byte? UserTypeId { get; set; }

        [DataMember]
        public int? ExternalUserId { get; set; }
    }
}
