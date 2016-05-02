using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;

namespace CreateDataDictionary.Business.Services
{
    public class StoredProcFuncSheetCreator : IStoredProcFuncSheetCreator
    {
        #region const
        const int _NUMBER_OF_COLUMNS_WIDE = 7;
        #endregion const

        #region private
        private List<StoredProcFuncInfo> _data;
        #endregion private

        /// <summary>
        /// Create sheet describing Stored Procedures and functions
        /// </summary>
        /// <param name="workbook">The workbook to create the sheet in</param>
        /// <param name="storedProcFuncs">The stored procedures and functions to document</param>
        public void CreateSheetInWorkbook(ref XLWorkbook workbook, IEnumerable<StoredProcFuncInfo> storedProcFuncs)
        {
            if (storedProcFuncs == null)
                throw new ArgumentNullException(nameof(storedProcFuncs));
            if (storedProcFuncs.Count() == 0)
                throw new ArgumentException(nameof(storedProcFuncs));

            _data = storedProcFuncs.ToList();

            IXLWorksheet sheet = workbook.AddWorksheet("DbObjects");

            CreateTableRowsForObjects(ref sheet);
            FormatWorksheet(ref sheet);
        }

        #region private helpers
        /// <summary>
        /// Create a row
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="currentRow">The current row</param>
        /// <returns>A IXLRange</returns>
        private IXLRange CreateRow(ref IXLWorksheet sheet, ref int currentRow)
        {
            string rangeBegin = XLHelper.GetColumnLetterFromNumber(1) + currentRow;
            string rangeEnd = XLHelper.GetColumnLetterFromNumber(_NUMBER_OF_COLUMNS_WIDE) + currentRow;
            IXLRange range = sheet.Range(rangeBegin, rangeEnd);

            currentRow++;
            return range;
        }

        /// <summary>
        /// Apply formatting to the worksheet
        /// </summary>
        /// <param name="worksheet">The worksheet to format</param>
        private void FormatWorksheet(ref IXLWorksheet worksheet)
        {
            worksheet.ColumnsUsed().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);
        }

        /// <summary>
        /// Apply shading to the section
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="startSectionRow">The start section row</param>
        /// <param name="endSectionRow">The end section row</param>
        private void ApplyFormattingToSection(ref IXLWorksheet worksheet, int startSectionRow, int endSectionRow)
        {
            string rangeBegin = XLHelper.GetColumnLetterFromNumber(1) + startSectionRow;
            string rangeEnd = XLHelper.GetColumnLetterFromNumber(_NUMBER_OF_COLUMNS_WIDE) + endSectionRow;
            IXLRange range = worksheet.Range(rangeBegin, rangeEnd);

            range.Style.Fill.SetBackgroundColor(XLColor.LightGray);
        }
        #endregion private helpers

        #region private methods
        /// <summary>
        /// Create rows for DB objects
        /// </summary>
        /// <param name="sheet">The worksheet to format</param>
        private void CreateTableRowsForObjects(ref IXLWorksheet sheet)
        {
            int currentRow = 1;

            CreateTableHeader(ref sheet, ref currentRow);

            foreach (StoredProcFuncInfo obj in _data)
            {
                CreateTableRowsForObject(ref sheet, ref currentRow, obj);
            }

        }

        /// <summary>
        /// Create the table header
        /// </summary>
        /// <param name="sheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        private void CreateTableHeader(ref IXLWorksheet sheet, ref int currentRow)
        {
            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = "Object Name";
            row.Cell(1, currentColumn++).Value = "Object Type";
            row.Cell(1, currentColumn++).Value = "Parameter Name";
            row.Cell(1, currentColumn++).Value = "Parameter sequence";
            row.Cell(1, currentColumn++).Value = "Parameter Type";
            row.Cell(1, currentColumn++).Value = "Parameter Max Length";
            row.Cell(1, currentColumn++).Value = "Out parameter?";
        }

        /// <summary>
        /// Create rows for db objects
        /// </summary>
        /// <param name="sheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="obj">The object to print</param>
        private void CreateTableRowsForObject(ref IXLWorksheet sheet, ref int currentRow, StoredProcFuncInfo obj)
        {
            int startSectionRow = currentRow;

            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = obj.ObjectName;
            row.Cell(1, currentColumn++).Value = obj.ObjectType;

            foreach (StoredProcFuncInfoParams param in obj.Parameters)
                CreateTableRowsForParameter(ref sheet, ref currentRow, param);

            int endSectionRow = currentRow - 1;

            ApplyFormattingToSection(ref sheet, startSectionRow, endSectionRow);

            currentRow++;
        }

        /// <summary>
        /// Create row for parameter
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="param">The parameter to print</param>
        private void CreateTableRowsForParameter(ref IXLWorksheet sheet, ref int currentRow, StoredProcFuncInfoParams param)
        {
            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = param.ParentObject.ObjectName;
            row.Cell(1, currentColumn++).Value = param.ParentObject.ObjectType;
            row.Cell(1, currentColumn++).Value = param.ParameterName;
            row.Cell(1, currentColumn++).Value = param.ParameterId;
            row.Cell(1, currentColumn++).Value = param.ParameterDataType;
            row.Cell(1, currentColumn++).Value = param.ParameterMaxLength;
            row.Cell(1, currentColumn++).Value = param.IsOutParameter;
        }
        #endregion private methods
    }
}