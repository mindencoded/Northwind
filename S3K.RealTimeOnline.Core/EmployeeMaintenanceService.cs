using System.IO;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Core
{
    public partial class BusinessService : IEmployeeMaintenanceService
    {
        public Stream SelectEmployee(string page, string pageSize)
        {
            return Select<IBusinessUnitOfWork, Employee>(page, pageSize);
        }

        public Stream SelectEmployeeById(string id)
        {
            return SelectById<IBusinessUnitOfWork, Employee>(id);
        }

        public void InsertEmployee(EmployeeDto command)
        {
            Insert<IBusinessUnitOfWork, Employee, EmployeeDto>(command);
        }

        public void UpdateEmployee(string id, EmployeeDto command)
        {
            Update<IBusinessUnitOfWork, Employee, EmployeeDto>(id, command);
        }

        public void PartialUpdateEmployee(string id, string json)
        {
            PartialUpdate<IBusinessUnitOfWork, Employee>(id, json);
        }

        public void DeleteEmployeeById(string id)
        {
            DeleteById<IBusinessUnitOfWork, Employee>(id);
        }
    }
}