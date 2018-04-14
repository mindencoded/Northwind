using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.QueryHandlers.SelectRolesByUserName
{
    public class SelectRolesByUserNameQuery : IQuery<IEnumerable<Role>>
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Username { get; set; }
    }
}