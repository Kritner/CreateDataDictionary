using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for DataDictionaryExclusionRulesService
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableExclusionRulesServiceTests
    {
        #region private
        TableExclusionRulesService _biz;
        List<TableColumnInfoRaw> _testData;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _biz = new TableExclusionRulesService();
            _testData = DataHelpers.GetSampleTableColumnInfoRaw();
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// GetRules returns an <see cref="IEnumerable"/> of <see cref="IDataDictionaryTableExcluder"/>
        /// </summary>
        /// <remarks>
        /// As of writing the test, there are 5 rules that should be applied
        /// </remarks>
        [TestMethod]
        public void TableExclusionRulesService_GetRules_ReturnsRules()
        {
            // Act
            int expectedRules = 5;
            var results = _biz.GetRules();

            // Assert
            Assert.IsInstanceOfType(results, typeof(IEnumerable<IDataDictionaryTableExcluder>), "results type");
            Assert.AreEqual(expectedRules, results.Count(), "results count");
        }

        /// <summary>
        /// FilterTablesMeetingRuleCriteria throws <see cref="ArgumentNullException"/> with null rules
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableExclusionRulesService_FilterTablesMeetingRuleCriteria_ArgumentNullExceptionWithNullRules()
        {
            // Arrange / Act / Assert
            var results = _biz.FilterTablesMeetingRuleCriteria(null, _testData);
        }

        /// <summary>
        /// FilterTablesMeetingRuleCriteria throws <see cref="ArgumentNullException"/> with null table ienumerable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FilterTablesMeetingRuleCriteria_FilterTablesMeetingRuleCriteria_ArgumentNullExceptionWithNullTables()
        {
            // Arrange / Act / Assert
            var results = _biz.FilterTablesMeetingRuleCriteria(_biz.GetRules(), null);
        }

        /// <summary>
        /// FilterTablesMeetingRuleCriteria throws <see cref="ArgumentException"/> with empty table ienumerable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FilterTablesMeetingRuleCriteria_FilterTablesMeetingRuleCriteria_ArgumentExceptionWithEmptyTables()
        {
            // Arrange / Act / Assert
            var results = _biz.FilterTablesMeetingRuleCriteria(_biz.GetRules(), new List<TableColumnInfoRaw>());
        }

        /// <summary>
        /// FilterTablesMeetingRuleCriteria returns the original list when no rules are present
        /// </summary>
        [TestMethod]
        public void FilterTablesMeetingRuleCriteria_FilterTablesMeetingRuleCriteria_OriginalReturnedWithNoRules()
        {
            // Arrange
            int originalCount = _testData.Count;

            // Act
            var results = _biz.FilterTablesMeetingRuleCriteria(new List<IDataDictionaryTableExcluder>(), _testData);

            // Assert
            Assert.AreEqual(originalCount, results.Count(), "results");
        }

        /// <summary>
        /// FilterTablesMeetingRuleCriteria successfully filters tables from the list when provided with not null/not empty parameters
        /// </summary>
        [TestMethod]
        public void FilterTablesMeetingRuleCriteria_FilterTablesMeetingRuleCriteria_Success()
        {
            // Arrange
            int originalCount = _testData.Count;

            // Act
            var results = _biz.FilterTablesMeetingRuleCriteria(_biz.GetRules(), _testData);

            // Assert
            Assert.AreNotEqual(originalCount, results.Count());
        }
        #endregion Public methods/tests
    }
}
