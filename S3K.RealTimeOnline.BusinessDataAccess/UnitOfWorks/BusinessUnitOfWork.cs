using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWorks;

namespace S3K.RealTimeOnline.BusinessDataAccess.UnitOfWorks
{
    public class BusinessUnitOfWork : UnitOfWork, IBusinessUnitOfWork
    {
        public BusinessUnitOfWork(SqlConnection connection) : base(connection)
        {
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(ProductRepository)))
                    Repositories.Add(typeof(ProductRepository), new ProductRepository(Connection, Transaction));

                return (IProductRepository) Repositories[typeof(ProductRepository)];
            }
        }
    }
}