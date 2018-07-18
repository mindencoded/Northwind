using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Repositories;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class SelectRolesByUserNameQueryHandler : IQueryHandler<SelectRolesByUserNameQuery, IEnumerable<Role>>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public SelectRolesByUserNameQueryHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Role> Handle(SelectRolesByUserNameQuery query)
        {
            IList<Role> roles = new List<Role>();
            IDictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Username", query.Username},
                {"Active", true}
            };

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
                        IRepository<Role> roleRepository = _unitOfWork.Repository<Role>();
                        foreach (RoleDetail roleDetail in roleDetails)
                        {
                            parameters = new Dictionary<string, object>
                            {
                                {"Id", roleDetail.RoleId}
                            };
                            Role role = roleRepository.Select(parameters).FirstOrDefault();
                            roles.Add(role);
                        }

                        return roles;
                    }
                }

                return roles;
            }
        }

        public Task<IEnumerable<Role>> HandleAsync(SelectRolesByUserNameQuery query)
        {
            throw new NotImplementedException();
        }
    }
}