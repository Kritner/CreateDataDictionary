using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Used to generate a sheet in the excel workbook that lists the missing descriptions
    /// </summary>
    public class MissingDescriptionsSheetCreator : IMissingTableDescriptionsSheetCreator
    {

        #region const
        const int _NUMBER_OF_COLUMNS_WIDE = 5;
        #endregion const

        #region private
        List<TableInfo> _data;
        #endregion private

        #region Public methods
        /// <summary>
        /// Create a worksheet that lists tables/columns that are missing descriptions
        /// </summary>
        /// <param name="workbook">The workbook in which to write</param>
        /// <param name="tables">The table data</param>
        public void CreateSheetInWorkbook(ref XLWorkbook workbook, List<TableInfo> tables)
        {
            if (tables == null)
                throw new ArgumentNullException(nameof(tables));
            if (tables.Count == 0)
                throw new ArgumentException(nameof(tables));

            bool anyEmptyTableDescriptions = tables.Any(a => a.TableDescription == string.Empty);
            bool anyEmptyColumnsDescriptions = tables.Any(a => a.TableColumns.Any(a2 => a2.ColumnDescription == string.Empty));

            // All tables and columns contain descriptions
            if (!anyEmptyColumnsDescriptions && !anyEmptyColumnsDescriptions)
                return;

            _data = tables;

            IXLWorksheet sheet = workbook.AddWorksheet("MissingDescriptions");

            CreateTableRowsForMissingDescriptions(ref sheet);
            FormatWorksheet(ref sheet);
        }
        #endregion Public methods

        #region Report Helpers
        /// <summary>
        /// Create a new row
        /// </summary>
        /// <param name="worksheet">The worksheet to create the </param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        /// <returns></returns>
        private IXLRange CreateRow(ref IXLWorksheet worksheet, ref int currentRow)
        {
            string rangeBegin = XLHelper.GetColumnLetterFromNumber(1) + currentRow;
            string rangeEnd = XLHelper.GetColumnLetterFromNumber(_NUMBER_OF_COLUMNS_WIDE) + currentRow;
            IXLRange range = worksheet.Range(rangeBegin, rangeEnd);

            currentRow++;
            return range;
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

        /// <summary>
        /// Apply formatting to the worksheet
        /// </summary>
        /// <param name="worksheet">The worksheet to format</param>
        private void FormatWorksheet(ref IXLWorksheet worksheet)
        {
            worksheet.ColumnsUsed().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);
            worksheet.Rows().Height = 30;
            worksheet.Column(3).Width = 40;
        }
        #endregion Report Helpers

        #region Private methods
        /// <summary>
        /// Create rows for tables that have tables or columns with missing descriptions
        /// </summary>
        /// <param name="sheet">The worksheet to format</param>
        private void CreateTableRowsForMissingDescriptions(ref IXLWorksheet sheet)
        {
            int currentRow = 1;

            CreateTableHeader(ref sheet, ref currentRow);

            foreach (TableInfo table in _data)
            {
                CreateTableRowsForMissingDescriptions(ref sheet, ref currentRow, table);
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
            row.Cell(1, currentColumn++).Value = "Table Name";
            row.Cell(1, currentColumn++).Value = "Column Name";
            row.Cell(1, currentColumn++).Value = "Description";
            row.Cell(1, currentColumn++).Value = string.Empty;
            row.Cell(1, currentColumn++).Value = "SQL note find/replace double quote, added automatically";
        }

        /// <summary>
        /// Create rows for tables that have missing table or column descriptions
        /// </summary>
        /// <param name="sheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="table">The table to print</param>
        private void CreateTableRowsForMissingDescriptions(ref IXLWorksheet sheet, ref int currentRow, TableInfo table)
        {
            bool emptyTableDescription = table.TableDescription == string.Empty;
            bool emptyColumnsDescriptions = table.TableColumns.Any(a => a.ColumnDescription == string.Empty);

            if (!emptyTableDescription && !emptyColumnsDescriptions)
                return;

            int startSectionRow = currentRow;

            if (emptyTableDescription)
            {
                CreateTableRowForMissingDescription(ref sheet, ref currentRow, table);
            }

            foreach (TableColumnInfo column in table.TableColumns)
                CreateTableRowsForMissingColumnDescriptions(ref sheet, ref currentRow, column);

            int endSectionRow = currentRow - 1;

            ApplyFormattingToSection(ref sheet, startSectionRow, endSectionRow);

            currentRow++;
        }

        /// <summary>
        /// Create the Row describing for a table missing its description
        /// </summary>
        /// <param name="sheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="table">The table to print</param>
        private void CreateTableRowForMissingDescription(ref IXLWorksheet sheet, ref int currentRow, TableInfo table)
        {
            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = table.TableName;
            row.Cell(1, currentColumn++).Value = string.Empty;
            row.Cell(1, currentColumn++).Value = table.TableDescription;
            row.Cell(1, currentColumn++).Value = string.Empty;

            string replacementFormula = "RC[1]";
            replacementFormula = @"SUBSTITUTE(" + replacementFormula + @", ""{0}"", RC[-4])"; // Replaces {0} with table name
            replacementFormula = "SUBSTITUTE(" + replacementFormula + @", ""{1}"", SUBSTITUTE(RC[-2], ""'"", ""''""))"; // Replaces {1} with description, also replaces "'" in description with "''" for SQL single quote escape

            row.Cell(1, currentColumn++).FormulaR1C1 = @"=IF(RC[-2] = """", ""Enter a Table description (Column C)"", " + replacementFormula + ")";

            int sqlScriptColumn = currentColumn++;
            row.Cell(1, sqlScriptColumn).Value = Common.SqlQuery._SCRIPT_TEMPLATE_FOR_TABLE;
            row.Cell(1, sqlScriptColumn).Style.Font.SetFontColor(XLColor.White);
        }

        /// <summary>
        /// Create rows for missing column descriptions
        /// </summary>
        /// <param name="sheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="column">The column to print</param>
        private void CreateTableRowsForMissingColumnDescriptions(ref IXLWorksheet sheet, ref int currentRow, TableColumnInfo column)
        {
            if (!string.IsNullOrEmpty(column.ColumnDescription))
                return;

            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = column.Table.TableName;
            row.Cell(1, currentColumn++).Value = column.ColumnName;
            row.Cell(1, currentColumn++).Value = column.ColumnDescription;
            row.Cell(1, currentColumn++).Value = string.Empty;

            string replacementFormula = "RC[1]";
            replacementFormula = @"SUBSTITUTE(" + replacementFormula + @", ""{0}"", RC[-4])"; // Replaces {0} with table name
            replacementFormula = @"SUBSTITUTE(" + replacementFormula + @", ""{1}"", RC[-3])"; // Replaces {1} with columns name
            replacementFormula = "SUBSTITUTE(" + replacementFormula + @", ""{2}"", SUBSTITUTE(RC[-2], ""'"", ""''""))"; // Replaces {2} with description, also replaces "'" in description with "''" for SQL single quote escape

            row.Cell(1, currentColumn++).FormulaR1C1 = @"=IF(RC[-2] = """", ""Enter a Column description (Column C)"", " + replacementFormula + ")";

            int sqlScriptColumn = currentColumn++;
            row.Cell(1, sqlScriptColumn).Value = Common.SqlQuery._SCRIPT_TEMPLATE_FOR_TABLE_COLUMN;
            row.Cell(1, sqlScriptColumn).Style.Font.SetFontColor(XLColor.White);
        }
        #endregion Private methods

    }
}
