using System.Data.SqlClient;

namespace S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork
{
    public class SecurityUnitOfWork : GenericDataAccess.UnitOfWork.UnitOfWork, ISecurityUnitOfWork
    {
        public SecurityUnitOfWork(SqlConnection connection) : base(connection)
        {
        }
    }
}