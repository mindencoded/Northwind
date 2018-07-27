using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.DataTransferObjects
{
    [DataContract]
    public class InventoryTransactionTypeDto : SerializableDynamicObject
    {
        [DataMember] public byte Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required]
        public string TypeName { get; set; }
    }
}