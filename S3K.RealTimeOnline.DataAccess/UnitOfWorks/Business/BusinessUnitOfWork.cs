using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.Repositories.Business;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks.Business
{
    public class BusinessUnitOfWork : UnitOfWork, IBusinessUnitOfWork
    {
        public BusinessUnitOfWork(SqlConnection sqlConnection, bool isTransactional = true) : base(sqlConnection,
            isTransactional)
        {
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(ProductRepository)))
                    Repositories.Add(typeof(ProductRepository), new ProductRepository(SqlConnection, SqlTransaction));

                return (IProductRepository) Repositories[typeof(ProductRepository)];
            }
        }
    }
}