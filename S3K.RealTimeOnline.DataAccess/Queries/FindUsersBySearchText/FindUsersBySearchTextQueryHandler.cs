using System.Collections.Generic;
using System.Linq;
using S3K.RealTimeOnline.DataAccess.Repositories;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.SecurityDomain;

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
                //SqlDataAdapter adapter = _unitOfWork.Repository<User>().SqlDataAdapter();
                //dynamic parameters = new
                //{
                //    Username = query.SearchText,
                //    Active = !query.IncludeInactiveUsers
                //};
                IDictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "Username",  query.SearchText},
                    { "Active",  !query.IncludeInactiveUsers}
                };
                IList<string> columns = new List<string>
                {
                    "Username", "Active"
                };
                IRepository<User> userRepository = _unitOfWork.Repository<User>();
                IEnumerable<dynamic> users = userRepository.Select(columns, parameters);
                dynamic[] array = users.ToArray();
                return null;
            }
        }
    }
}