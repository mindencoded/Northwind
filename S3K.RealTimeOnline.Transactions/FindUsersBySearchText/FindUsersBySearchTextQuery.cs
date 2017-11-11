using System.ComponentModel.DataAnnotations;
using S3K.RealTimeOnline.GenericDataAccess.Queries;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.Transactions.FindUsersBySearchText
{
    public class FindUsersBySearchTextQuery : IQuery<User[]>
    {
        [Required]
        [StringLength(25)]
        public string SearchText { get; set; }

        public bool IncludeInactiveUsers { get; set; }
    }
}