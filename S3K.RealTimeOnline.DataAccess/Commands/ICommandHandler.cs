namespace S3K.RealTimeOnline.DataAccess.Commands
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
