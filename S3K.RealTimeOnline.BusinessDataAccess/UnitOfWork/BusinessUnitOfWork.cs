using System.Data.SqlClient;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories;
using S3K.RealTimeOnline.BusinessDataAccess.Repositories.Contracts;

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

        public IShipperRepository ShipperRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(ShipperRepository)))
                    Repositories.Add(typeof(ShipperRepository),
                        new ShipperRepository(Connection, Transaction));

                return (IShipperRepository) Repositories[typeof(ShipperRepository)];
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(OrderRepository)))
                    Repositories.Add(typeof(OrderRepository),
                        new OrderRepository(Connection, Transaction));

                return (IOrderRepository) Repositories[typeof(OrderRepository)];
            }
        }

        public IOrderStatusRepository OrderStatusRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(OrderStatusRepository)))
                    Repositories.Add(typeof(OrderStatusRepository),
                        new OrderStatusRepository(Connection, Transaction));

                return (IOrderStatusRepository) Repositories[typeof(OrderStatusRepository)];
            }
        }

        public IOrderTaxStatusRepository OrderTaxStatusRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(OrderTaxStatusRepository)))
                    Repositories.Add(typeof(OrderTaxStatusRepository),
                        new OrderTaxStatusRepository(Connection, Transaction));

                return (IOrderTaxStatusRepository) Repositories[typeof(OrderTaxStatusRepository)];
            }
        }

        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(OrderDetailRepository)))
                    Repositories.Add(typeof(OrderDetailRepository),
                        new OrderDetailRepository(Connection, Transaction));

                return (IOrderDetailRepository) Repositories[typeof(OrderDetailRepository)];
            }
        }

        public IOrderDetailStatusRepository OrderDetailStatusRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(OrderDetailStatusRepository)))
                    Repositories.Add(typeof(OrderDetailStatusRepository),
                        new OrderDetailStatusRepository(Connection, Transaction));

                return (IOrderDetailStatusRepository) Repositories[typeof(OrderDetailStatusRepository)];
            }
        }

        public ISupplierRepository SupplierRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(SupplierRepository)))
                    Repositories.Add(typeof(SupplierRepository),
                        new SupplierRepository(Connection, Transaction));

                return (ISupplierRepository)Repositories[typeof(SupplierRepository)];
            }
        }

        public IPurchaseOrderRepository PurchaseOrderRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(PurchaseOrderRepository)))
                    Repositories.Add(typeof(PurchaseOrderRepository),
                        new PurchaseOrderRepository(Connection, Transaction));

                return (IPurchaseOrderRepository)Repositories[typeof(PurchaseOrderRepository)];
            }
        }

        public IPurchaseOrderDetailRepository PurchaseOrderDetailRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(PurchaseOrderDetailRepository)))
                    Repositories.Add(typeof(PurchaseOrderDetailRepository),
                        new PurchaseOrderDetailRepository(Connection, Transaction));

                return (IPurchaseOrderDetailRepository)Repositories[typeof(PurchaseOrderDetailRepository)];
            }
        }

        public IPurchaseOrderStatusRepository PurchaseOrderStatusRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(PurchaseOrderStatusRepository)))
                    Repositories.Add(typeof(PurchaseOrderStatusRepository),
                        new PurchaseOrderStatusRepository(Connection, Transaction));

                return (IPurchaseOrderStatusRepository)Repositories[typeof(PurchaseOrderStatusRepository)];
            }
        }

        public IInventoryTransactionRepository InventoryTransactionRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(InventoryTransactionRepository)))
                    Repositories.Add(typeof(InventoryTransactionRepository),
                        new InventoryTransactionRepository(Connection, Transaction));

                return (IInventoryTransactionRepository)Repositories[typeof(InventoryTransactionRepository)];
            }
        }

        public IInvoiceRepository InvoiceRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(InvoiceRepository)))
                    Repositories.Add(typeof(InvoiceRepository),
                        new InvoiceRepository(Connection, Transaction));

                return (IInvoiceRepository)Repositories[typeof(InvoiceRepository)];
            }
        }
    }
}