using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDomain;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class UserTypeCrudService : CrudService<ISecurityUnitOfWork, UserType, UserTypeDto>, IUserTypeCrudService
    {
        public UserTypeCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IUserTypeCrudService>(config, "");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Select")]
        public override Stream SelectA(string page, string pageSize)
        {
            return base.SelectA(page, pageSize);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby)
        {
            return base.SelectB(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectC(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Select")]
        public override Stream SelectD(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.SelectD(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Insert")]
        public override void Insert(UserTypeDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Update")]
        public override void Update(string id, UserTypeDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "UserTypeCrud.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }
    }
}