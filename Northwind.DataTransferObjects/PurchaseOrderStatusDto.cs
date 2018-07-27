using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.DataTransferObjects
{
    [DataContract]
    public class PurchaseOrderStatusDto : SerializableDynamicObject
    {
        [DataMember] public byte Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string StatusName { get; set; }
    }
}