using System.ServiceModel;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDomain;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class RoleDetailCrudService : CrudService<ISecurityUnitOfWork, RoleDetail, RoleDetailDto>,
        IRoleDetailCrudService
    {
        public RoleDetailCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IRoleDetailCrudService>(config, "");
        }
    }
}