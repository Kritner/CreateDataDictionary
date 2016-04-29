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
    /// Tests for DataDictionaryStoredProcFuncDataProvider
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DataDictionaryStoredProcFuncDataProviderTests
    {
        #region private
        private Mock<IGetDbStoredProcFuncInfo> _mockIGetDbStoredProcFuncInfo;
        private Mock<IStoredProcFuncModelObjectCreator> _mockIStoredProcFuncModelObjectCreator;
        private DataDictionaryStoredProcFuncDataProvider _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockIGetDbStoredProcFuncInfo = new Mock<IGetDbStoredProcFuncInfo>();
            _mockIStoredProcFuncModelObjectCreator = new Mock<IStoredProcFuncModelObjectCreator>();
            _biz = new DataDictionaryStoredProcFuncDataProvider(
                _mockIGetDbStoredProcFuncInfo.Object,
                _mockIStoredProcFuncModelObjectCreator.Object
            );
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// Tests object is constructed properly
        /// </summary>
        [TestMethod]
        public void DataDictionaryStoredProcFuncDataProvider_ctor_ConstructedProperly()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(DataDictionaryStoredProcFuncDataProvider));
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> throw when no provided <see cref="IGetDbStoredProcFuncInfo"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryStoredProcFuncDataProvider_ctor_ArgumentNullExceptionThrownWhenNoProvidedIGetDbStoredProcFuncInfo()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryStoredProcFuncDataProvider(null, _mockIStoredProcFuncModelObjectCreator.Object);
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> throw when no provided <see cref="IStoredProcFuncModelObjectCreator"/>
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataDictionaryStoredProcFuncDataProvider_ctor_ArgumentNullExceptionThrownWhenNoProvidedIStoredProcFuncModelObjectCreator()
        {
            // Arrange / Act / Assert
            _biz = new DataDictionaryStoredProcFuncDataProvider(_mockIGetDbStoredProcFuncInfo.Object, null);
        }

        /// <summary>
        /// Execute calls interface functions and returns data
        /// </summary>
        [TestMethod]
        public void DataDictionaryStoredProcFuncDataProvider_Execute_Succeeds()
        {
            // Act
            var results = _biz.Execute();

            // Assert
            Assert.IsInstanceOfType(results, typeof(IEnumerable<StoredProcFuncInfo>));
            _mockIGetDbStoredProcFuncInfo.Verify(v => v.GetStoredProcFunctionInformation(), Times.Once, "GetStoredProcFunctionInformation");
            _mockIStoredProcFuncModelObjectCreator.Verify(v => v.TransformRawDataIntoFormattedObjects(It.IsAny<IEnumerable<StoredProcFuncInfoRaw>>()), Times.Once, "TransformRawDataIntoFormattedObjects");
        }
        #endregion Public methods/tests
    }
}
