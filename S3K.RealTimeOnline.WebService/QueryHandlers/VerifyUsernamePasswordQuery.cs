using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.QueryHandlers
{
    public class VerifyUsernamePasswordQuery : IQuery<bool>
    {
        [Required] [StringLength(50)] public string Username { get; set; }

        [Required] [StringLength(32)] public string Password { get; set; }

        public bool Active
        {
            get { return true; }
        }
    }
}