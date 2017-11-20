using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("CUSTOMER")]
    public class Customer : Person
    {
        public virtual ICollection<Order> Orders { get; set; }
    }
}