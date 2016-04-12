using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Interface to provide a <see cref="IDbConnection"/>
    /// </summary>
    public interface IBaseDatabaseConnection
    {
        /// <summary>
        /// Get a database connection
        /// </summary>
        /// <returns>IDbConnection</returns>
        IDbConnection GetDatabaseConnection();
    }
}
