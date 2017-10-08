using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace S3K.RealTimeOnline.Domain
{
    [Table("USER")]
    public class User : Entity
    {
        public User()
        {
            Active = true;
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Column("USERNAME")]
        public string Username { get; set; }

        [MaxLength(32)]
        [Required]
        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("CREATED")]
        public DateTime? Created { get; set; }

        [Column("LAST_MODIFIED")]
        public DateTime? LastModified { get; set; }

        [Column("ACTIVE")]
        public bool Active { get; set; }
    }
}