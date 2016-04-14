using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for DataDictionaryCreationService
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DataDictionaryCreationServiceTests
    {
        #region private
        private Mock<IGetDbTableColumnInfo> _mockIGetDbTableColumnInfo;
        private Mock<IDataDictionaryExclusionRules> _mockIDataDictionaryExclusionRules;
        private Mock<IDataDictionaryObjectCreator> _mockIDataDictionaryObjectCreator;
        private Mock<IDataDictionaryReportGenerator> _mockIDataDictionaryReportGenerator;
        private DataDictionaryCreationService _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockIGetDbTableColumnInfo = new Mock<IGetDbTableColumnInfo>();
            _mockIDataDictionaryExclusionRules = new Mock<IDataDictionaryExclusionRules>();
            _mockIDataDictionaryObjectCreator = new Mock<IDataDictionaryObjectCreator>();
            _mockIDataDictionaryReportGenerator = new Mock<IDataDictionaryReportGenerator>();

            _biz = new DataDictionaryCreationService(
                _mockIGetDbTableColumnInfo.Object,
                _mockIDataDictionaryExclusionRules.Object,
                _mockIDataDictionaryObjectCreator.Object,
                _mockIDataDictionaryReportGenerator.Object
            );
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// Object created successfully
        /// </summary>
        [TestMethod]
        public void DataDictionaryCreationService_ctor_Success()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(DataDictionaryCreationService));
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="IGetDbTableColumnInfo"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreationService_ctor_ArgumentNullExceptionWithNoIGetDbTableColumnInfo()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryCreationService(
                null,
                _mockIDataDictionaryExclusionRules.Object,
                _mockIDataDictionaryObjectCreator.Object,
                _mockIDataDictionaryReportGenerator.Object
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="IDataDictionaryExclusionRules"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreationService_ctor_ArgumentNullExceptionWithNoIDataDictionaryExclusionRules()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryCreationService(
                _mockIGetDbTableColumnInfo.Object,
                null,
                _mockIDataDictionaryObjectCreator.Object,
                _mockIDataDictionaryReportGenerator.Object
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="IDataDictionaryObjectCreator"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreationService_ctor_ArgumentNullExceptionWithNoIDataDictionaryObjectCreator()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryCreationService(
                _mockIGetDbTableColumnInfo.Object,
                _mockIDataDictionaryExclusionRules.Object,
                null,
                _mockIDataDictionaryReportGenerator.Object
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="IDataDictionaryReportGenerator"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreationService_ctor_ArgumentNullExceptionWithNoIDataDictionaryReportGenerator()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryCreationService(
                _mockIGetDbTableColumnInfo.Object,
                _mockIDataDictionaryExclusionRules.Object,
                _mockIDataDictionaryObjectCreator.Object,
                null
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when no filename
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreationService_Execute_ArgumentNullExceptionThrownWhenNullFilename()
        {
            // Arrange / Act / Assert
            _biz.Execute(null);
        }

        [TestMethod]
        public void DataDictionaryCreationService_Execute_ExecutesDependencies()
        {
            // Act
            _biz.Execute("FileName");

            // Assert
            _mockIGetDbTableColumnInfo.Verify(v => v.GetTableColumnInformation(), Times.Once, "GetTableColumnInformation");
            _mockIDataDictionaryExclusionRules.Verify(v => v.GetRules(), Times.Once, "GetRules");
            _mockIDataDictionaryExclusionRules.Verify(v => v.FilterTablesMeetingRuleCriteria(It.IsAny<IEnumerable<IDataDictionaryTableExcluder>>(), It.IsAny<IEnumerable<TableColumnInfoRaw>>()), Times.Once, "FilterTablesMeetingRuleCriteria");
            _mockIDataDictionaryObjectCreator.Verify(v => v.TransformRawDataIntoFormattedObjects(It.IsAny<IEnumerable<TableColumnInfoRaw>>()), Times.Once, "TransformRawDataIntoFormattedObjects");
            _mockIDataDictionaryReportGenerator.Verify(v => v.GenerateReport(It.IsAny<IEnumerable<TableInfo>>(), It.IsAny<string>()));
        }
        #endregion Public methods/tests
    }
}
