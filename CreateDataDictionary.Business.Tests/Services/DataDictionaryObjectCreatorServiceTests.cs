using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for DataDictionaryObjectCreatorService
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DataDictionaryObjectCreatorServiceTests
    {

        #region private
        List<TableColumnInfoRaw> _testData;
        DataDictionaryObjectCreatorService _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _testData = DataHelpers.GetSampleTableColumnInfoRaw();
            _biz = new DataDictionaryObjectCreatorService();
        }

        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException thrown when RawData is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryObjectCreatorService_TransformRawDataIntoFormattedObjects_ArgumentNullExceptionThrownWhenNullRawData()
        {
            // Arrange / Act / Assert
            var results = _biz.TransformRawDataIntoFormattedObjects(null);
        }

        /// <summary>
        /// When there is no data within rawData, ensure no exception is thrown
        /// </summary>
        [TestMethod]
        public void DataDictionaryObjectCreatorService_TransformRawDataIntoFormattedObjects_NoRawDataDoesNotThrownException()
        {
            // Arrange
            int expectedResultsCount = 0;

            // Act
            var results = _biz.TransformRawDataIntoFormattedObjects(new List<TableColumnInfoRaw>());

            // Assert
            Assert.AreEqual(expectedResultsCount, results.Count());
        }

        /// <summary>
        /// Tests successful transformation using a variety of data scenarios
        /// </summary>
        [TestMethod]
        public void DataDictionaryObjectCreatorService_TransformRawDataIntoFormattedObjects_Success()
        {
            // Arrange
            int expectedRawDataCount = 7;
            int expectedResultsTableCount = 6;

            // Act
            var results = _biz.TransformRawDataIntoFormattedObjects(_testData);

            // Assert
            if (_testData.Count != expectedRawDataCount)
                Assert.Fail(string.Format("Expected {0} records in {1}", expectedRawDataCount, nameof(_testData)));
            if (results.Count() != expectedResultsTableCount)
                Assert.Fail(string.Format("Expected {0} records in {1}", expectedResultsTableCount, nameof(results)));
            Assert.AreEqual(1, results.First(f => f.TableName == "SampleTableShouldNotBeRemoved").TableColumns.Count, "SampleTableShouldNotBeRemoved columns count");
            Assert.AreEqual(2, results.First(f => f.TableName == "TableWithDescriptionNoColumnDescription").TableColumns.Count, "TableWithDescriptionNoColumnDescription columns count");
        }
        #endregion Public methods/tests
    }
}
