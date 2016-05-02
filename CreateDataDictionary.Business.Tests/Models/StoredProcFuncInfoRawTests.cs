using System;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Models
{
    /// <summary>
    /// Tests for StoredProcFuncInfoRaw
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StoredProcFuncInfoRawTests
    {
        #region private
        private StoredProcFuncInfoRaw _biz;

        private string _objectName;
        private string _objectType;
        private int? _parameterId;
        private string _parameterName;
        private string _parameterDataType;
        private int? _parameterMaxLength;
        private bool? _isOutPutParameter;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _objectName = "ObjectName";
            _objectType = "Stored Procedure";
            _parameterId = 1;
            _parameterName = "Parameter";
            _parameterDataType = "DataTime";
            _parameterMaxLength = 8;
            _isOutPutParameter = false;

            _biz = new StoredProcFuncInfoRaw(
                _objectName,
                _objectType,
                _parameterId,
                _parameterName,
                _parameterDataType,
                _parameterMaxLength,
                _isOutPutParameter
            );
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when ObjectName is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfoRaw_ctor_ArgumentNullExceptionThrownEmptyObjectName()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfoRaw(
                null,
                _objectType,
                _parameterId,
                _parameterName,
                _parameterDataType,
                _parameterMaxLength,
                _isOutPutParameter
            );
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when ObjectType is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfoRaw_ctor_ArgumentNullExceptionThrownEmptyObjectType()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfoRaw(
                _objectName,
                null,
                _parameterId,
                _parameterName,
                _parameterDataType,
                _parameterMaxLength,
                _isOutPutParameter
            );
        }

        /// <summary>
        /// Tests that the ctor parameters become class properties
        /// </summary>
        [TestMethod]
        public void StoredProcFuncInfoRaw_ctor_ParametersBecomeProperties()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(StoredProcFuncInfoRaw));
            Assert.AreEqual(_objectName, _biz.ObjectName, nameof(_biz.ObjectName));
            Assert.AreEqual(_objectType, _biz.ObjectType, nameof(_biz.ObjectType));
            Assert.AreEqual(_parameterId, _biz.ParameterId, nameof(_biz.ParameterId));
            Assert.AreEqual(_parameterName, _biz.ParameterName, nameof(_biz.ParameterName));
            Assert.AreEqual(_parameterDataType, _biz.ParameterDataType, nameof(_biz.ParameterDataType));
            Assert.AreEqual(_parameterMaxLength, _biz.ParameterMaxLength, nameof(_biz.ParameterMaxLength));
            Assert.AreEqual(_isOutPutParameter, _biz.IsOutParameter, nameof(_biz.IsOutParameter));
        }
        #endregion Public methods/tests
    }
}
