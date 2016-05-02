using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreateDataDictionary.Business.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CreateDataDictionary.Business.Tests.Services
{
    /// <summary>
    /// Tests for GetDbStoredProcFuncInfoTests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GetDbStoredProcFuncInfoTests
    {
        #region private
        private Mock<IBaseDatabaseConnection> _mockIBaseDatabaseConnection;
        private Mock<IDbConnection> _mockIDbConnection;
        private Mock<IDbCommand> _mockIDbCommand;
        private Mock<IDataReader> _mockIDataReader;
        private GetDbStoredProcFuncInfo _biz;

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
            _mockIBaseDatabaseConnection = new Mock<IBaseDatabaseConnection>();
            _mockIDbConnection = new Mock<IDbConnection>();
            _mockIDbCommand = new Mock<IDbCommand>();
            _mockIDataReader = new Mock<IDataReader>();

            _mockIBaseDatabaseConnection
                .Setup(s => s.GetDatabaseConnection())
                .Returns(_mockIDbConnection.Object);
            _mockIDbConnection
                .Setup(s => s.CreateCommand())
                .Returns(_mockIDbCommand.Object);
            _mockIDbCommand
                .Setup(s => s.ExecuteReader())
                .Returns(_mockIDataReader.Object);

            _biz = new GetDbStoredProcFuncInfo(_mockIBaseDatabaseConnection.Object);

            _objectName = "ObjectName";
            _objectType = "ObjectType";
            _parameterId = 1;
            _parameterName = "ParameterName";
            _parameterDataType = "ParameterType";
            _parameterMaxLength = 5;
            _isOutPutParameter = true;
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// GetDbStoredProcFuncInfo constructed successfully
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_ctor_ConstructedSuccessfully()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(GetDbStoredProcFuncInfo));
        }

        /// <summary>
        /// ArgumentNullException thrown when no provided IBaseDatabaseConnection
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDbStoredProcFuncInfo_ctor_ArgumentNullExceptionThrownWhenIBaseDatabaseConnectionNotProvided()
        {
            // Arrange / Act / Assert
            _biz = new GetDbStoredProcFuncInfo(null);
        }

        /// <summary>
        /// Tests properties are assigned correctly to object when there are no nulls
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_GetStoredProcFunctionInformation_NoNulls()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, false, false, false, false);

            // Act
            var results = _biz.GetStoredProcFunctionInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_objectName, results.First().ObjectName, nameof(_objectName));
            Assert.AreEqual(_objectType, results.First().ObjectType, nameof(_objectType));
            Assert.AreEqual(_parameterId, results.First().ParameterId, nameof(_parameterId));
            Assert.AreEqual(_parameterName, results.First().ParameterName, nameof(_parameterName));
            Assert.AreEqual(_parameterDataType, results.First().ParameterDataType, nameof(_parameterDataType));
            Assert.AreEqual(_parameterMaxLength, results.First().ParameterMaxLength, nameof(_parameterMaxLength));
            Assert.AreEqual(_isOutPutParameter, results.First().IsOutParameter, nameof(_isOutPutParameter));
        }

        /// <summary>
        /// Tests properties are assigned correctly to object when there is a null parameter ID
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_GetStoredProcFunctionInformation_NullParameterId()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(true, false, false, false, false);

            // Act
            var results = _biz.GetStoredProcFunctionInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_objectName, results.First().ObjectName, nameof(_objectName));
            Assert.AreEqual(_objectType, results.First().ObjectType, nameof(_objectType));
            Assert.AreEqual(null, results.First().ParameterId, nameof(_parameterId));
            Assert.AreEqual(_parameterName, results.First().ParameterName, nameof(_parameterName));
            Assert.AreEqual(_parameterDataType, results.First().ParameterDataType, nameof(_parameterDataType));
            Assert.AreEqual(_parameterMaxLength, results.First().ParameterMaxLength, nameof(_parameterMaxLength));
            Assert.AreEqual(_isOutPutParameter, results.First().IsOutParameter, nameof(_isOutPutParameter));
        }

        /// <summary>
        /// Tests properties are assigned correctly to object when there is a null parameter name
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_GetStoredProcFunctionInformation_NullParameterName()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, true, false, false, false);

            // Act
            var results = _biz.GetStoredProcFunctionInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_objectName, results.First().ObjectName, nameof(_objectName));
            Assert.AreEqual(_objectType, results.First().ObjectType, nameof(_objectType));
            Assert.AreEqual(_parameterId, results.First().ParameterId, nameof(_parameterId));
            Assert.AreEqual(null, results.First().ParameterName, nameof(_parameterName));
            Assert.AreEqual(_parameterDataType, results.First().ParameterDataType, nameof(_parameterDataType));
            Assert.AreEqual(_parameterMaxLength, results.First().ParameterMaxLength, nameof(_parameterMaxLength));
            Assert.AreEqual(_isOutPutParameter, results.First().IsOutParameter, nameof(_isOutPutParameter));
        }

        /// <summary>
        /// Tests properties are assigned correctly to object when there is a null parameter data type
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_GetStoredProcFunctionInformation_NullParameterDataType()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, false, true, false, false);

            // Act
            var results = _biz.GetStoredProcFunctionInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_objectName, results.First().ObjectName, nameof(_objectName));
            Assert.AreEqual(_objectType, results.First().ObjectType, nameof(_objectType));
            Assert.AreEqual(_parameterId, results.First().ParameterId, nameof(_parameterId));
            Assert.AreEqual(_parameterName, results.First().ParameterName, nameof(_parameterName));
            Assert.AreEqual(null, results.First().ParameterDataType, nameof(_parameterDataType));
            Assert.AreEqual(_parameterMaxLength, results.First().ParameterMaxLength, nameof(_parameterMaxLength));
            Assert.AreEqual(_isOutPutParameter, results.First().IsOutParameter, nameof(_isOutPutParameter));
        }

        /// <summary>
        /// Tests properties are assigned correctly to object when there is a null parameter length
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_GetStoredProcFunctionInformation_NullLength()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, false, false, true, false);

            // Act
            var results = _biz.GetStoredProcFunctionInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_objectName, results.First().ObjectName, nameof(_objectName));
            Assert.AreEqual(_objectType, results.First().ObjectType, nameof(_objectType));
            Assert.AreEqual(_parameterId, results.First().ParameterId, nameof(_parameterId));
            Assert.AreEqual(_parameterName, results.First().ParameterName, nameof(_parameterName));
            Assert.AreEqual(_parameterDataType, results.First().ParameterDataType, nameof(_parameterDataType));
            Assert.AreEqual(null, results.First().ParameterMaxLength, nameof(_parameterMaxLength));
            Assert.AreEqual(_isOutPutParameter, results.First().IsOutParameter, nameof(_isOutPutParameter));
        }

        /// <summary>
        /// Tests properties are assigned correctly to object when there is a null parameter output flag
        /// </summary>
        [TestMethod]
        public void GetDbStoredProcFuncInfo_GetStoredProcFunctionInformation_NullOutPutParameter()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, false, false, false, true);

            // Act
            var results = _biz.GetStoredProcFunctionInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_objectName, results.First().ObjectName, nameof(_objectName));
            Assert.AreEqual(_objectType, results.First().ObjectType, nameof(_objectType));
            Assert.AreEqual(_parameterId, results.First().ParameterId, nameof(_parameterId));
            Assert.AreEqual(_parameterName, results.First().ParameterName, nameof(_parameterName));
            Assert.AreEqual(_parameterDataType, results.First().ParameterDataType, nameof(_parameterDataType));
            Assert.AreEqual(_parameterMaxLength, results.First().ParameterMaxLength, nameof(_parameterMaxLength));
            Assert.AreEqual(null, results.First().IsOutParameter, nameof(_isOutPutParameter));
        }
        #endregion Public methods/tests

        #region Private methods
        /// <summary>
        /// Setup the data reader return
        /// </summary>
        /// <param name="isParameterIdDbNull">Should ParameterId return null?</param>
        /// <param name="isParameterNameDbNull">Should parameter name return null?</param>
        /// <param name="isParameterTypeDbNull">Should paramter type return null?</param>
        /// <param name="isLengthDbNull">Should length return null?</param>
        /// <param name="isOutputParameterDbNull">Should output parameter return null?</param>
        /// <returns></returns>
        private Mock<IDataReader> SetupTestForIDataReader(bool isParameterIdDbNull, bool isParameterNameDbNull, bool isParameterTypeDbNull, bool isLengthDbNull, bool isOutputParameterDbNull)
        {
            bool readToggle = true;
            _mockIDataReader
                .Setup(s => s.Read())
                // Returns "true" on first call
                .Returns(() => readToggle)
                // After first call the callback is fired, 
                // setting readToggle to false, 
                // which would prevent Read() from being run subsequent times.
                .Callback(() => readToggle = false);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ObjectName"))
                .Returns(0);
            _mockIDataReader
                .Setup(s => s.GetString(0))
                .Returns(_objectName);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ObjectType"))
                .Returns(1);
            _mockIDataReader
                .Setup(s => s.GetString(1))
                .Returns(_objectType);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ParameterId"))
                .Returns(2);
            _mockIDataReader
                .Setup(s => s.IsDBNull(2))
                .Returns(isParameterIdDbNull);
            _mockIDataReader
                .Setup(s => s.GetInt32(2))
                .Returns((int)_parameterId);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ParameterName"))
                .Returns(3);
            _mockIDataReader
                .Setup(s => s.GetString(3))
                .Returns(isParameterNameDbNull ? null : _parameterName);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ParameterDataType"))
                .Returns(4);
            _mockIDataReader
                .Setup(s => s.GetString(4))
                .Returns(isParameterTypeDbNull ? null : _parameterDataType);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ParameterMaxLength"))
                .Returns(5);
            _mockIDataReader
                .Setup(s => s.IsDBNull(5))
                .Returns(isLengthDbNull);
            _mockIDataReader
                .Setup(s => s.GetInt32(5))
                .Returns((int)_parameterMaxLength);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("IsOutPutParameter"))
                .Returns(6);
            _mockIDataReader
                .Setup(s => s.IsDBNull(6))
                .Returns(isOutputParameterDbNull);
            _mockIDataReader
                .Setup(s => s.GetBoolean(6))
                .Returns((bool)_isOutPutParameter);

            return _mockIDataReader;
        }
        #endregion Private methods
    }
}
