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
    /// Generate the DataDictionary in closedXml
    /// </summary>
    public class DataDictionaryCreateClosedXMLReport : IDataDictionaryReportGenerator
    {
        
        #region private
        private List<TableInfo> _data;
        private XLColor _headingColor = XLColor.FromArgb(153, 204, 255);
        private readonly IMissingTableDescriptionsSheetCreator _iMissingDescriptionsSheetCreator;
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iMissingDescriptionsSheetCreator">The IMissingDescriptionsSheetCreator implementation</param>
        public DataDictionaryCreateClosedXMLReport(IMissingTableDescriptionsSheetCreator iMissingDescriptionsSheetCreator)
        {
            _iMissingDescriptionsSheetCreator = iMissingDescriptionsSheetCreator;
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Generate the report
        /// </summary>
        /// <param name="dataDictionaryData">The data to use for the creation of the report</param>
        /// <returns>The report</returns>
        public XLWorkbook GenerateReport(IEnumerable<TableInfo> dataDictionaryData)
        {
            if (dataDictionaryData == null)
                throw new ArgumentNullException(nameof(dataDictionaryData));
            if (dataDictionaryData.Count() == 0)
                throw new ArgumentException(nameof(dataDictionaryData));

            _data = dataDictionaryData.ToList();

            XLWorkbook workbook = CreateWorkbook();

            // Generate the missing descriptions sheet within the workbook, if an implementation is provided
            if (_iMissingDescriptionsSheetCreator != null)
                _iMissingDescriptionsSheetCreator.CreateSheetInWorkbook(ref workbook, _data);

            workbook = CreateWorksheetContents(workbook);

            return workbook;
        }

        /// <summary>
        /// Save the report
        /// </summary>
        /// <param name="workbook">The workbook to save</param>
        /// <param name="fileName">The filename to save</param>
        public void SaveReport(XLWorkbook workbook, string fileName)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            workbook.SaveAs(fileName);
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
        private IXLRange CreateRow(ref IXLWorksheet worksheet, ref int currentRow, int lastColumn)
        {
            string rangeBegin = XLHelper.GetColumnLetterFromNumber(1) + currentRow;
            string rangeEnd = XLHelper.GetColumnLetterFromNumber(lastColumn) + currentRow;
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
            worksheet.Column(2).Width = 30;
            worksheet.Column(2).Style.Alignment.WrapText = true;
        }

        /// <summary>
        /// Apply shading to every other data row
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="startTableDataRow">The table start row</param>
        /// <param name="endTableDataRow">The table end row</param>
        /// <param name="lastColumn">The last column</param>
        private void ApplyShadingEveryOtherRow(ref IXLWorksheet worksheet, int startTableDataRow, int endTableDataRow, int lastColumn)
        {
            string rangeBegin = XLHelper.GetColumnLetterFromNumber(1) + startTableDataRow;
            string rangeEnd = XLHelper.GetColumnLetterFromNumber(lastColumn) + endTableDataRow;
            IXLRange range = worksheet.Range(rangeBegin, rangeEnd);
            range.AddConditionalFormat().WhenIsTrue("=mod(row(),2)=0").Fill.SetBackgroundColor(XLColor.LightGray);
        }
        #endregion Report Helpers

        #region Protected methods
        /// <summary>
        /// Create the workbook
        /// </summary>
        /// <returns></returns>
        protected virtual XLWorkbook CreateWorkbook()
        {
            return new XLWorkbook();
        }
        #endregion Protected methods

        #region Private methods
        /// <summary>
        /// Create the workbook contents
        /// </summary>
        /// <param name="workbook">The workbook to use</param>
        /// <returns>The workbook</returns>
        private XLWorkbook CreateWorksheetContents(XLWorkbook workbook)
        {
            _data.ForEach(table => CreateSheetForTable(ref workbook, table));

            return workbook;
        }

        /// <summary>
        /// Create a sheet for the table
        /// </summary>
        /// <param name="workbook">The workbook</param>
        /// <param name="table">The table to create data for</param>
        private void CreateSheetForTable(ref XLWorkbook workbook, TableInfo table)
        {
            IXLWorksheet worksheet = workbook.AddWorksheet(table.TableName);

            int currentRow = 1;
            int lastColumn = 7;

            CreateHeading(ref worksheet, ref currentRow, lastColumn);
            CreateSubHeading(ref worksheet, ref currentRow, lastColumn);

            currentRow++;

            // Table Content
            CreateTableNameRow(ref worksheet, ref table, ref currentRow, lastColumn);
            CreateTableDescriptionRow(ref worksheet, ref table, ref currentRow, lastColumn);
            CreateTableLastModifiedRow(ref worksheet, ref table, ref currentRow, lastColumn);

            currentRow++;

            // Column Content
            CreateTableColumnsHeadingRow(ref worksheet, ref currentRow, lastColumn);
            CreateTableColumnsData(ref worksheet, ref table, ref currentRow, lastColumn);

            FormatWorksheet(ref worksheet);
        }

        /// <summary>
        /// Create the report heading
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateHeading(ref IXLWorksheet worksheet, ref int currentRow, int lastColumn)
        {
            IXLRange headingRow = CreateRow(ref worksheet, ref currentRow, lastColumn);
            headingRow.Style.Font.Bold = true;
            headingRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.CenterContinuous;

            headingRow.FirstCell().Value = "Data Dictionary";
        }

        /// <summary>
        /// Create the report sub - heading
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateSubHeading(ref IXLWorksheet worksheet, ref int currentRow, int lastColumn)
        {
            IXLRange subHeadingRow = CreateRow(ref worksheet, ref currentRow, lastColumn);
            subHeadingRow.Style.Font.Bold = true;
            subHeadingRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.CenterContinuous;

            subHeadingRow.FirstCell().Value = string.Format("Generated on {0}", DateTime.Now);
        }

        /// <summary>
        /// Row for table name
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="table">The table being printed</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateTableNameRow(ref IXLWorksheet worksheet, ref TableInfo table, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);

            row.Cell(1, 1).Value = "Table Name:";
            row.Cell(1, 1).Style.Font.Bold = true;
            row.Cell(1, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            row.Cell(1, 1).Style.Fill.SetBackgroundColor(_headingColor);
            
            row.Cell(1, 2).Value = table.TableName;
        }

        /// <summary>
        /// Row for table description
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="table">The table being printed</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateTableDescriptionRow(ref IXLWorksheet worksheet, ref TableInfo table, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);

            row.Cell(1, 1).Value = "Table Description:";
            row.Cell(1, 1).Style.Font.Bold = true;
            row.Cell(1, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            row.Cell(1, 1).Style.Fill.SetBackgroundColor(_headingColor);

            row.Cell(1, 2).Value = table.TableDescription;
        }
        
        /// <summary>
        /// Row for table last modified row
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="table"></param>
        /// <param name="currentRow"></param>
        /// <param name="lastColumn"></param>
        private void CreateTableLastModifiedRow(ref IXLWorksheet worksheet, ref TableInfo table, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);

            row.Cell(1, 1).Value = "Table Last Modified:";
            row.Cell(1, 1).Style.Font.Bold = true;
            row.Cell(1, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            row.Cell(1, 1).Style.Fill.SetBackgroundColor(_headingColor);

            row.Cell(1, 2).Value = table.LastModified;
            row.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }

        /// <summary>
        /// Create heading row for columns table
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateTableColumnsHeadingRow(ref IXLWorksheet worksheet, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);
            row.Style.Font.Bold = true;
            row.Style.Alignment.WrapText = true;

            row.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            row.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            row.Style.Fill.SetBackgroundColor(_headingColor);

            int column = 1;
            row.Cell(1, column++).Value = "Column Name";
            row.Cell(1, column++).Value = "Description";
            row.Cell(1, column++).Value = "Type";
            row.Cell(1, column++).Value = "Additional Type Information";
            row.Cell(1, column++).Value = "Default Value";
            row.Cell(1, column++).Value = "Allows Nulls?";
            row.Cell(1, column++).Value = "Part of Key?";
        }

        /// <summary>
        /// Create Columns table data
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="table">The current DB table</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateTableColumnsData(ref IXLWorksheet worksheet, ref TableInfo table, ref int currentRow, int lastColumn)
        {
            int startTableDataRow = currentRow;

            foreach (TableColumnInfo column in table.TableColumns)
                CreateTableColumnRow(ref worksheet, column, ref currentRow, lastColumn);

            int endTableDataRow = currentRow-1;

            ApplyShadingEveryOtherRow(ref worksheet, startTableDataRow, endTableDataRow, lastColumn);
        }

        /// <summary>
        /// Create column row
        /// </summary>
        /// <param name="worksheet">The worksheet</param>
        /// <param name="table">The current DB table column</param>
        /// <param name="currentRow">The current row</param>
        /// <param name="lastColumn">The last column</param>
        private void CreateTableColumnRow(ref IXLWorksheet worksheet, TableColumnInfo column, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);

            row.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            row.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            int currentColumn = 1;
            row.Cell(1, currentColumn++).Value = column.ColumnName;
            row.Cell(1, currentColumn++).Value = column.ColumnDescription;
            row.Cell(1, currentColumn++).Value = column.ColumnDataType;
            row.Cell(1, currentColumn++).Value = column.AdditionalInfoFormatted;
            row.Cell(1, currentColumn++).Value = column.DefaultValue;
            row.Cell(1, currentColumn++).Value = column.AllowsNulls;
            row.Cell(1, currentColumn++).Value = column.PartOfKeyFormatted;
        }
        #endregion Private methods
    }
}
