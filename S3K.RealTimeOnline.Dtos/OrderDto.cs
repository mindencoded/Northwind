using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.Dtos
{
    [DataContract]
    public class OrderDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember] [Range(1, int.MaxValue)] public int EmployeeId { get; set; }

        [DataMember] [Range(1, int.MaxValue)] public int CustomerId { get; set; }

        [DataMember] [Range(1, int.MaxValue)] public int ShipperId { get; set; }

        [DataMember] public DateTime OrderDate { get; set; }

        [DataMember] public DateTime ShippedDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string ShipName { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        public string ShipAddress { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string ShipCity { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string ShipStateProvince { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string ShipZipPostalCode { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string ShipCountryRegion { get; set; }

        [DataMember] public decimal ShippingFee { get; set; }

        [DataMember] public decimal Tax { get; set; }

        [DataMember] public decimal TaxRate { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; }

        [DataMember] public DateTime PaidDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(250)]
        public string Notes { get; set; }

        [DataMember] [Required] public byte? TaxStatusId { get; set; }

        [DataMember] [Required] public byte? StatusId { get; set; }
    }
}