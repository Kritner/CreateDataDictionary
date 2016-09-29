using System.Collections.Generic;
using ClosedXML.Excel;
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
        /// <param name="dataDictionaryTableData">The dictionary data table to generate the report based on.</param>
        /// <param name="dataDictionaryStoredProcFuncData">The dictionary stored procedure and function data.</param>
        XLWorkbook GenerateReport(IEnumerable<TableInfo> dataDictionaryTableData, IEnumerable<StoredProcFuncInfo> dataDictionaryStoredProcFuncData);
        
        /// <summary>
        /// Save the XLWorkbook
        /// </summary>
        /// <param name="workbook">The workbook to save</param>
        /// <param name="fileName">The fileName to save</param>
        void SaveReport(XLWorkbook workbook, string fileName);
    }
}