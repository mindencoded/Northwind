using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories;

namespace S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork
{
    public class BusinessUnitOfWork : GenericDataAccess.UnitOfWork.UnitOfWork, IBusinessUnitOfWork
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

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(CustomerRepository)))
                    Repositories.Add(typeof(CustomerRepository), new CustomerRepository(Connection, Transaction));

                return (ICustomerRepository) Repositories[typeof(CustomerRepository)];
            }
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(EmployeeRepository)))
                    Repositories.Add(typeof(EmployeeRepository), new EmployeeRepository(Connection, Transaction));

                return (IEmployeeRepository) Repositories[typeof(EmployeeRepository)];
            }
        }

        public IInventoryTransactionTypeRepository InventoryTransactionTypeRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(InventoryTransactionTypeRepository)))
                    Repositories.Add(typeof(InventoryTransactionTypeRepository),
                        new InventoryTransactionTypeRepository(Connection, Transaction));

                return (IInventoryTransactionTypeRepository) Repositories[typeof(InventoryTransactionTypeRepository)];
            }
        }
    }
}