﻿using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class ProductCrudService : CrudService<IBusinessUnitOfWork, Product, ProductDto>, IProductCrudService
    {
        public ProductCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IProductCrudService>(config, "");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Select")]
        public override Stream SelectA(string page, string pageSize)
        {
            return base.SelectA(page, pageSize);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby)
        {
            return base.SelectB(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectC(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Select")]
        public override Stream Select(string page, string pageSize, string @orderby, string filter, string select)
        {
            return base.Select(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Insert")]
        public override void Insert(ProductDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Update")]
        public override void Update(string id, ProductDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.DeleteById")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CrudProduct.SelectById")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }
    }
}