using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace S3K.RealTimeOnline.WebService.Tools
{
    public class SqlNotificationRequestRegister
    {
        private readonly string _connectionString;
        private readonly string _listenSql;
        private readonly int _notificationTimeout;
        private readonly string _serviceName;
        private SqlCommand _cmd;
        private Task _listenTask;

        public SqlNotificationRequestRegister(string listenSqlStrig, string service, int timeout, string connString)
        {
            _listenSql = listenSqlStrig;
            _serviceName = service;
            _notificationTimeout = timeout;
            _connectionString = connString;
            RegisterSqlNotificationRequest();
        }

        public SqlNotificationRequest NotificationRequest { get; private set; }

        // This event will be invoked after the data is changed.
        public event EventHandler OnChanged;

        /// <summary>
        ///     Begin to monitor the Service Broker queue
        /// </summary>
        public void StartSqlNotification()
        {
            _listenTask = new Task(Listen);
            _listenTask.Start();
        }

        /// <summary>
        ///     Create a SqlNotificationRequest and invoke the event.
        /// </summary>
        private void RegisterSqlNotificationRequest()
        {
            NotificationRequest = new SqlNotificationRequest();
            NotificationRequest.UserData = new Guid().ToString();
            NotificationRequest.Options = string.Format("Service={0};", _serviceName);
            NotificationRequest.Timeout = _notificationTimeout;
            if (OnChanged != null)
                OnChanged(this, null);
        }

        /// <summary>
        ///     Monitoring the Service Broker queue.
        /// </summary>
        private void Listen()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (_cmd = new SqlCommand(_listenSql, conn))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    _cmd.CommandTimeout = _notificationTimeout + 120;

                    using (SqlDataReader reader = _cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            for (int i = 0; i <= reader.FieldCount - 1; i++)
                                Debug.WriteLine(reader[i].ToString());
                    }
                }
            }

            RegisterSqlNotificationRequest();
        }

        public void StopSqlNotification()
        {
            if (_cmd != null)
                _cmd.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposed)
        {
            if (disposed)
                StopSqlNotification();
        }
    }
}