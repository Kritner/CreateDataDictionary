using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Used to generate a sheet in the excel workbook that lists the missing descriptions
    /// </summary>
    public class MissingDescriptionsSheetCreator
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
        /// <param name="workbook"></param>
        /// <param name="tables"></param>
        public void CreateSheetInWorkbook(ref XLWorkbook workbook, List<TableInfo> tables)
        {
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
        /// <param name="currentRow"></param>
        /// <param name="lastColumn"></param>
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
        /// Apply formatting to the worksheet
        /// </summary>
        /// <param name="worksheet"></param>
        private void FormatWorksheet(ref IXLWorksheet worksheet)
        {
            worksheet.ColumnsUsed().AdjustToContents();
        }
        #endregion Report Helpers

        #region Private methods
        /// <summary>
        /// Create rows for tables that have tables or columns with missing descriptions
        /// </summary>
        /// <param name="sheet"></param>
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
        /// <param name="sheet"></param>
        /// <param name="currentRow"></param>
        private void CreateTableHeader(ref IXLWorksheet sheet, ref int currentRow)
        {
            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = "Table Name";
            row.Cell(1, currentColumn++).Value = "Column Name";
            row.Cell(1, currentColumn++).Value = "Description";
        }

        /// <summary>
        /// Create rows for tables that have missing table or column descriptions
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="currentRow"></param>
        /// <param name="table"></param>
        private void CreateTableRowsForMissingDescriptions(ref IXLWorksheet sheet, ref int currentRow, TableInfo table)
        {
            bool emptyTableDescription = table.TableDescription == string.Empty;
            bool emptyColumnsDescriptions = table.TableColumns.Any(a => a.ColumnDescription == string.Empty);

            if (!emptyTableDescription && !emptyColumnsDescriptions)
                return;

            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = table.TableName;
            row.Cell(1, currentColumn++).Value = string.Empty;
            row.Cell(1, currentColumn++).Value = table.TableDescription;

            foreach (TableColumnInfo column in table.TableColumns)
                CreateTableRowsForMissingColumnDescriptions(ref sheet, ref currentRow, column);

            currentRow++;
        }

        /// <summary>
        /// Create rows for missing column descriptions
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="currentRow"></param>
        /// <param name="column"></param>
        private void CreateTableRowsForMissingColumnDescriptions(ref IXLWorksheet sheet, ref int currentRow, TableColumnInfo column)
        {
            if (!string.IsNullOrEmpty(column.ColumnDescription))
                return;

            IXLRange row = CreateRow(ref sheet, ref currentRow);
            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = column.Table.TableName;
            row.Cell(1, currentColumn++).Value = column.ColumnName;
            row.Cell(1, currentColumn++).Value = column.ColumnDescription;
        }
        #endregion Private methods

    }
}
