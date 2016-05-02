using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using System.Configuration;
using System.Data.SqlClient;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Interface to provide a <see cref="IDbConnection"/>
    /// </summary>
    public class BaseDatabaseConnection : IBaseDatabaseConnection
    {
        /// <summary>
        /// The connection string name to use for database connections
        /// </summary>
        public const string _CONNECTION_STRING_NAME = "DefaultConnectionString";

        #region Properties
        /// <summary>
        /// The connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_CONNECTION_STRING_NAME].ConnectionString;
            }
        }
        #endregion Properties

        #region Public methods
        /// <summary>
        /// Get a database connection
        /// </summary>
        /// <returns>IDbConnection</returns>
        public IDbConnection GetDatabaseConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        #endregion Public methods
    }
}
