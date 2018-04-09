using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.Repositories
{
    public class RoleGroupRepository : Repository<RoleGroup>, IRoleGroupRepository
    {
        public RoleGroupRepository(SqlConnection connection) : base(connection)
        {
        }

        public RoleGroupRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public RoleGroupRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public RoleGroupRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public RoleGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public RoleGroupRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}