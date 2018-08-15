using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.WebRole.Dtos
{
    [DataContract]
    public class RoleGroupDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}