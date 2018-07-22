using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.Shared
{
    [DataContract]
    public class LoginDto : SerializableDynamicObject
    {
        [DataMember]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Username { get; set; }

        [DataMember]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(32)]
        public string Password { get; set; }

        [DataMember] public bool RememberMe { get; set; }
    }
}