using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Provides data for the table portion of the data dictionary
    /// </summary>
    public interface IDataDictionaryTableDataProvider
    {
        /// <summary>
        /// Get table data
        /// </summary>
        /// <returns>IEnumerable of TableInfo</returns>
        IEnumerable<TableInfo> Execute();
    }
}
