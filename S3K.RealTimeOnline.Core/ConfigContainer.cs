using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.Contracts;
using S3K.RealTimeOnline.GenericDataAccess.CommandHandlers;
using S3K.RealTimeOnline.GenericDataAccess.QueryHandlers;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDomain;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace S3K.RealTimeOnline.Core
{
    public class ConfigContainer
    {
        private static ConfigContainer _instance;

        private ConfigContainer(IUnityContainer container)
        {
            Init(container);
        }

        public static ConfigContainer Instance(IUnityContainer container)
        {
            if (_instance == null)
            {
                _instance = new ConfigContainer(container);
            }
            return _instance;
        }

        private void Init(IUnityContainer container)
        {
            string securityDbConnectionName = ConfigurationManager.AppSettings["SecurityDbConnectionName"];
            string businessDbConnectionName = ConfigurationManager.AppSettings["BusinessDbConnectionName"];
            container.RegisterType<ISecurityUnitOfWork, SecurityUnitOfWork>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(DbManager.GetSqlConnection(securityDbConnectionName)));
            container.RegisterType<IBusinessUnitOfWork, BusinessUnitOfWork>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(DbManager.GetSqlConnection(businessDbConnectionName)));
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x =>
                x.GetName().Name == "S3K.RealTimeOnline.BusinessDataAccess" ||
                x.GetName().Name == "S3K.RealTimeOnline.SecurityDataAccess" ||
                x.GetName().Name == "S3K.RealTimeOnline.CommonDataAccess");

            foreach (var assembly in assemblies)
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(t => t.GetInterfaces()
                                                                             .Count(i => i.IsGenericType &&
                                                                                         (i.GetGenericTypeDefinition() ==
                                                                                          typeof(ICommandHandler<>) ||
                                                                                          i.GetGenericTypeDefinition() ==
                                                                                          typeof(IQueryHandler<,>))) >
                                                                         0);

                foreach (var type in types)
                {
                    Type[] interfaces = type.GetInterfaces();
                    container.RegisterType(interfaces.First(), type);
                }
            }

            Type[] genericHandlerTypes =
            {
                typeof(GenericSelectQueryHandler<,>),
                typeof(GenericDeleteByIdCommandHandler<,>),
                typeof(GenericDeleteCommandHandler<,>),
                typeof(GenericInsertCommandHandler<,>),
                typeof(GenericUpdateCommandHandler<,>)
            };

            IEnumerable<Type> businessDomainTypes = AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.GetName().Name == "S3K.RealTimeOnline.BusinessDomain").GetTypes()
                .Where(t => typeof(Entity).IsAssignableFrom(t));

            foreach (var type in businessDomainTypes)
            {
                foreach (var genericHandlerType in genericHandlerTypes)
                {
                    Type genericType =
                        genericHandlerType.MakeGenericType(
                            typeof(IBusinessUnitOfWork), type);
                    container.RegisterType(genericType);
                }
            }

            IEnumerable<Type> securityDomainTypes = AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.GetName().Name == "S3K.RealTimeOnline.SecurityDomain").GetTypes()
                .Where(t => typeof(Entity).IsAssignableFrom(t));

            foreach (var type in securityDomainTypes)
            {
                foreach (var genericHandlerType in genericHandlerTypes)
                {
                    Type genericType =
                        genericHandlerType.MakeGenericType(
                            typeof(ISecurityUnitOfWork), type);
                    container.RegisterType(genericType);
                }
            }

            IEnumerable<Type> serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.GetName().Name == "S3K.RealTimeOnline.Contracts").GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(IMainService)) && mytype.IsInterface);
            foreach (Type type in serviceTypes)
            {
                container.RegisterType(type, typeof(MainService));
            }
        }
    }
}