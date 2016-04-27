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
        private readonly IDataDictionaryTableDataProvider _iDataDictionaryTableDataProvider;
        private readonly IDataDictionaryReportGenerator _iDataDictionaryReportGenerator;
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iDataDictionaryTableDataProvider">The IDataDictionaryTableDataProvider dependency</param>
        /// <param name="iDataDictionaryReportGenerator">The IDataDictionaryReportGenerator dependency</param>
        public DataDictionaryCreationService(
            IDataDictionaryTableDataProvider iDataDictionaryTableDataProvider,
            IDataDictionaryReportGenerator iDataDictionaryReportGenerator
        )
        {
            if (iDataDictionaryTableDataProvider == null)
                throw new ArgumentNullException(nameof(iDataDictionaryTableDataProvider));
            if (iDataDictionaryReportGenerator == null)
                throw new ArgumentNullException(nameof(iDataDictionaryReportGenerator));

            _iDataDictionaryTableDataProvider = iDataDictionaryTableDataProvider;
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

            // Get table data
            var transformedData = _iDataDictionaryTableDataProvider.GetTableData();

            // Generate the report
            var results = _iDataDictionaryReportGenerator.GenerateReport(transformedData);
            _iDataDictionaryReportGenerator.SaveReport(results, filename);
        }
        #endregion Public methods
    }
}
