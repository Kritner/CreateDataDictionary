using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Used to get the table exclusion rules for the data dictionary
    /// </summary>
    public interface ITableExclusionRules
    {
        /// <summary>
        /// Get the rules for DataDictionary table exclusion
        /// </summary>
        /// <returns></returns>
        IEnumerable<IDataDictionaryTableExcluder> GetRules();

        /// <summary>
        /// Filter tables 
        /// </summary>
        /// <param name="rules">The rules to filter based on</param>
        /// <param name="preFilterTables">The tables to filter</param>
        /// <returns>The filtered tables</returns>
        IEnumerable<TableColumnInfoRaw> FilterTablesMeetingRuleCriteria(IEnumerable<IDataDictionaryTableExcluder> rules, IEnumerable<TableColumnInfoRaw> preFilterTables);
    }
}
