using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.Repositories
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