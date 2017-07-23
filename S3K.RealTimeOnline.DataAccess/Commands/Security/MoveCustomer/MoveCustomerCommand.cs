using S3K.RealTimeOnline.Domain.Entities.Security;

namespace S3K.RealTimeOnline.DataAccess.Commands.Security.MoveCustomer
{
    public class MoveCustomerCommand
    {
        public int CustomerId { get; set; }

        public Address NewAddress { get; set; }
    }
}
