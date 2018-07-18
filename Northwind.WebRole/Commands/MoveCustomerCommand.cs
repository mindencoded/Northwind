using Northwind.WebRole.Domain;

namespace Northwind.WebRole.Commands
{
    public class MoveCustomerCommand
    {
        public int CustomerId { get; set; }

        public Address NewAddress { get; set; }
    }
}