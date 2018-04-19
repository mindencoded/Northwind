using System;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class PurchaseOrderDetailDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int? PurchaseOrderId { get; set; }

        [DataMember] public int? ProductId { get; set; }

        [DataMember] public int? InventoryId { get; set; }

        [DataMember] public decimal Quantity { get; set; }

        [DataMember] public decimal UnitCost { get; set; }

        [DataMember] public bool PostedToInventory { get; set; }
    }
}