using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Services
{
    /// <summary>
    /// Tests for DataDictionaryTableExcluderRegex
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableExcluderRegexTests
    {
        #region private
        private List<TableColumnInfoRaw> _testData;
        private Regex _testRegex;
        private TableExcluderRegex _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _testData = DataHelpers.GetSampleTableColumnInfoRaw();
            _testRegex = new Regex(@"bak$");
            _biz = new TableExcluderRegex(_testRegex);
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// Ensure table containing "bak" (for backup) is removed from the test list
        /// </summary>
        [TestMethod]
        public void TableExcluderRegex_RemoveTables_bakRemoved()
        {
            // Arrange
            int expectedTestDataCount = 7;
            string tableShouldBeRemoved = "ToBeRemovedbak";

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
        #endregion Public methods/tests
    }
}
