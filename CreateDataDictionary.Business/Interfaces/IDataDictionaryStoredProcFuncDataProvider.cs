using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Provides interface for getting data dictionary information pertaining to Stored procedures and functions
    /// </summary>
    public interface IDataDictionaryStoredProcFuncDataProvider
    {
        /// <summary>
        /// Gets and returns data based on stored procs/functions within the database
        /// </summary>
        /// <returns>IEnumerable of StoredProcFuncInfo</returns>
        IEnumerable<StoredProcFuncInfo> Execute();
    }
}
