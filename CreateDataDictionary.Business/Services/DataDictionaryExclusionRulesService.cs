using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// The rules for excluding tables from the DataDictionary
    /// </summary>
    public class DataDictionaryExclusionRulesService : IDataDictionaryExclusionRules
    {

        #region const
        public const string _WEB_CONFIG_KEY_TABLE_EXCLUSION = "CommaDelimitedTableExclusionList";
        #endregion const

        #region private
        List<string> _tablesToExclude = new List<string>();
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor
        /// </summary>
        public DataDictionaryExclusionRulesService()
        {
            _tablesToExclude = ConfigurationManager.AppSettings[_WEB_CONFIG_KEY_TABLE_EXCLUSION].Split(',').ToList();
        }
        #endregion ctor

        /// <summary>
        /// Get the rules for data dictionary table exclusion.
        /// </summary>
        /// <returns>IEnumerable of IDataDictionaryTableExcluder</returns>
        public IEnumerable<IDataDictionaryTableExcluder> GetRules()
        {
            List<IDataDictionaryTableExcluder> list = new List<IDataDictionaryTableExcluder>();
            list.Add(new DataDictionaryTableExcluderList(_tablesToExclude));
            list.Add(new DataDictionaryTableExcluderRegex(new Regex(@"bak", RegexOptions.Compiled | RegexOptions.IgnoreCase)));
            list.Add(new DataDictionaryTableExcluderRegex(new Regex(@"bkup", RegexOptions.Compiled | RegexOptions.IgnoreCase)));
            list.Add(new DataDictionaryTableExcluderRegex(new Regex(@"tmp", RegexOptions.Compiled | RegexOptions.IgnoreCase)));
            list.Add(new DataDictionaryTableExcluderRegex(new Regex(@"temp", RegexOptions.Compiled | RegexOptions.IgnoreCase)));

            return list;
        }

        /// <summary>
        /// Filter the tables based on the rules
        /// </summary>
        /// <param name="rules">The rules to use to filter</param>
        /// <param name="preFilterTables">The tables to filter</param>
        /// <returns>The filtered tables</returns>
        public IEnumerable<TableColumnInfoRaw> FilterTablesMeetingRuleCriteria(IEnumerable<IDataDictionaryTableExcluder> rules, IEnumerable<TableColumnInfoRaw> preFilterTables)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));
            if (preFilterTables == null)
                throw new ArgumentNullException(nameof(preFilterTables));
            if (preFilterTables.Count() == 0)
                throw new ArgumentException(nameof(preFilterTables));

            // List to track tables after applying filters
            List<TableColumnInfoRaw> list = new List<TableColumnInfoRaw>();
            list.AddRange(preFilterTables);

            foreach (IDataDictionaryTableExcluder rule in rules)
                list = rule.RemoveTables(list).ToList();

            return list;
        }
    }
}
