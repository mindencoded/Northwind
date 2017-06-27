using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace S3K.RealTimeOnline.Domain.Entities.Business
{
    [Table("PRODUCT")]
    public class Product : Entity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
