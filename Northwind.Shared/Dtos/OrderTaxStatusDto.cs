using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.Shared.Dtos
{
    [DataContract]
    public class OrderTaxStatusDto : SerializableDynamicObject
    {
        [DataMember] public byte Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string StatusName { get; set; }
    }
}