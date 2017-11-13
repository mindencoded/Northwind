using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("INVENTORY_TRANSACTION_TYPE")]
    public class InventoryTransactionType
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("TYPE_NAME")]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string TypeName { get; set; }
    }
}