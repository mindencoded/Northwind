using System;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class InvoiceDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int? OrderId { get; set; }

        [DataMember] public DateTime? InvoiceDate { get; set; }

        [DataMember] public DateTime? DueDate { get; set; }

        [DataMember] public decimal Tax { get; set; }

        [DataMember] public decimal Shipping { get; set; }

        [DataMember] public decimal AmountDue { get; set; }
    }
}