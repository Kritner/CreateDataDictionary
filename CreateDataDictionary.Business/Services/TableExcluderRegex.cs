using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Class is used to exclude tables from the data dictionary as per the <see cref="Regex"/> passed in the constructor
    /// </summary>
    public class TableExcluderRegex : ITableExcluder
    {
        #region Protected
        /// <summary>
        /// Regex used to filter results from the data dictionary
        /// </summary>
        protected Regex _regex;
        #endregion Protected

        #region ctor
        /// <summary>
        /// Constructor - takes in regex
        /// </summary>
        /// <param name="regex">The regex to use for table removal</param>
        public TableExcluderRegex(Regex regex)
        {
            _regex = regex;
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Removes tables based on the regex provided in the constructor
        /// </summary>
        /// <param name="tables">The list to parse</param>
        /// <returns>The list after removal.</returns>
        public IEnumerable<TableColumnInfoRaw> RemoveTables(IEnumerable<TableColumnInfoRaw> tables)
        {
            List<TableColumnInfoRaw> list = tables.ToList();
            list.RemoveAll(item => _regex.IsMatch(item.TableName));

            return list;
        }
        #endregion Public methods
    }
}
