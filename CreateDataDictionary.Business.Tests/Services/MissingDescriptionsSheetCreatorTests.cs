using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Services
{
    /// <summary>
    /// Tests for MissingDescriptionsSheetCreator
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MissingDescriptionsSheetCreatorTests
    {
        #region const
        const string _TABLE_DESCRIPTION = "Enter a Table description (Column C)";
        const string _COLUMN_DESCRIPTION = "Enter a Column description (Column C)";
        #endregion const

        #region private
        private MissingDescriptionsSheetCreator _biz;
        private XLWorkbook _workbook;
        private List<TableInfo> _data;
        private List<TableInfo> _missingDescriptionsData;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _biz = new MissingDescriptionsSheetCreator();
            _workbook = new XLWorkbook();

            TableModelObjectCreatorService service = new TableModelObjectCreatorService();

            _data = service
                .TransformRawDataIntoFormattedObjects(DataHelpers.GetSampleTableColumnInfoRaw())
                .ToList();

            _missingDescriptionsData = new List<TableInfo>()
            {
                new TableInfo(
                    "Table",
                    string.Empty,
                    new DateTime(2000, 1, 1),
                    new List<TableColumnInfo>()
                    {
                        new TableColumnInfo(
                            "Column",
                            string.Empty,
                            "type",
                            1,
                            "default",
                            true,
                            1
                        )
                    })
            };
            _missingDescriptionsData.First().TableColumns.First().Table = _missingDescriptionsData.First();
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException is thrown when TableInfo is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_ArgumentNullExceptionNullTableInfo()
        {
            // Arrange / Act / Assert
            _biz.CreateSheetInWorkbook(ref _workbook, null);
        }

        /// <summary>
        /// ArgumentException is thrown when TableInfo has 0 records
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_ArgumentExceptionZeroCountTableInfo()
        {
            // Arrange / Act / Assert
            _biz.CreateSheetInWorkbook(ref _workbook, new List<TableInfo>());
        }

        /// <summary>
        /// A worksheet is not created in the workbook when all TableInfo and TableColumnInfo contains descriptions
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_WorksheetNotCreatedWhenNoDescriptionsMissing()
        {
            // Arrange
            _data = new List<TableInfo>()
            {
                new TableInfo(
                    "Table", 
                    "TableDescription", 
                    new DateTime(2000, 1, 1), 
                    new List<TableColumnInfo>()
                    {
                        new TableColumnInfo("Column", "Column description", "data type", 1, "default", true, 1)
                    }
                )
            };
            _data.First().TableColumns.First().Table = _data.First();

            // Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);

            // Assert
            Assert.IsTrue(_workbook.Worksheets.Count == 0);
        }

        /// <summary>
        /// A worksheet is created when descriptions are missing
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_WorksheetCreatedWhenDescriptionsMissing()
        {
            // Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);

            // Assert
            Assert.IsTrue(_workbook.Worksheets.Count == 1);
        }

        /// <summary>
        /// A table should not be present on the worksheet if there are no descriptions missing
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_TableNotPresentInSheetWhenNoMissingDescriptions()
        {
            // Arrange / Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);
            string tableWithoutMissingDescriptions = "SampleTableShouldNotBeRemoved";

            // There cannot be more missing descriptions than there are rows, 
            // so get maximum number of rows possible on worksheet * 2 + 1. ( * 2 to account for "spacer rows")
            int rawDataCount = (DataHelpers.GetSampleTableColumnInfoRaw().Count * 2) + 1;

            // Assert
            for (int rowIterator = 1; rowIterator < rawDataCount; rowIterator++)
            {
                // Table names are stored in the first column "A"
                Assert.IsTrue(_workbook.Worksheets.First().Cell(rowIterator, 1).Value.ToString() != tableWithoutMissingDescriptions);
            }
        }

        /// <summary>
        /// A table should be present on the worksheet if there is a missing table descriptions
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_TablePresentInSheetForMissingTableDescription()
        {
            // Arrange / Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);
            string tableWithMissingDescription = "TableWithDescNoDesc";

            // There cannot be more missing descriptions than there are rows, 
            // so get maximum number of rows possible on worksheet * 2 + 1. ( * 2 to account for "spacer rows")
            int rawDataCount = (DataHelpers.GetSampleTableColumnInfoRaw().Count * 2) + 1;
            int foundCount = 0;
            int expectedFoundCount = 1;

            for (int rowIterator = 1; rowIterator < rawDataCount; rowIterator++)
            {
                if (_workbook.Worksheets.First().Cell(rowIterator, 1).Value.ToString() == tableWithMissingDescription)
                    foundCount++;
            }

            // Assert
            Assert.AreEqual(expectedFoundCount, foundCount, string.Format("Found {0} instances of {1}, expected {2}", foundCount, tableWithMissingDescription, expectedFoundCount));
        }

        /// <summary>
        /// A table should be present on the worksheet if there is a missing column descriptions
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_TablePresentInSheetForMissingColumnDescription()
        {
            // Arrange / Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);
            string tableWithMissingColumnDescription = "TableWithDescNoColumnDesc";

            // There cannot be more missing descriptions than there are rows, 
            // so get maximum number of rows possible on worksheet * 2 + 1. ( * 2 to account for "spacer rows")
            int rawDataCount = (DataHelpers.GetSampleTableColumnInfoRaw().Count * 2) + 1;
            int foundCount = 0;
            int expectedFoundCount = 1;

            for (int rowIterator = 1; rowIterator < rawDataCount; rowIterator++)
            {
                if (_workbook.Worksheets.First().Cell(rowIterator, 1).Value.ToString() == tableWithMissingColumnDescription)
                    foundCount++;
            }

            // Assert
            Assert.AreEqual(expectedFoundCount, foundCount, string.Format("Found {0} instances of {1}, expected {2}", foundCount, tableWithMissingColumnDescription, expectedFoundCount));
        }

        /// <summary>
        /// Tests data assumptions that the data contains two rows with reference to table "Table"
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_MissingDescriptionsCheckAssumptionsTwoInstances()
        {
            // Arrange / Act
            if (_missingDescriptionsData.First().TableName != "Table")
                Assert.Fail("Expected Table to exist within data");
            if (_missingDescriptionsData.Count != 1)
                Assert.Fail("Expected only one table to exist within data");

            string table = _missingDescriptionsData.First().TableName;

            _biz.CreateSheetInWorkbook(ref _workbook, _missingDescriptionsData);

            if (_workbook.Worksheets.Count != 1)
                Assert.Fail("Expected only one worksheet within workbook");

            int rawDataCount = 10;
            int foundCount = 0;
            int expectedFoundCount = 2;

            for (int rowIterator = 1; rowIterator < rawDataCount; rowIterator++)
            {
                if (_workbook.Worksheets.First().Cell(rowIterator, 1).Value.ToString() == table)
                    foundCount++;
            }

            // Assert
            Assert.AreEqual(expectedFoundCount, foundCount, string.Format("Found {0} instances of {1}, expected {2}", foundCount, table, expectedFoundCount));
        }

        /// <summary>
        /// Checks that the excel sheet states there is a missing table and a missing column description, and that the labels are set appropriately while that information is missing
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_CheckOneMissingTableOneMissingColumnDesc()
        {
            _biz.CreateSheetInWorkbook(ref _workbook, _missingDescriptionsData);

            int rawDataCount = 10;
            int foundCountTable = 0;
            int foundCountColumn = 0;
            int expectedFoundCountTable = 1;
            int expectedFoundCountColumn = 1;

            for (int rowIterator = 1; rowIterator < rawDataCount; rowIterator++)
            {
                if (_workbook.Worksheets.First().Cell(rowIterator, 5).Value.ToString() == _TABLE_DESCRIPTION)
                    foundCountTable++;
                if (_workbook.Worksheets.First().Cell(rowIterator, 5).Value.ToString() == _COLUMN_DESCRIPTION)
                    foundCountColumn++;
            }

            // Assert
            Assert.AreEqual(expectedFoundCountTable, foundCountTable, string.Format("Found {0} instances of {1}, expected {2}", foundCountTable, nameof(foundCountTable), expectedFoundCountTable));
            Assert.AreEqual(expectedFoundCountColumn, foundCountColumn, string.Format("Found {0} instances of {1}, expected {2}", foundCountColumn, nameof(foundCountColumn), expectedFoundCountColumn));
        }

        /// <summary>
        /// Modifying the blank description creates a SQL script that matches expectations
        /// </summary>
        [TestMethod]
        public void MissingDescriptionsSheetCreator_CreateSheetInWorkbook_ModifyBlankDescriptionCreatesSqlScript()
        {
            // Arrange / Act
            _biz.CreateSheetInWorkbook(ref _workbook, _missingDescriptionsData);
            string description = "This test's stuff";

            int maxRowCount = 10;
            // Update the "description" for rows
            for (int rowIterator = 1; rowIterator < maxRowCount; rowIterator++)
                _workbook.Worksheets.First().Cell(rowIterator, 3).Value = description;

            int expectedTable = 1;
            int expectedColumn = 1;
            int foundTable = 0;
            int foundColumn = 0;

            for (int rowIterator = 1; rowIterator < maxRowCount; rowIterator++)
            {
                // Table check
                if (_workbook.Worksheets.First().Cell(rowIterator, 5).Value.ToString() ==
                    string.Format(
                        Common.SqlQuery._SCRIPT_TEMPLATE_FOR_TABLE, 
                        _missingDescriptionsData.First().TableName, 
                        description.Replace("'", "''")))
                    foundTable++;

                // Column check
                if (_workbook.Worksheets.First().Cell(rowIterator, 5).Value.ToString() ==
                    string.Format(
                        Common.SqlQuery._SCRIPT_TEMPLATE_FOR_TABLE_COLUMN, 
                        _missingDescriptionsData.First().TableName, 
                        _missingDescriptionsData.First().TableColumns.First().ColumnName, 
                        description.Replace("'", "''")))
                    foundColumn++;
            }

            // Assert
            Assert.AreEqual(expectedTable, foundTable, "Expected {0}, found {1} in {2}", expectedTable, foundTable, nameof(expectedTable));
            Assert.AreEqual(expectedColumn, foundColumn, "Expected {0}, found {1} in {2}", expectedColumn, foundColumn, nameof(expectedColumn));
        }
        #endregion Public methods/tests
    }
}
