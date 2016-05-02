using System.Collections.Generic;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Interface for creating a sheet for missing descriptions within the database tables and column
    /// </summary>
    public interface IMissingTableDescriptionsSheetCreator
    {
        /// <summary>
        /// Creates a sheet within the provided workbook
        /// </summary>
        /// <param name="workbook">The workbook to create the sheet within</param>
        /// <param name="tables">The tables to parse for missing information</param>
        void CreateSheetInWorkbook(ref XLWorkbook workbook, IEnumerable<TableInfo> tables);
    }
}