namespace S3K.RealTimeOnline.GenericDataAccess.Commands
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}