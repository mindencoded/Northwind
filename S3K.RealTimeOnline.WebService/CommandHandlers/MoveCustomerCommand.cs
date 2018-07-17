namespace S3K.RealTimeOnline.WebService.CommandHandlers
{
    public class MoveCustomerCommand
    {
        public int CustomerId { get; set; }

        public Address NewAddress { get; set; }
    }
}