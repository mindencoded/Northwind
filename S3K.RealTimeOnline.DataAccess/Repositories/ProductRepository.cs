using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SqlConnection connection) : base(connection)
        {
        }

        public ProductRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public ProductRepository(IBusinessUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}