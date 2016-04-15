using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Used to create the data dictionary
    /// </summary>
    public class DataDictionaryCreationService
    {

        #region private
        private readonly IGetDbTableColumnInfo _iGetDbTableColumnInfo;
        private readonly IDataDictionaryExclusionRules _iDataDictionaryExclusionRules;
        private readonly IDataDictionaryObjectCreator _iDataDictionaryObjectCreator;
        private readonly IDataDictionaryReportGenerator _iDataDictionaryReportGenerator;
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iGetDbTableColumnInfo">The IGetDbTableColumnInfo dependency</param>
        /// <param name="iDataDictionaryExclusionRules">The IDataDictionaryExclusionRules dependency</param>
        /// <param name="iDataDictionaryObjectCreator">The IDataDictionaryObjectCreator dependency</param>
        /// <param name="iDataDictionaryReportGenerator">The IDataDictionaryReportGenerator dependency</param>
        public DataDictionaryCreationService(
            IGetDbTableColumnInfo iGetDbTableColumnInfo, 
            IDataDictionaryExclusionRules iDataDictionaryExclusionRules, 
            IDataDictionaryObjectCreator iDataDictionaryObjectCreator,
            IDataDictionaryReportGenerator iDataDictionaryReportGenerator
        )
        {
            if (iGetDbTableColumnInfo == null)
                throw new ArgumentNullException(nameof(iGetDbTableColumnInfo));
            if (iDataDictionaryExclusionRules == null)
                throw new ArgumentNullException(nameof(iDataDictionaryExclusionRules));
            if (iDataDictionaryObjectCreator == null)
                throw new ArgumentNullException(nameof(iDataDictionaryObjectCreator));
            if (iDataDictionaryReportGenerator == null)
                throw new ArgumentNullException(nameof(iDataDictionaryReportGenerator));

            _iGetDbTableColumnInfo = iGetDbTableColumnInfo;
            _iDataDictionaryExclusionRules = iDataDictionaryExclusionRules;
            _iDataDictionaryObjectCreator = iDataDictionaryObjectCreator;
            _iDataDictionaryReportGenerator = iDataDictionaryReportGenerator;
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Generate the report with the filename provided
        /// </summary>
        /// <param name="filename">The filename to create</param>
        public void Execute(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            // Get the data from the db
            var dbRawData = _iGetDbTableColumnInfo.GetTableColumnInformation();

            // Exclude the tables from the data that should be filtered
            var filteredData = _iDataDictionaryExclusionRules.FilterTablesMeetingRuleCriteria(
                _iDataDictionaryExclusionRules.GetRules(),
                dbRawData
            );

            // Generate the objects for use in the generation of the report
            var transformedData = _iDataDictionaryObjectCreator.TransformRawDataIntoFormattedObjects(filteredData);

            // Generate the report
            var results = _iDataDictionaryReportGenerator.GenerateReport(transformedData);
            _iDataDictionaryReportGenerator.SaveReport(results, filename);
        }
        #endregion Public methods
    }
}
