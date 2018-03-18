using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class ProductDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember]
        [MaxLength(25)]
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }

        [DataMember]
        [MaxLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [DataMember] [MaxLength(250)] public string Description { get; set; }

        [DataMember] public decimal StandardCost { get; set; }

        [DataMember] public decimal Price { get; set; }

        [DataMember] public int? ReorderLevel { get; set; }

        [DataMember] public int? TargetLevel { get; set; }

        [DataMember] [MaxLength(50)] public string QuantityPerUnit { get; set; }

        [DataMember] public int? MinimumReorderQuantity { get; set; }

        [DataMember] public bool Discontinued { get; set; }

        [DataMember] public int? CategoryId { get; set; }

        [DataMember] public byte[] Attachments { get; set; }
    }
}