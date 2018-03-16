using System;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class InventoryTransactionDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public byte? TypeId { get; set; }

        [DataMember]
        public DateTime? TransactionCreatedDate { get; set; }

        [DataMember]
        public DateTime? TransactionModifiedDate { get; set; }

        [DataMember] public int? ProductId { get; set; }

        [DataMember] public int Quantity { get; set; }

        [DataMember] public int? PurchaseOrderId { get; set; }

        [DataMember] public int? CustomerOrderId { get; set; }

        [DataMember] public string Comments { get; set; }
    }
}