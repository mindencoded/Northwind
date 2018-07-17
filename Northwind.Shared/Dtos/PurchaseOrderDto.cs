using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.Shared.Dtos
{
    [DataContract]
    public class PurchaseOrderDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int? SupplierId { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime? CreationDate { get; set; }

        [DataMember] public int? SubmittedBy { get; set; }

        [DataMember] public DateTime? SubmittedDate { get; set; }

        [DataMember] public int? ApprovedBy { get; set; }

        [DataMember] public DateTime? ApprovedDate { get; set; }

        [DataMember] public byte? StatusId { get; set; }

        [DataMember] public DateTime? ExpectedDate { get; set; }

        [DataMember] public decimal ShippingFee { get; set; }

        [DataMember] public decimal Tax { get; set; }

        [DataMember] public decimal PaymentAmount { get; set; }

        [DataMember] public DateTime? PaymentDate { get; set; }

        [DataMember] [StringLength(50)] public string PaymentMethod { get; set; }

        [DataMember] [StringLength(250)] public string Notes { get; set; }
    }
}