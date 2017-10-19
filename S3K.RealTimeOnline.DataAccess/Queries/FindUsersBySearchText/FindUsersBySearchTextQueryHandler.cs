using System.Collections.Generic;
using System.Linq;
using S3K.RealTimeOnline.DataAccess.Repositories;
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

                //IEnumerable<User> users = _unitOfWork.ExecuteQuery<IEnumerable<User>>(Resources.FindUsersBySearchText, new Dictionary<string, object>
                //    {
                //        {"@SearchText", query.SearchText},
                //        {"@IncludeInactiveUsers", query.IncludeInactiveUsers}
                //    });

                

                IRepository<User> userRepository = _unitOfWork.Repository<User>();


                //User user = userRepository.SelectById(2);

                /*User newUser = new User
                {
                     Username = "MARIA",
                     Password = "123",
                     FullName = "MARIA KLER",
                     Active = true,
                     LastModified = DateTime.Now,
                     Created = DateTime.Now
                };

                int result= userRepository.Insert(newUser);

                _unitOfWork.Commit();*/

                /*dynamic userUpdated = new
                {
                    Id = 1,
                    Username = "LOUIS",
                    FullName = "LOUIS MC CONNINHAM"
                };
                
                int result= userRepository.Update(userUpdated);
                _unitOfWork.Commit();*/

                /*dynamic parameters = new
                {                
                    FullName = "LOUIS Mc LAUREN"
                };

                dynamic conditions = new
                {
                    Username = "LOUIS"
                };

                userRepository.Update(parameters, conditions);
                _unitOfWork.Commit();*/
                /*dynamic conditions = new
                {
                    Username = "KARL"
                };
                userRepository.Delete(conditions);
                _unitOfWork.Commit();*/

                dynamic parameters = new
                {
                    Username = query.SearchText,
                    Active = !query.IncludeInactiveUsers
                };
                IEnumerable<User> users = userRepository.Select(parameters);
                return users.ToArray();
            }
        }
    }
}