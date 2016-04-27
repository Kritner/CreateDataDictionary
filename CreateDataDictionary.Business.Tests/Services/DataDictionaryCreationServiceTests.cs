using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ClosedXML.Excel;
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
        private Mock<IDataDictionaryTableDataProvider> _mockIDataDictionaryTableDataProvider;
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
            _mockIDataDictionaryTableDataProvider = new Mock<IDataDictionaryTableDataProvider>();
            _mockIDataDictionaryReportGenerator = new Mock<IDataDictionaryReportGenerator>();

            _biz = new DataDictionaryCreationService(
                _mockIDataDictionaryTableDataProvider.Object,
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
        /// <see cref="ArgumentNullException"/> thrown when no provided <see cref="IDataDictionaryTableDataProvider"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryCreationService_ctor_ArgumentNullExceptionWithNoIDataDictionaryTableDataProvider()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryCreationService(
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
                _mockIDataDictionaryTableDataProvider.Object,
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
            _mockIDataDictionaryTableDataProvider.Verify(v => v.GetTableData(), Times.Once, "GetTableData");
            _mockIDataDictionaryReportGenerator.Verify(v => v.GenerateReport(It.IsAny<IEnumerable<TableInfo>>()), Times.Once, "GenerateReport");
            _mockIDataDictionaryReportGenerator.Verify(v => v.SaveReport(It.IsAny<XLWorkbook>(), It.IsAny<string>()), Times.Once, "SaveReport");
        }
        #endregion Public methods/tests
    }
}
