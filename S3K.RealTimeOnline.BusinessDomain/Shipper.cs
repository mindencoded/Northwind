using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("SHIPPER")]
    public class Shipper : Person
    {
        public virtual ICollection<Order> Orders { get; set; }
    }
}