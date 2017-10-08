using System.Linq;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.Domain;

namespace S3K.RealTimeOnline.DataAccess.Queries.FindUsersBySearchText
{
    public class FindUsersBySearchTextQueryHandler : IQueryHandler<FindUsersBySearchTextQuery, User[]>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public FindUsersBySearchTextQueryHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User[] Handle(FindUsersBySearchTextQuery query)
        {
            using (_unitOfWork)
            {
                var user = new User { Username = query.SearchText, Active = !query.IncludeInactiveUsers };
                return _unitOfWork.UserRepository.Select(user).ToArray();
            }
        }
    }
}