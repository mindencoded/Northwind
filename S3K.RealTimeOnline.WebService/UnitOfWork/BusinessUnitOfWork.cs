using System.Data.SqlClient;

namespace S3K.RealTimeOnline.WebService.UnitOfWork
{
    public class BusinessUnitOfWork : UnitOfWork, IBusinessUnitOfWork
    {
        public BusinessUnitOfWork(SqlConnection connection) : base(connection)
        {
        }
    }
}