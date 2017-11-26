using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(SqlConnection connection) : base(connection)
        {
        }

        public RoleRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public RoleRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public RoleRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection,
            transaction, ignoreNulls)
        {
        }

        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public RoleRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}