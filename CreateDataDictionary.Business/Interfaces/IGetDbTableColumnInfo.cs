using System.Collections.Generic;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{

    /// <summary>
    /// Interface for getting the table, column, and schema information from the database
    /// </summary>
    public interface IGetDbTableColumnInfo
    {
        /// <summary>
        /// Get the data from the database, and return as an <see cref="IEnumerable{T}"/> of <see cref="TableColumnInfoRaw"/>
        /// </summary>
        /// <returns>IEnumerable of TableColumnInfoRaw</returns>
        IEnumerable<TableColumnInfoRaw> GetTableColumnInformation();
    }
}