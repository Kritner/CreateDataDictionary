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
    /// Tests for DataDictionaryTableExcluderList
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableExcluderListTests
    {
        #region private
        private List<TableColumnInfoRaw> _testData;
        private List<string> _listToRemove;
        private TableExcluderList _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _testData = DataHelpers.GetSampleTableColumnInfoRaw();
            _listToRemove = new List<string>()
            {
                "RemoveDueToCtorParameter"
            };
            _biz = new TableExcluderList(_listToRemove);
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// RemoveTables - table "aspnet_Paths" is removed from the list 
        /// due to being present in the list of tables to remove as per the implementation.
        /// </summary>
        [TestMethod]
        public void TableExcluderList_RemoveTables_RemovesTableFromClassList()
        {
            // Arrange
            int expectedTestDataCount = 7;
            string tableShouldBeRemoved = "aspnet_Paths";
            _biz = new TableExcluderList(null);

            // Relying on specific data - if the count does not match, fail - not a perfect test by any means, but should help
            if (_testData.Count != expectedTestDataCount)
                Assert.Fail("Test data count different than expected");

            // Act
            var results = _biz.RemoveTables(_testData);

            // Assert
            Assert.AreEqual(expectedTestDataCount - 1, results.Count(), string.Format("results count expected to be one less than {0}", nameof(expectedTestDataCount)));
            Assert.IsTrue(_testData.Select(s => s.TableName).Contains(tableShouldBeRemoved), string.Format("Test data issue - {0} should be contained within list", tableShouldBeRemoved));
            Assert.IsFalse(results.Select(s => s.TableName).Contains(tableShouldBeRemoved), string.Format("{0} should not be contained within list after removal operation", tableShouldBeRemoved));
        }

        /// <summary>
        /// RemoveTables - table "RemoveDueToCtorParameter" is removed from the list 
        /// due to being present in the list of tables to remove from the constructor.
        /// </summary>
        [TestMethod]
        public void TableExcluderList_RemoveTables_RemovesTableFromConstructorList()
        {
            // Arrange
            int expectedTestDataCount = 7;
            string tableShouldBeRemoved = "RemoveDueToCtorParameter";

            // Relying on specific data - if the count does not match, fail - not a perfect test by any means, but should help
            if (_testData.Count != expectedTestDataCount)
                Assert.Fail("Test data count different than expected");

            // Act
            var results = _biz.RemoveTables(_testData);

            // Assert
            Assert.AreEqual(expectedTestDataCount - 2, results.Count(), string.Format("results count expected to be two less than {0}", nameof(expectedTestDataCount)));
            Assert.IsTrue(_testData.Select(s => s.TableName).Contains(tableShouldBeRemoved), string.Format("Test data issue - {0} should be contained within list", tableShouldBeRemoved));
            Assert.IsFalse(results.Select(s => s.TableName).Contains(tableShouldBeRemoved), string.Format("{0} should not be contained within list after removal operation", tableShouldBeRemoved));
        }
        #endregion Public methods/tests
    }
}
