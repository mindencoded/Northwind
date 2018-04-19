using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("ORDER")]
    public class Order : Entity
    {
        [Key] [Column("ID")] public int Id { get; set; }

        [ForeignKey("Employee")]
        [Column("EMPLOYEE_ID")]
        public int EmployeeId { get; set; }

        [ForeignKey("Customer")]
        [Column("CUSTOMER_ID")]
        public int CustomerId { get; set; }

        [ForeignKey("Shipper")]
        [Column("SHIPPER_ID")]
        public int ShipperId { get; set; }

        [Column("ORDER_DATE")] public DateTime OrderDate { get; set; }

        [Column("SHIPPED_DATE")] public DateTime ShippedDate { get; set; }

        [Column("SHIP_NAME")]
        [StringLength(50)]
        public string ShipName { get; set; }

        [Column("SHIP_ADDRESS")]
        [StringLength(250)]
        public string ShipAddress { get; set; }

        [Column("SHIP_CITY")]
        [StringLength(50)]
        public string ShipCity { get; set; }

        [Column("SHIP_STATE_PROVINCE")]
        [StringLength(50)]
        public string ShipStateProvince { get; set; }

        [Column("SHIP_ZIP_POSTAL_CODE")]
        [StringLength(50)]
        public string ShipZipPostalCode { get; set; }

        [Column("SHIP_COUNTRY_REGION")]
        [StringLength(50)]
        public string ShipCountryRegion { get; set; }

        [Column("SHIPPING_FEE")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal ShippingFee { get; set; }

        [Column("TAX")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Tax { get; set; }

        [Column("TAX_RATE")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal TaxRate { get; set; }

        [Column("PAYMENT_TYPE")]
        [StringLength(50)]
        public string PaymentType { get; set; }

        [Column("PAID_DATE")] public DateTime PaidDate { get; set; }

        [Column("NOTES")] [StringLength(250)] public string Notes { get; set; }

        [ForeignKey("OrderTaxStatus")]
        [Column("TAX_STATUS_ID")]
        public byte? TaxStatusId { get; set; }

        [ForeignKey("OrderStatus")]
        [Column("STATUS_ID")]
        public byte? StatusId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Shipper Shipper { get; set; }

        public virtual OrderTaxStatus OrderTaxStatus { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    }
}

//[RegularExpression(@"^\d+.\d{0,4}$")]
//[RegularExpression(@"\d+(\.\d{0,4})?")]
//[RegularExpression(@"^(0|-?\d{0,19}(\.\d{0,4})?)$")]
//[RegularExpression("^[-+]?(?:[0-9]|[0-1][0-2](?:[.][0-9])?)$")]
//[RegularExpression(@"^\d*(\.|,|(\.\d{1,4})|(,\d{1,4}))?$")]
//[RegularExpression(@"[0-9]{0,}\.[0-9]{4}")]  