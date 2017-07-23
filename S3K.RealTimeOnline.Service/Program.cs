using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Microsoft.Practices.Unity;
using S3K.RealTimeOnline.Commons;
using S3K.RealTimeOnline.DataAccess.QuerieObjects;
using S3K.RealTimeOnline.DataAccess.QuerieObjects.Security.FindUsersBySearchText;
using S3K.RealTimeOnline.DataAccess.Repositories.Security;
using S3K.RealTimeOnline.DataAccess.Tools;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks.Business;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks.Security;
using S3K.RealTimeOnline.Domain.Entities.Business;
using S3K.RealTimeOnline.Domain.Entities.Security;
using Serilog;

namespace S3K.RealTimeOnline.Service
{
    internal class Program
    {
        public delegate int BinaryOp(int x, int y);

        public delegate string MyDelegate(string txt);

        private static readonly ContainerBootstrapper Bootstrapper;

        private static readonly Container Container;

        static Program()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RollingFile(@"log-{Date}.txt")
                .WriteTo.ColoredConsole()
                .CreateLogger();
            Container = ConfigureContainer();
            Bootstrapper = ConfigureContainerBootstrapper();
        }

        private static void Main()
        {
            //SelectUserById(container);
            //SelectUserByUsername(container);
            //SelectProductByName(container);

            var queryProcessor = (IQueryProcessor) new QueryProcessor(Bootstrapper);
            var parameter = new FindUsersBySearchTextQuery {SearchText = "Mark Smith", IncludeInactiveUsers = false};
            var users = queryProcessor.Process<IQuery<User[]>, User[]>(parameter);

            // var queryHandler =  Bootstrapper.Resolve<FindUsersBySearchTextQueryHandler>();
            // User[] users = queryHandler.Handle(query);


            //Print out the ID of the executing thread
            //Console.WriteLine("Main() running on thread {0}", Thread.CurrentThread.ManagedThreadId);

            /*
            BinaryOp bp = Add;
            IAsyncResult iftAr = bp.BeginInvoke(5, 5, AddComplete, "This message is from Main() thread " + Thread.CurrentThread.ManagedThreadId);
            while (!iftAr.AsyncWaitHandle.WaitOne(50, true))
            {
                Console.WriteLine("Doing some work in Main()!");
            }
            */

            /*
            //Invoke Add() on a secondary thread. The object required here to access Add() inside static method so don’t have to bother
            BinaryOp b = Add;
            IAsyncResult iftAr = b.BeginInvoke(5, 5, null, null);
            //Do some other work on priamry thread..
            Console.WriteLine("Doing some work in Main()!");
            //Obtain the reault of Add() method when ready
            int result = b.EndInvoke(iftAr);
            Console.WriteLine("5 + 5 is {0}", result);
            */

            /*
            Console.WriteLine("================= SYNC =================");
            SynchronousDelegateSample();
            Console.WriteLine("================= ASYNC ================");
            AsynchronousDelegateSample();
            */
            Console.Read();
        }

        //An Add() method that do some simple arithamtic operation
        private static int Add(int a, int b)
        {
            Console.WriteLine("Add() running on thread {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(500);
            return a + b;
        }

        //Target of AsyncCallback delegate should match the following pattern
        private static void AddComplete(IAsyncResult iftAr)
        {
            Console.WriteLine("AddComplete() running on thread {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Operation completed.");

            //Getting result
            var ar = (AsyncResult) iftAr;
            var bp = (BinaryOp) ar.AsyncDelegate;
            var result = bp.EndInvoke(iftAr);

            //Recieving the message from Main() thread.
            var msg = (string) iftAr.AsyncState;
            Console.WriteLine("5 + 5 ={0}", result);
            Console.WriteLine("Message recieved on thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, msg);
        }

        private static void SynchronousDelegateSample()
        {
            var sw = new Stopwatch();
            sw.Start();

            /*
             * All the calls to the delegate will be synchronous
             * This mean that the order will always be 1,2,3,4,5,6
             * The thread used will always be the same.
             */
            MyDelegate dm = DelegateMethod;
            Console.WriteLine(dm("calling delegate (1)"));
            Console.WriteLine(dm("calling delegate (2)"));
            Console.WriteLine(dm("calling delegate (3)"));
            Console.WriteLine(dm("calling delegate (4)"));
            Console.WriteLine(dm("calling delegate (5)"));
            Console.WriteLine(dm("calling delegate (6)"));

            sw.Stop();
            Console.WriteLine("All work was done in: {0} milliseconds.nn", sw.ElapsedMilliseconds);
        }

        private static void AsynchronousDelegateSample()
        {
            var sw = new Stopwatch();
            sw.Start();

            /*
             * All the work will be invoked in a blink.
             * Multiple thread will be used to compute the result of the method.
             */
            MyDelegate dm = DelegateMethod;
            var result1 = dm.BeginInvoke("calling delegate (1)", null, null);
            var result2 = dm.BeginInvoke("calling delegate (2)", null, null);
            var result3 = dm.BeginInvoke("calling delegate (3)", null, null);
            var result4 = dm.BeginInvoke("calling delegate (4)", null, null);
            var result5 = dm.BeginInvoke("calling delegate (5)", null, null);
            var result6 = dm.BeginInvoke("calling delegate (6)", null, null);

            /*
             * EndInvoke is synchronous we force to wait for the asynchronous results.
             */

            var res1 = dm.EndInvoke(result1);
            var res2 = dm.EndInvoke(result2);
            var res3 = dm.EndInvoke(result3);
            var res4 = dm.EndInvoke(result4);
            var res5 = dm.EndInvoke(result5);
            var res6 = dm.EndInvoke(result6);

            Console.WriteLine(res1);
            Console.WriteLine(res2);
            Console.WriteLine(res3);
            Console.WriteLine(res4);
            Console.WriteLine(res5);
            Console.WriteLine(res6);

            sw.Stop();

            Console.WriteLine("All asynchronous  work was done in: {0}",
                sw.ElapsedMilliseconds);
        }

        private static string DelegateMethod(string txt)
        {
            // Wait 5 seconds.
            Thread.Sleep(2000);

            // Work is finished
            Console.WriteLine(txt + " n > Thread ID is: {0}", Thread.CurrentThread.ManagedThreadId);
            return ">> " + txt + " Done !";
        }

        private static void SelectUserById(IContainer container)
        {
            var search = new User
            {
                Id = 2
            };

            using (var userRepository = container.Resolve<IUserRepository>())
            {
                var result = userRepository.SelectById(search);
                if (result != null)
                    Console.WriteLine("Id: {0}, Username: {1}, Password: {2}", result.Id, result.Username,
                        result.Password);
            }
        }

        private static void SelectUserByUsername(IContainer container)
        {
            var search = new User
            {
                Username = "Mark Smith"
            };

            using (var unitOfWork = container.Resolve<ISecurityUnitOfWork>())
            {
                var result = unitOfWork.UserRepository.Select(search).FirstOrDefault();
                if (result != null)
                    Console.WriteLine("Id: {0}, Username: {1}, Password: {2}", result.Id, result.Username,
                        result.Password);
                else
                    Console.WriteLine("User Not Found!");
            }
        }

        private static void SelectProductByName(IContainer container)
        {
            var search = new Product
            {
                Name = "COCA-COLA"
            };

            using (var unitOfWork = container.Resolve<IBusinessUnitOfWork>())
            {
                var productRepository = unitOfWork.ProductRepository;
                var result = productRepository.Select(search).FirstOrDefault();
                if (result != null)
                    Console.WriteLine("Id: {0}, Name: {1}, Description: {2}", result.Id, result.Name,
                        result.Description);
                else
                    Console.WriteLine("Product Not Found!");
            }
        }

        private static Container ConfigureContainer()
        {
            var container = new Container();

            container.Configuration.Add("ConnectionNameSecurityDb",
                ConfigurationManager.AppSettings["ConnectionNameSecurityDb"]);
            container.Configuration.Add("ConnectionNameBusinessDb",
                ConfigurationManager.AppSettings["ConnectionNameBusinessDb"]);

            container.Register<ISecurityUnitOfWork>(delegate
            {
                var connectionName = container.GetConfiguration<string>("ConnectionNameSecurityDb");
                return new SecurityUnitOfWork(DbManager.GetSqlConnection(connectionName));
            });

            container.Register<IBusinessUnitOfWork>(delegate
            {
                var connectionName = container.GetConfiguration<string>("ConnectionNameBusinessDb");
                return new BusinessUnitOfWork(DbManager.GetSqlConnection(connectionName));
            });

            container.Register<IQueryHandler<FindUsersBySearchTextQuery, User[]>>(delegate
            {
                return new FindUsersBySearchTextQueryHandler(container.Resolve<ISecurityUnitOfWork>());
            });

            return container;
        }

        private static ContainerBootstrapper ConfigureContainerBootstrapper()
        {
            var unityContainer = new UnityContainer();
            var connectionNameSecurityDb = ConfigurationManager.AppSettings["ConnectionNameSecurityDb"];
            var connectionNameBusinessDb = ConfigurationManager.AppSettings["ConnectionNameBusinessDb"];
            unityContainer.RegisterType<ISecurityUnitOfWork, SecurityUnitOfWork>(
                new InjectionConstructor(DbManager.GetSqlConnection(connectionNameSecurityDb), true));
            unityContainer.RegisterType<IBusinessUnitOfWork, BusinessUnitOfWork>(
                new InjectionConstructor(DbManager.GetSqlConnection(connectionNameBusinessDb), true));
            unityContainer
                .RegisterType<IQueryHandler<FindUsersBySearchTextQuery, User[]>, FindUsersBySearchTextQueryHandler>(
                    new InjectionConstructor(unityContainer.Resolve<ISecurityUnitOfWork>()));
            return new ContainerBootstrapper(unityContainer);
        }
    }
}