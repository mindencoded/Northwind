using System.Linq;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.Domain.Entities.Security;

namespace S3K.RealTimeOnline.DataAccess.QuerieObjects.FindUsersBySearchText
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
                var userRepository = _db.UserRepository;
                var users = userRepository.SelectAll(new User { Username = query.SearchText }).ToArray();
                return users;
            } 
        }
    }
}
