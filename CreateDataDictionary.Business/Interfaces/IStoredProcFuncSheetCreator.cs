using System.Collections.Generic;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Interface for creating an excel sheet within a workbook related to stored procedure and functions
    /// </summary>
    public interface IStoredProcFuncSheetCreator
    {
        /// <summary>
        /// Creates a sheet within the provided workbook
        /// </summary>
        /// <param name="workbook">The workbook to create the sheet within</param>
        /// <param name="storedProcFunc">The tables to parse for missing information</param>
        void CreateSheetInWorkbook(ref XLWorkbook workbook, IEnumerable<StoredProcFuncInfo> storedProcFunc);
    }
}