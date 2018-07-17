using System.Data.SqlClient;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SqlConnection connection) : base(connection)
        {
        }

        public ProductRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public ProductRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public ProductRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ProductRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}