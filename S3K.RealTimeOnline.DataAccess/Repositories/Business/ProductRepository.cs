using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks.Business;
using S3K.RealTimeOnline.Domain.Entities.Business;

namespace S3K.RealTimeOnline.DataAccess.Repositories.Business
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        
        public ProductRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public ProductRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction) : base(sqlConnection, sqlTransaction)
        {
        }

        public ProductRepository(IBusinessUnitOfWork unitOfWork) : base(unitOfWork)
        {    
        }
    }
}
