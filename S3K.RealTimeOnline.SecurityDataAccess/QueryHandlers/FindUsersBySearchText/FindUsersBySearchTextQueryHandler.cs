using System.Collections.Generic;
using System.Linq;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.QueryHandlers.FindUsersBySearchText
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
            //SqlDataAdapter adapter = _unitOfWork.Repository<User>().SqlDataAdapter();
            //dynamic parameters = new
            //{
            //    Username = query.SearchText,
            //    Active = !query.IncludeInactiveUsers
            //};
            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Username", query.SearchText},
                {"Active", !query.IncludeInactiveUsers}
            };
            IList<string> columns = new List<string>
            {
                "Username",
                "Active"
            };


            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IRepository<User> userRepository = _unitOfWork.Repository<User>();
                IEnumerable<dynamic> users = userRepository.Select(columns, parameters);
                dynamic[] array = users.ToArray();
                return null;
            }
        }
    }
}