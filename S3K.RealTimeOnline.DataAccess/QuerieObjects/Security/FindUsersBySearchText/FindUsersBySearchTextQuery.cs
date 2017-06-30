using System.ComponentModel.DataAnnotations;
using S3K.RealTimeOnline.Domain.Entities.Security;

namespace S3K.RealTimeOnline.DataAccess.QuerieObjects.Security.FindUsersBySearchText
{
    public class FindUsersBySearchTextQuery : IQuery<User[]>
    {
        [Required]
        [StringLength(1)]
        public string SearchText { get; set; }
        public bool IncludeInactiveUsers { get; set; }
    }
}
