using S3K.RealTimeOnline.DataAccess.UnitOfWorks;

namespace S3K.RealTimeOnline.DataAccess.Commands.InsertUser
{
    public class InsertUserCommandHandler : ICommandHandler<InsertUserCommand>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public InsertUserCommandHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Handle(InsertUserCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
