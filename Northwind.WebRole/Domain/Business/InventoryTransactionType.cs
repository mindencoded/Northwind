using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("INVENTORY_TRANSACTION_TYPE")]
    public class InventoryTransactionType : Entity
    {
        [Key] [Column("ID")] public byte Id { get; set; }

        [Column("TYPE_NAME")]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string TypeName { get; set; }

        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }
    }
}