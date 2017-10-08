using System.ComponentModel.DataAnnotations;
using S3K.RealTimeOnline.Domain;

namespace S3K.RealTimeOnline.DataAccess.Queries.FindUsersBySearchText
{
    public class FindUsersBySearchTextQuery : IQuery<User[]>
    {
        [Required]
        [StringLength(25)]
        public string SearchText { get; set; }

        public bool IncludeInactiveUsers { get; set; }
    }
}