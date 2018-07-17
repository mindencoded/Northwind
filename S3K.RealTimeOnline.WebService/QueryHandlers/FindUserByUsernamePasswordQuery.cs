using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.QueryHandlers
{
    public class FindUserByUsernamePasswordQuery : IQuery<User>
    {
        public FindUserByUsernamePasswordQuery()
        {
            Active = true;
        }

        [Required] [StringLength(50)] public string Username { get; set; }

        [Required] [StringLength(32)] public string Password { get; set; }

        public bool Active { get; set; }
    }
}