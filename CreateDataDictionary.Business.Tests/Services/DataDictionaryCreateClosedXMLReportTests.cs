using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for DataDictionaryCreateClosedXMLReportTests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DataDictionaryCreateClosedXMLReportTests
    {
        #region const
        const string _TABLE_NAME_CELL = "B4";
        const string _TABLE_DESCRIPTION_CELL = "B5";
        const string _TABLE_DATE_CELL = "B6";

        const int _COLUMN_START_ROW = 9;
        const string _COLUMN_NAME = "A";
        const string _COLUMN_DESCRIPTION = "B";
        const string _COLUMN_TYPE = "C";
        const string _COLUMN_ADDITIONAL = "D";
        const string _COLUMN_DEFAULT = "E";
        const string _COLUMN_ALLOWS_NULL = "F";
        const string _COLUMN_KEY_SEQ = "G";
        #endregion const

        #region private
        List<TableInfo> _testData;
        DataDictionaryCreateClosedXMLReport _biz;
        XLWorkbook _testWorkbook;
        Mock<IMissingDescriptionsSheetCreator> _mockIMissingDescriptionsSheetCreator;
        #region test class
        private class TestableDataDictionaryCreateClosedXMLReport : DataDictionaryCreateClosedXMLReport
        {
            private XLWorkbook _workbook;

            public TestableDataDictionaryCreateClosedXMLReport(IMissingDescriptionsSheetCreator iMissingDescriptionsSheetCreator) : 
                base(iMissingDescriptionsSheetCreator)
            {
            }

            public void SetWorkBook(XLWorkbook workbook)
            {
                _workbook = workbook;
            }

            protected override XLWorkbook CreateWorkbook()
            {
                return _workbook;
            }
        }
        #endregion test class
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            DataDictionaryObjectCreatorService service = new DataDictionaryObjectCreatorService();
            _testData = service.TransformRawDataIntoFormattedObjects(DataHelpers.GetSampleTableColumnInfoRaw()).ToList();
            _mockIMissingDescriptionsSheetCreator = new Mock<IMissingDescriptionsSheetCreator>();
            _biz = new DataDictionaryCreateClosedXMLReport(_mockIMissingDescriptionsSheetCreator.Object);
            _testWorkbook = _biz.GenerateReport(_testData);
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException is thrown when DataDictionaryData is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreateClosedXMLReport_GenerateReport_ArgumentNullExceptionThrownDataDictionaryDataNull()
        {
            // Arrange / Act / Assert
            var results = _biz.GenerateReport(null);
        }

        /// <summary>
        /// ArgumentException is thrown when DataDictionaryData has a count of 0
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DataDictionaryCreateClosedXMLReport_GenerateReport_ArgumentExceptionThrownDataDictionaryDataZeroRecords()
        {
            // Arrange / Act / Assert
            var results = _biz.GenerateReport(new List<TableInfo>());
        }

        /// <summary>
        /// Tests that there should be one sheet for every Table in the data source.
        /// Check table and column info is written to expected cells within the document.
        /// </summary>
        [TestMethod]
        public void DataDictionaryCreateClosedXMLReport_GenerateReport_SheetForEveryTable()
        {
            // Assert
            foreach(TableInfo table in _testData)
            {
                IXLWorksheet worksheet;
                Assert.IsTrue(_testWorkbook.TryGetWorksheet(table.TableName, out worksheet));

                // Test dynamic layout
                Assert.AreEqual(table.TableName, worksheet.Cell(_TABLE_NAME_CELL).Value.ToString(), string.Format("{0} {1}", table, nameof(table.TableName)));
                Assert.AreEqual(table.TableDescription, worksheet.Cell(_TABLE_DESCRIPTION_CELL).Value.ToString(), string.Format("{0} {1}", table, nameof(table.TableDescription)));
                Assert.AreEqual(table.LastModified.ToString(), worksheet.Cell(_TABLE_DATE_CELL).Value.ToString(), string.Format("{0} {1}", table, nameof(table.LastModified)));

                int currentRow = _COLUMN_START_ROW;
                foreach (TableColumnInfo column in table.TableColumns)
                {
                    Assert.AreEqual(column.ColumnName, worksheet.Cell(_COLUMN_NAME + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.ColumnName)));
                    Assert.AreEqual(column.ColumnDescription, worksheet.Cell(_COLUMN_DESCRIPTION + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.ColumnDescription)));
                    Assert.AreEqual(column.ColumnDataType, worksheet.Cell(_COLUMN_TYPE + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.ColumnDataType)));
                    Assert.AreEqual(column.AdditionalInfoFormatted, worksheet.Cell(_COLUMN_ADDITIONAL + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.AdditionalInfoFormatted)));
                    Assert.AreEqual(column.DefaultValue, worksheet.Cell(_COLUMN_DEFAULT + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.DefaultValue)));
                    Assert.AreEqual(column.AllowsNulls, worksheet.Cell(_COLUMN_ALLOWS_NULL + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.AllowsNulls)));
                    Assert.AreEqual(column.PartOfKeyFormatted, worksheet.Cell(_COLUMN_KEY_SEQ + currentRow).Value, string.Format("{0} {1} {2}", table, column, nameof(column.PartOfKeyFormatted)));

                    currentRow++;
                }
            }
        }

        /// <summary>
        /// ArgumentNullException is thrown with a null workbook
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreateClosedXMLReport_SaveReport_ArgumentNullExceptionThrownWithNullWorkbook()
        {
            // Arrange / Act / Assert
            _biz.SaveReport(null, "test");
        }

        /// <summary>
        /// ArgumentNullException is thrown with a null filename
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreateClosedXMLReport_SaveReport_ArgumentNullExceptionThrownWithNullFileName()
        {
            // Arrange / Act / Assert
            _biz.SaveReport(_testWorkbook, null);
        }

        /// <summary>
        /// Save report creates a file at the specified filename and path
        /// </summary>
        [TestMethod]
        public void DataDictionaryCreateClosedXMLReport_SaveReport_Success()
        {
            // Arrange
            string fileName = "unitTestDataDictionary.xlsx";

            if (File.Exists(fileName))
                Assert.Fail(string.Format("{0} already exists.", fileName));

            // Act
            _biz.SaveReport(_testWorkbook, fileName);

            // Assert
            Assert.IsTrue(File.Exists(fileName), "File not found");

            // Cleanup
            File.Delete(fileName);
        }

        /// <summary>
        /// Ensure when <see cref="IMissingDescriptionsSheetCreator"/> not provided, CreateSheetInWorkbook is not called
        /// </summary>
        [TestMethod]
        public void DataDictionaryCreateClosedXMLReport_GenerateReport_WithNoIIMissingDescriptionsInterfaceNotInvoked()
        {
            // Arrange
            Mock<IMissingDescriptionsSheetCreator> mock = new Mock<IMissingDescriptionsSheetCreator>();
            TestableDataDictionaryCreateClosedXMLReport biz = new TestableDataDictionaryCreateClosedXMLReport(null);
            XLWorkbook wb = new XLWorkbook();
            biz.SetWorkBook(wb);

            // Act
            biz.GenerateReport(_testData);

            // Assert
            mock.Verify(v => v.CreateSheetInWorkbook(ref wb, It.IsAny<List<TableInfo>>()), Times.Never, "CreateSheetInWorkbook");
        }

        /// <summary>
        /// Ensure when <see cref="IMissingDescriptionsSheetCreator"/> provided, CreateSheetInWorkbook is called
        /// </summary>
        [TestMethod]
        public void DataDictionaryCreateClosedXMLReport_GenerateReport_WithIIMissingDescriptionsInterfaceInvoked()
        {
            // Arrange
            Mock<IMissingDescriptionsSheetCreator> mock = new Mock<IMissingDescriptionsSheetCreator>();
            TestableDataDictionaryCreateClosedXMLReport biz = new TestableDataDictionaryCreateClosedXMLReport(mock.Object);
            XLWorkbook wb = new XLWorkbook();
            biz.SetWorkBook(wb);

            // Act
            biz.GenerateReport(_testData);

            // Assert
            mock.Verify(v => v.CreateSheetInWorkbook(ref wb, It.IsAny<List<TableInfo>>()), Times.Once, "CreateSheetInWorkbook");
        }
        #endregion Public methods/tests
    }
}
