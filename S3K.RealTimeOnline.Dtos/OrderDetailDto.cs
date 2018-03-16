using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class OrderDetailDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int OrderId { get; set; }

        [DataMember] [Required] public int? ProductId { get; set; }

        [DataMember] public decimal Quantity { get; set; }

        [DataMember] public decimal UnitPrice { get; set; }

        [DataMember] public decimal Discount { get; set; }

        [DataMember] [Required] public DateTime? DateAllocated { get; set; }

        [DataMember] [Required] public byte? StatusId { get; set; }

        [DataMember] [Required] public int? PurchaseOrderId { get; set; }

        [DataMember] [Required] public int? InventoryId { get; set; }
    }
}