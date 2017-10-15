using System.Collections.Generic;
using System.Linq;
using S3K.RealTimeOnline.DataAccess.Properties;
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
                //IEnumerable<User> users = _unitOfWork.ExecuteFunction<IEnumerable<User>>(Resources.SpFindUsersBySearchText, query.SearchText, query.IncludeInactiveUsers);
 
                //IEnumerable<User> users = _unitOfWork.ExecuteFunction<IEnumerable<User>>(
                //    Resources.SpFindUsersBySearchText, new Dictionary<string, object>
                //    {
                //        {"@SearchText", query.SearchText},
                //        {"@IncludeInactiveUsers", query.IncludeInactiveUsers}
                //    });

                //IEnumerable<User> users = _unitOfWork.ExecuteQuery<IEnumerable<User>>(Resources.FindUsersBySearchText, query.SearchText, query.IncludeInactiveUsers);

                IEnumerable<User> users = _unitOfWork.ExecuteQuery<IEnumerable<User>>(Resources.FindUsersBySearchText, new Dictionary<string, object>
                    {
                        {"@SearchText", query.SearchText},
                        {"@IncludeInactiveUsers", query.IncludeInactiveUsers}
                    });
                return users.ToArray();
            }
        }
    }
}