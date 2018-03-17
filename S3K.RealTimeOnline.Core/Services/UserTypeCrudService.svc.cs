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
    }
}
