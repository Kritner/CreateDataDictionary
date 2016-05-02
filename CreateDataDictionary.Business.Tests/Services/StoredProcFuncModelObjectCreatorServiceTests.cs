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
    /// Tests for StoredProcFuncModelObjectCreatorService
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StoredProcFuncModelObjectCreatorServiceTests
    {
        #region private
        private List<StoredProcFuncInfoRaw> _testData;
        private StoredProcFuncModelObjectCreatorService _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _testData = DataHelpers.GetSampleObjectInfoRaw();
            _biz = new StoredProcFuncModelObjectCreatorService();
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException thrown when RawData is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncModelObjectCreatorService_TransformRawDataIntoFormattedObjects_ArgumentNullExceptionThrownWhenNullRawData()
        {
            // Arrange / Act / Assert
            var results = _biz.TransformRawDataIntoFormattedObjects(null);
        }

        /// <summary>
        /// When there is no data within rawData, ensure no exception is thrown
        /// </summary>
        [TestMethod]
        public void StoredProcFuncModelObjectCreatorService_TransformRawDataIntoFormattedObjects_NoRawDataDoesNotThrownException()
        {
            // Arrange
            int expectedResultsCount = 0;

            // Act
            var results = _biz.TransformRawDataIntoFormattedObjects(new List<StoredProcFuncInfoRaw>());

            // Assert
            Assert.AreEqual(expectedResultsCount, results.Count());
        }

        /// <summary>
        /// Tests successful transformation using a variety of data scenarios
        /// </summary>
        [TestMethod]
        public void StoredProcFuncModelObjectCreatorService_TransformRawDataIntoFormattedObjects_Success()
        {
            // Arrange
            int expectedRawDataCount = 3;
            int expectedResultsTableCount = 2;

            // Act
            var results = _biz.TransformRawDataIntoFormattedObjects(_testData);

            // Assert
            if (_testData.Count != expectedRawDataCount)
                Assert.Fail(string.Format("Expected {0} records in {1}", expectedRawDataCount, nameof(_testData)));
            if (results.Count() != expectedResultsTableCount)
                Assert.Fail(string.Format("Expected {0} records in {1}", expectedResultsTableCount, nameof(results)));
            Assert.AreEqual(0, results.First(f => f.ObjectName == "ObjectNameNoParams").Parameters.Count, "ObjectNameNoParams parameter count");
            Assert.AreEqual(2, results.First(f => f.ObjectName == "ObjectNameParams").Parameters.Count, "ObjectNameParams parameter count");
        }
        #endregion Public methods/tests
    }
}
