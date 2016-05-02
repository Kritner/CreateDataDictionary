using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Models
{

    /// <summary>
    /// Tests for StoredProcFuncInfoParams
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StoredProcFuncInfoParamsTests
    {
        #region private
        private StoredProcFuncInfoParams _biz;

        private StoredProcFuncInfo _parentObject;
        private int _parameterId;
        private string _parameterName;
        private string _parameterDataType;
        private int _parameterMaxLength;
        private bool _isOutParameter;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _parameterId = 1;
            _parameterName = "ParameterName";
            _parameterDataType = "Type";
            _parameterMaxLength = 2;
            _isOutParameter = true;

            _biz = new StoredProcFuncInfoParams(
                _parameterId, 
                _parameterName, 
                _parameterDataType, 
                _parameterMaxLength, 
                _isOutParameter
            );

            List<StoredProcFuncInfoParams> bizCollection = new List<StoredProcFuncInfoParams>();
            bizCollection.Add(_biz);

            _parentObject = new StoredProcFuncInfo("ObjectName", "ObjectType", bizCollection);
            _biz.ParentObject = _parentObject;
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when null Parameter name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfo_ctor_ArgumentNullExceptionThrownWithNullParameterName()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfoParams(_parameterId, null, _parameterDataType, _parameterMaxLength, _isOutParameter);
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when null Parameter type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfo_ctor_ArgumentNullExceptionThrownWithNullParameterType()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfoParams(_parameterId, _parameterName, null, _parameterMaxLength, _isOutParameter);
        }

        /// <summary>
        /// Constructor parameters become properties
        /// </summary>
        [TestMethod]
        public void StoredProcFuncInfo_ctor_ParametersBecomeProperties()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(StoredProcFuncInfoParams));
            Assert.AreEqual(_parameterId, _biz.ParameterId, nameof(_biz.ParameterId));
            Assert.AreEqual(_parameterName, _biz.ParameterName, nameof(_biz.ParameterName));
            Assert.AreEqual(_parameterDataType, _biz.ParameterDataType, nameof(_biz.ParameterDataType));
            Assert.AreEqual(_parameterMaxLength, _biz.ParameterMaxLength, nameof(_biz.ParameterMaxLength));
            Assert.AreEqual(_isOutParameter, _biz.IsOutParameter, nameof(_biz.IsOutParameter));
        }
        #endregion Public methods/tests
    }
}
