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
    public class DataDictionaryCreateCloxedXMLReport : IDataDictionaryReportGenerator
    {

        #region private
        private List<TableInfo> _data;
        private string _fileName;
        #endregion private

        #region Public methods
        /// <summary>
        /// Generate the report
        /// </summary>
        /// <param name="dataDictionaryData"></param>
        /// <param name="fileName"></param>
        public void GenerateReport(IEnumerable<TableInfo> dataDictionaryData, string fileName)
        {
            if (dataDictionaryData == null)
                throw new ArgumentNullException(nameof(dataDictionaryData));
            if (dataDictionaryData.Count() == 0)
                throw new ArgumentException(nameof(dataDictionaryData));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            _data = dataDictionaryData.ToList();
            _fileName = fileName;

            var workbook = CreateWorkbook();
            workbook = CreateWorkbookContents(workbook);
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
            return range;
        }
        #endregion Report Helpers

        #region Private methods
        /// <summary>
        /// Create the workbook
        /// </summary>
        /// <returns></returns>
        private XLWorkbook CreateWorkbook()
        {
            return new XLWorkbook();
        }

        /// <summary>
        /// Create the workbook contents
        /// </summary>
        /// <param name="workbook">The workbook to use</param>
        /// <returns>The workbook</returns>
        private XLWorkbook CreateWorkbookContents(XLWorkbook workbook)
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

            // Table Content
            CreateTableNameRow(ref worksheet, ref table, ref currentRow, lastColumn);
            CreateTableDescriptionRow(ref worksheet, ref table, ref currentRow, lastColumn);
            // Table description

            // Column Content
            // Column Table heading
            // Column table columns
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

            currentRow++;
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

            currentRow++;
            currentRow++;
        }

        /// <summary>
        /// Row for table name
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="currentRow"></param>
        /// <param name="lastColumn"></param>
        private void CreateTableNameRow(ref IXLWorksheet worksheet, ref TableInfo table, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);

            row.Cell(1, 1).Value = "Table Name:";
            row.Cell(1, 1).Style.Font.Bold = true;

            row.Cell(1, 2).Value = table.TableName;
            
            currentRow++;
        }

        /// <summary>
        /// Row for table description
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="currentRow"></param>
        /// <param name="lastColumn"></param>
        private void CreateTableDescriptionRow(ref IXLWorksheet worksheet, ref TableInfo table, ref int currentRow, int lastColumn)
        {
            IXLRange row = CreateRow(ref worksheet, ref currentRow, lastColumn);

            row.Cell(1, 1).Value = "Table Description:";
            row.Cell(1, 1).Style.Font.Bold = true;

            row.Cell(1, 2).Value = table.TableDescription;

            currentRow++;
        }
        #endregion Private methods
    }
}
