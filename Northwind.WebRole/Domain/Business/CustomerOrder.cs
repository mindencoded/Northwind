using System.ComponentModel.DataAnnotations.Schema;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Domain.Business
{
    [View("CUSTOMER_ORDER")]
    public class CustomerOrder : Entity
    {
        [Column("CUSTOMER_ID")]
        public int CustomerId { get; set; }
        [Column("CUSTOMER_NAME")]
        public string CustomerName { get; set; }
        [Column("ORDER_ID")]
        public int OrderId { get; set; }
        [Column("EMPLOYEE_NAME")]
        public string EmployeeName { get; set; }
        [Column("TOTAL")]
        public decimal Total { get; set; }
    }
}