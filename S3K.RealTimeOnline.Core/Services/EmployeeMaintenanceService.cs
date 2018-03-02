using System.IO;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Core.Services
{
    public partial class MaintenanceService : IEmployeeMaintenanceService
    {
        public Stream SelectEmployee(string page, string pageSize)
        {
            return Select<IBusinessUnitOfWork, Employee>(page, pageSize);
        }

        public void InsertEmployee(EmployeeDto dto)
        {
            Insert<IBusinessUnitOfWork, Employee, EmployeeDto>(dto);
        }

        public void UpdateEmployee(string id, EmployeeDto dto)
        {
            Update<IBusinessUnitOfWork, Employee, EmployeeDto>(id, dto);
        }

        public void PartialUpdateEmployee(string id, string json)
        {
            PartialUpdate<IBusinessUnitOfWork, Employee>(id, json);
        }

        public void DeleteEmployeeById(string id)
        {
            DeleteById<IBusinessUnitOfWork, Employee>(id);
        }

        public Stream SelectEmployeeById(string id)
        {
            return SelectById<IBusinessUnitOfWork, Employee>(id);
        }
    }
}