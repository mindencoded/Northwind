﻿using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Northwind.WebRole.Contracts;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Dtos;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;
using Unity;

namespace Northwind.WebRole.Services
{
    [RoutePrefix("RoleGroups.svc")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RoleGroupService : CommandService<ISecurityUnitOfWork, RoleGroup, RoleGroupDto>, IRoleGroupService
    {
        public RoleGroupService(IUnityContainer container) : base(container)
        {
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Select")]
        public override Stream Select(string page, string pageSize, string orderby, string filter)
        {
            return base.Select(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Select")]
        public override Stream Select(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.Select(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Insert")]
        public override void Insert(RoleGroupDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Update")]
        public override void Update(string id, RoleGroupDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "RoleGroup.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }

        public static void Configure(ServiceConfiguration config)
        {
            Configure<IRoleGroupService>(config, "");
        }
    }
}