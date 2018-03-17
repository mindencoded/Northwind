using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Core.Services;
using Unity;
using Unity.Wcf;

namespace S3K.RealTimeOnline.Core
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType(typeof(ICustomerCrudService), typeof(CustomerCrudService));
            container.RegisterType(typeof(IEmployeeCrudService), typeof(EmployeeCrudService));
            container.RegisterType(typeof(IInventoryTransactionCrudService), typeof(InventoryTransactionCrudService));
            container.RegisterType(typeof(IInventoryTransactionTypeCrudService),
                typeof(InventoryTransactionTypeCrudService));
            container.RegisterType(typeof(IInvoiceCrudService), typeof(InvoiceCrudService));
            container.RegisterType(typeof(IOrderCrudService), typeof(OrderCrudService));
            container.RegisterType(typeof(IOrderDetailCrudService), typeof(OrderDetailCrudService));
            container.RegisterType(typeof(IOrderDetailStatusCrudService), typeof(OrderDetailStatusCrudService));
            container.RegisterType(typeof(IOrderStatusCrudService), typeof(OrderStatusCrudService));
            container.RegisterType(typeof(IOrderTaxStatusCrudService), typeof(OrderTaxStatusCrudService));
            container.RegisterType(typeof(IProductCrudService), typeof(ProductCrudService));
            container.RegisterType(typeof(IPurchaseOrderCrudService), typeof(PurchaseOrderCrudService));
            container.RegisterType(typeof(IPurchaseOrderDetailCrudService), typeof(PurchaseOrderDetailCrudService));
            container.RegisterType(typeof(IPurchaseOrderStatusCrudService), typeof(PurchaseOrderStatusCrudService));
            container.RegisterType(typeof(IShipperCrudService), typeof(ShipperCrudService));
            container.RegisterType(typeof(ISupplierCrudService), typeof(SupplierCrudService));
            container.RegisterType(typeof(IRoleCrudService), typeof(RoleCrudService));
            container.RegisterType(typeof(IRoleDetailCrudService), typeof(RoleDetailCrudService));
            container.RegisterType(typeof(IUserCrudService), typeof(UserCrudService));
            container.RegisterType(typeof(IUserTypeCrudService), typeof(UserTypeCrudService));
            ConfigContainer.Instance(container);
        }
    }
}