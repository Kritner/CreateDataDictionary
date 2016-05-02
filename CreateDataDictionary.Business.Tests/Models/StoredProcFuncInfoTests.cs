using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreateDataDictionary.Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Models
{
    /// <summary>
    /// Tests for StoredProcFuncInfo
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StoredProcFuncInfoTests
    {
        #region private
        private StoredProcFuncInfo _biz;

        private string _objectName;
        private string _objectType;
        private List<StoredProcFuncInfoParams> _parameters;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _objectName = "ObjectName";
            _objectType = "ObjectType";
            _parameters = new List<StoredProcFuncInfoParams>()
            {
                new StoredProcFuncInfoParams(1, "ParameterName", "ParameterDataType", 2, true)
            };

            _biz = new StoredProcFuncInfo(_objectName, _objectType, _parameters);
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when ObjectName is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfo_ctor_ArgumentNullExceptionThrownWithNullObjectName()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfo(null, _objectType, _parameters);
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when ObjectType is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfo_ctor_ArgumentNullExceptionThrownWithNullObjectType()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfo(_objectName, null, _parameters);
        }

        /// <summary>
        /// <see cref="ArgumentNullException"/> thrown when ObjectType is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncInfo_ctor_ArgumentNullExceptionThrownWithNullParameters()
        {
            // Arrange / Act / Assert
            _biz = new StoredProcFuncInfo(_objectName, _objectType, null);
        }

        /// <summary>
        /// Constructor parameters become properties
        /// </summary>
        [TestMethod]
        public void StoredProcFuncInfo_ctor_ParametersBecomeProperties()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(StoredProcFuncInfo));
            Assert.AreEqual(_objectName, _biz.ObjectName, nameof(_biz.ObjectName));
            Assert.AreEqual(_objectType, _biz.ObjectType, nameof(_biz.ObjectType));
            Assert.AreEqual(1, _biz.Parameters.Count, "Parameters Count");
            Assert.AreEqual(_parameters.First().IsOutParameter, _biz.Parameters.First().IsOutParameter, "IsOutParameter");
            Assert.AreEqual(_parameters.First().ParameterDataType, _biz.Parameters.First().ParameterDataType, "ParameterDataType");
            Assert.AreEqual(_parameters.First().ParameterId, _biz.Parameters.First().ParameterId, "ParameterId");
            Assert.AreEqual(_parameters.First().ParameterMaxLength, _biz.Parameters.First().ParameterMaxLength, "ParameterMaxLength");
            Assert.AreEqual(_parameters.First().ParameterName, _biz.Parameters.First().ParameterName, "ParameterName");
        }
        #endregion Public methods/tests
    }
}
