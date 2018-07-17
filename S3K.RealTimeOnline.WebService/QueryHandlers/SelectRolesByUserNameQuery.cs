using System.Collections.Generic;
using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.QueryHandlers
{
    public class SelectRolesByUserNameQuery : IQuery<IEnumerable<Role>>
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Username { get; set; }
    }
}