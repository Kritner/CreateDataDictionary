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

        #region private
        MissingDescriptionsSheetCreator _biz;
        XLWorkbook _workbook;
        List<TableInfo> _data;
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

            DataDictionaryObjectCreatorService service = new DataDictionaryObjectCreatorService();

            _data = service
                .TransformRawDataIntoFormattedObjects(DataHelpers.GetSampleTableColumnInfoRaw())
                .ToList();
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
        #endregion Public methods/tests

    }
}
