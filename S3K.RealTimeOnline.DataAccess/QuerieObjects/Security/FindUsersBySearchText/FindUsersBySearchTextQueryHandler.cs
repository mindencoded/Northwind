using System.Linq;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks.Security;
using S3K.RealTimeOnline.Domain.Entities.Security;

namespace S3K.RealTimeOnline.DataAccess.QuerieObjects.Security.FindUsersBySearchText
{
    public class FindUsersBySearchTextQueryHandler : IQueryHandler<FindUsersBySearchTextQuery, User[]>
    {
        private readonly ISecurityUnitOfWork _db;

        public FindUsersBySearchTextQueryHandler(ISecurityUnitOfWork db)
        {
            _db = db;
        }

        public User[] Handle(FindUsersBySearchTextQuery query)
        {
            using (_db)
            {
                var user = new User { Username = query.SearchText, Active = !query.IncludeInactiveUsers };
                var users = _db.UserRepository.Select(user).ToArray();
                return users;
            }
        }
    }
}