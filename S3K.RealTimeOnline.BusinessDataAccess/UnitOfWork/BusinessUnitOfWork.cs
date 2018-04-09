using System.Data.SqlClient;

namespace S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork
{
    public class BusinessUnitOfWork : GenericDataAccess.UnitOfWork.UnitOfWork, IBusinessUnitOfWork
    {
        public BusinessUnitOfWork(SqlConnection connection) : base(connection)
        {
        }
    }
}