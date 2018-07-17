using System.Data.SqlClient;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.Repositories
{
    public class RoleDetailRepository : Repository<RoleDetail>, IRoleDetailRepository
    {
        public RoleDetailRepository(SqlConnection connection) : base(connection)
        {
        }

        public RoleDetailRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public RoleDetailRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public RoleDetailRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public RoleDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public RoleDetailRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}