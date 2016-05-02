using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Interface a means of excluding tables from the data dictionary
    /// </summary>
    public interface ITableExcluder
    {
        /// <summary>
        /// Take in the <see cref="IEnumerable{T}"/> of <see cref="TableColumnInfoRaw"/>,
        /// entries based on implementation, and return.
        /// </summary>
        /// <param name="tables">The tables to evaluate for removal</param>
        /// <returns>The tables post removal.</returns>
        IEnumerable<TableColumnInfoRaw> RemoveTables(IEnumerable<TableColumnInfoRaw> tables);
    }
}
