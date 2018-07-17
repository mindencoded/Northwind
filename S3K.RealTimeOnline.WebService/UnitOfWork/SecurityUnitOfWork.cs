using System.Data.SqlClient;

namespace S3K.RealTimeOnline.WebService.UnitOfWork
{
    public class SecurityUnitOfWork : UnitOfWork, ISecurityUnitOfWork
    {
        public SecurityUnitOfWork(SqlConnection connection) : base(connection)
        {
        }
    }
}