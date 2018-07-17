using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Repositories;
using Northwind.WebRole.Tools;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.QueryHandlers
{
    public class FindUserByUsernamePasswordQueryHandler : IQueryHandler<FindUserByUsernamePasswordQuery, User>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public FindUserByUsernamePasswordQueryHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User Handle(FindUserByUsernamePasswordQuery query)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Username", query.Username},
                {"Password", query.Password},
                {"Active", query.Active}
            };

            //IList<string> columns = new List<string>
            //{
            //    "Id",
            //    "Username",
            //    "Active"
            //};

            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IRepository<User> userRepository = _unitOfWork.Repository<User>();
                User user = userRepository.Select(parameters).FirstOrDefault();
                if (user != null)
                {
                    parameters = new Dictionary<string, object>
                    {
                        {"UserId", user.Id},
                        {"Active", true}
                    };
                    IRepository<RoleDetail> roleDetailRepository = _unitOfWork.Repository<RoleDetail>();
                    IList<RoleDetail> roleDetails = roleDetailRepository.Select(parameters).ToList();
                    if (roleDetails.Any())
                    {
                        object[] roleDetailIds = roleDetails.Select(rd => rd.RoleId).ToArray().Cast<object>().ToArray();
                        IRepository<Role> roleRepository = _unitOfWork.Repository<Role>();
                        parameters = new Dictionary<string, object>
                        {
                            {"Id", roleDetailIds}
                        };

                        IEnumerable<Role> roles = roleRepository.Select(parameters);

                        foreach (var role in roles)
                        {
                            RoleDetail roleDetail = roleDetails.FirstOrDefault(rd => rd.RoleId == role.Id);
                            if (roleDetail != null) roleDetail.Role = role;
                        }

                        user.RoleDetails = roleDetails;
                    }
                }

                return user;
            }
        }

        public Task<User> HandleAsync(FindUserByUsernamePasswordQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}