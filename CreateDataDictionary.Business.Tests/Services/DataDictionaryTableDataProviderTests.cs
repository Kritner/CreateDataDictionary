using System;
using System.Collections.Generic;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for DataDictionaryTableDataProvider
    /// </summary>
    [TestClass]
    public class DataDictionaryTableDataProviderTests
    {

        #region private
        private Mock<IGetDbTableColumnInfo> _mockIGetDbTableColumnInfo;
        private Mock<ITableExclusionRules> _mockIDataDictionaryExclusionRules;
        private Mock<ITableModelObjectCreator> _mockIDataDictionaryObjectCreator;
        private DataDictionaryTableDataProvider _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockIGetDbTableColumnInfo = new Mock<IGetDbTableColumnInfo>();
            _mockIDataDictionaryExclusionRules = new Mock<ITableExclusionRules>();
            _mockIDataDictionaryObjectCreator = new Mock<ITableModelObjectCreator>();

            _biz = new DataDictionaryTableDataProvider(
                _mockIGetDbTableColumnInfo.Object,
                _mockIDataDictionaryExclusionRules.Object,
                _mockIDataDictionaryObjectCreator.Object
            );
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// Object created successfully
        /// </summary>
        [TestMethod]
        public void DataDictionaryTableDataProvider_ctor_Success()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(DataDictionaryTableDataProvider));
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="IGetDbTableColumnInfo"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryTableDataProvider_ctor_ArgumentNullExceptionWithNoIGetDbTableColumnInfo()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryTableDataProvider(
                null,
                _mockIDataDictionaryExclusionRules.Object,
                _mockIDataDictionaryObjectCreator.Object
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="ITableExclusionRules"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryTableDataProvider_ctor_ArgumentNullExceptionWithNoIDataDictionaryExclusionRules()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryTableDataProvider(
                _mockIGetDbTableColumnInfo.Object,
                null,
                _mockIDataDictionaryObjectCreator.Object
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="ITableModelObjectCreator"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryTableDataProvider_ctor_ArgumentNullExceptionWithNoIDataDictionaryObjectCreator()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryTableDataProvider(
                _mockIGetDbTableColumnInfo.Object,
                _mockIDataDictionaryExclusionRules.Object,
                null
            );
        }

        [TestMethod]
        public void DataDictionaryTableDataProvider_Execute_ExecutesDependencies()
        {
            // Act
            _biz.GetTableData();

            // Assert
            _mockIGetDbTableColumnInfo.Verify(v => v.GetTableColumnInformation(), Times.Once, "GetTableColumnInformation");
            _mockIDataDictionaryExclusionRules.Verify(v => v.GetRules(), Times.Once, "GetRules");
            _mockIDataDictionaryExclusionRules.Verify(v => v.FilterTablesMeetingRuleCriteria(It.IsAny<IEnumerable<ITableExcluder>>(), It.IsAny<IEnumerable<TableColumnInfoRaw>>()), Times.Once, "FilterTablesMeetingRuleCriteria");
            _mockIDataDictionaryObjectCreator.Verify(v => v.TransformRawDataIntoFormattedObjects(It.IsAny<IEnumerable<TableColumnInfoRaw>>()), Times.Once, "TransformRawDataIntoFormattedObjects");
        }
        #endregion Public methods/tests

    }
}
