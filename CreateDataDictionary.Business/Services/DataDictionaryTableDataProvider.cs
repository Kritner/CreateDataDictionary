using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    public class DataDictionaryTableDataProvider : IDataDictionaryTableDataProvider
    {

        #region private
        private readonly IGetDbTableColumnInfo _iGetDbTableColumnInfo;
        private readonly ITableExclusionRules _iDataDictionaryExclusionRules;
        private readonly ITableModelObjectCreator _iDataDictionaryObjectCreator;
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iGetDbTableColumnInfo">Get information from the DB</param>
        /// <param name="iDataDictionaryExclusionRules">The exclusion rules to exclude tables from the resuling output</param>
        /// <param name="iDataDictionaryObjectCreator">Creates model objects based on the raw data</param>
        public DataDictionaryTableDataProvider(
            IGetDbTableColumnInfo iGetDbTableColumnInfo,
            ITableExclusionRules iDataDictionaryExclusionRules,
            ITableModelObjectCreator iDataDictionaryObjectCreator
        )
        {
            if (iGetDbTableColumnInfo == null)
                throw new ArgumentNullException(nameof(iGetDbTableColumnInfo));
            if (iDataDictionaryExclusionRules == null)
                throw new ArgumentNullException(nameof(iDataDictionaryExclusionRules));
            if (iDataDictionaryObjectCreator == null)
                throw new ArgumentNullException(nameof(iDataDictionaryObjectCreator));

            _iGetDbTableColumnInfo = iGetDbTableColumnInfo;
            _iDataDictionaryExclusionRules = iDataDictionaryExclusionRules;
            _iDataDictionaryObjectCreator = iDataDictionaryObjectCreator;
        }
        #endregion ctor

        public IEnumerable<TableInfo> Execute()
        {
            // Get the data from the db
            var dbRawData = _iGetDbTableColumnInfo.GetTableColumnInformation();

            // Exclude the tables from the data that should be filtered
            var filteredData = _iDataDictionaryExclusionRules.FilterTablesMeetingRuleCriteria(
                _iDataDictionaryExclusionRules.GetRules(),
                dbRawData
            );

            // Generate the objects for use in the generation of the report
            return _iDataDictionaryObjectCreator.TransformRawDataIntoFormattedObjects(filteredData);
        }
    }
}
