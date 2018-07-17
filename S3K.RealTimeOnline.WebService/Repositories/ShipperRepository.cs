using System.Data.SqlClient;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.Repositories
{
    public class ShipperRepository : Repository<Shipper>, IShipperRepository
    {
        public ShipperRepository(SqlConnection connection) : base(connection)
        {
        }

        public ShipperRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public ShipperRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public ShipperRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public ShipperRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ShipperRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}