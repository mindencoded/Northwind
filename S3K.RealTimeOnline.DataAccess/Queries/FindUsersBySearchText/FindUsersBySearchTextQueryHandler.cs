using System.Collections.Generic;
using System.Linq;
using Awesome.Data.Sql.Builder;
using S3K.RealTimeOnline.DataAccess.Properties;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.Domain;
using Serilog;

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
                
                var statement = SqlStatements.Select("u.ID", "u.USERNAME", "u.ACTIVE")
                    .From("USER u")
                    .Where("u.ACTIVE <> @IncludeInactiveUsers")
                    .Where("u.USERNAME LIKE '%' + @SearchText + '%'")
                    .OrderBy("u.USERNAME", false);

                var sql = statement.ToSql();

                Log.Debug(sql);

                //IEnumerable<User> users = _unitOfWork.ExecuteQueryFunction<User>("FINDUSERSBYSEARCHTEXTQUERY", query);

                IEnumerable<User> users = _unitOfWork.ExecuteQueryText<User>(Resources.FindUsersBySearchTextQuery, query);

                var user = new User {Username = query.SearchText, Active = !query.IncludeInactiveUsers};
                return _unitOfWork.UserRepository.Select(user).ToArray();
            }
        }
    }
}