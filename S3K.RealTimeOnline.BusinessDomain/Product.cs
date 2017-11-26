using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("PRODUCT")]
    public class Product : Entity
    {
        public Product()
        {
            Discontinued = false;
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [MaxLength(25)]
        [Required]
        [Column("CODE")]
        public string Code { get; set; }

        [MaxLength(50)]
        [Required]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(250)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("STANDARD_COST")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal StandardCost { get; set; }

        [Column("PRICE")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Price { get; set; }

        [Column("REORDER_LEVEL")]
        public int? ReorderLevel { get; set; }

        [Column("TARGET_LEVEL")]
        public int? TargetLevel { get; set; }

        [Column("QUANTITY_PER_UNIT")]
        public int? QuantityPerUnit { get; set; }

        [Column("MINIMUM_REORDER_QUANTITY")]
        public int? MinimumReorderQuantity { get; set; }

        [Column("DISCONTINUED")]
        public bool Discontinued { get; set; }

        [Column("SUB_CATEGORY_ID")]
        public int? SubCategoryId { get; set; }

        [Column("ATTACHMENTS")]
        public byte[] Attachments { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }


        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}