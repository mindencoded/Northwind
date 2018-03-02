using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class InventoryTransactionTypeDto : SerializableDynamicObject
    {
        [DataMember]
        public byte Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required]
        public string TypeName { get; set; }
    }
}
