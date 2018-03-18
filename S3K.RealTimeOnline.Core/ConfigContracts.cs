using System;
using System.Collections.Generic;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Core.Services;

namespace S3K.RealTimeOnline.Core
{
    public class ConfigContracts
    {
        public static IDictionary<Type, string> Addresses = new Dictionary<Type, string>
        {
            {typeof(ICustomerCrudService), ""},
            {typeof(IEmployeeCrudService), ""},
            {typeof(IInventoryTransactionCrudService), ""},
            {typeof(IInventoryTransactionTypeCrudService), ""},
            {typeof(IInvoiceCrudService), ""},
            {typeof(IOrderCrudService), ""},
            {typeof(IOrderDetailCrudService), ""},
            {typeof(IOrderDetailStatusCrudService), ""},
            {typeof(IOrderStatusCrudService), ""},
            {typeof(IOrderTaxStatusCrudService), ""},
            {typeof(IProductCrudService), ""},
            {typeof(IPurchaseOrderCrudService), ""},
            {typeof(IPurchaseOrderDetailCrudService), ""},
            {typeof(IPurchaseOrderStatusCrudService), ""},
            {typeof(IRoleCrudService), ""},
            {typeof(IRoleDetailCrudService), ""},
            {typeof(IShipperCrudService), ""},
            {typeof(ISupplierCrudService), ""},
            {typeof(IUserCrudService), ""},
            {typeof(IUserTypeCrudService), ""}
        };

        public static IDictionary<Type, Type> Services = new Dictionary<Type, Type>
        {
            {typeof(ICustomerCrudService), typeof(CustomerCrudService)},
            {typeof(IEmployeeCrudService), typeof(EmployeeCrudService)},
            {typeof(IInventoryTransactionCrudService), typeof(InventoryTransactionCrudService)},
            {typeof(IInventoryTransactionTypeCrudService), typeof(InventoryTransactionTypeCrudService)},
            {typeof(IInvoiceCrudService), typeof(InvoiceCrudService)},
            {typeof(IOrderCrudService), typeof(OrderCrudService)},
            {typeof(IOrderDetailCrudService), typeof(OrderDetailCrudService)},
            {typeof(IOrderDetailStatusCrudService), typeof(OrderDetailStatusCrudService)},
            {typeof(IOrderStatusCrudService), typeof(OrderStatusCrudService)},
            {typeof(IOrderTaxStatusCrudService), typeof(OrderTaxStatusCrudService)},
            {typeof(IProductCrudService), typeof(ProductCrudService)},
            {typeof(IPurchaseOrderCrudService), typeof(PurchaseOrderCrudService)},
            {typeof(IPurchaseOrderDetailCrudService), typeof(PurchaseOrderDetailCrudService)},
            {typeof(IPurchaseOrderStatusCrudService), typeof(PurchaseOrderStatusCrudService)},
            {typeof(IRoleCrudService), typeof(RoleCrudService)},
            {typeof(IRoleDetailCrudService), typeof(RoleDetailCrudService)},
            {typeof(IShipperCrudService), typeof(ShipperCrudService)},
            {typeof(ISupplierCrudService), typeof(SupplierCrudService)},
            {typeof(IUserCrudService), typeof(UserCrudService)},
            {typeof(IUserTypeCrudService), typeof(UserTypeCrudService)}
        };
    }
}