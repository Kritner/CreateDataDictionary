using System.Collections.Generic;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Used to generate the report
    /// </summary>
    public interface IDataDictionaryReportGenerator
    {
        /// <summary>
        /// Generate the report
        /// </summary>
        /// <param name="dataDictionaryData">The dictionary data to generate the report based on.</param>
        void GenerateReport(IEnumerable<TableInfo> dataDictionaryData, string fileName);
    }
}