using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.QueryHandlers
{
    public class FindUsersBySearchTextQuery : IQuery<User[]>
    {
        [Required] [StringLength(25)] public string SearchText { get; set; }

        public bool IncludeInactiveUsers { get; set; }
    }
}