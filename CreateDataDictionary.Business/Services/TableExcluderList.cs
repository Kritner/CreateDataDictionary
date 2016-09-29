using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{

    /// <summary>
    /// Contains a list of tables to exclude from the data dictionary
    /// </summary>
    public class TableExcluderList : ITableExcluder
    {
        #region Protected
        /// <summary>
        /// The list of tables that should be excluded from the data dictionary
        /// </summary>
        protected List<string> _TablesToExclude = new List<string>()
        {
            "aspnet_Paths",
            "aspnet_PersonalizationAllUsers",
            "aspnet_PersonalizationPerUser",
            "aspnet_SchemaVersions",
            "aspnet_WebEvent_Events",
            "ASPSession",
            "dtproperties",
            "Results"
        };
        #endregion Protected

        #region ctor
        /// <summary>
        /// Constructor - takes in additional tables to exclude
        /// </summary>
        /// <param name="additionalTablesToExclude"></param>
        public TableExcluderList(IEnumerable<string> additionalTablesToExclude)
        {
            if (additionalTablesToExclude != null && additionalTablesToExclude.Count() > 0)
                _TablesToExclude.AddRange(additionalTablesToExclude);
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Remove the tables as defined in <see cref="_TablesToExclude"/>
        /// </summary>
        /// <param name="tables">The tables to evaluate for removal</param>
        /// <returns>The tables post removal</returns>
        public IEnumerable<TableColumnInfoRaw> RemoveTables(IEnumerable<TableColumnInfoRaw> tables)
        {
            List<TableColumnInfoRaw> list = tables.ToList();
            list.RemoveAll(item => _TablesToExclude.Contains(item.TableName, StringComparer.OrdinalIgnoreCase));
            
            return list;
        }
        #endregion Public methods
    }
}
