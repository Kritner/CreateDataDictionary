using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Tests.Services
{
    /// <summary>
    /// Tests for GetDbTableColumnInfoService
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GetDbTableColumnInfoServiceTests
    {
        #region private
        private Mock<IBaseDatabaseConnection> _mockIBaseDatabaseConnection;
        private Mock<IDbConnection> _mockIDbConnection;
        private Mock<IDbCommand> _mockIDbCommand;
        private Mock<IDataReader> _mockIDataReader;
        private GetDbTableColumnInfoService _biz;

        private string _tableName;
        private string _tableDescription;
        private string _columnName;
        private string _columnDescription;
        private string _columnType;
        private int? _columnLength;
        private string _defaultValue;
        private bool _allowsNull;
        private int? _keySequence;
        private DateTime _lastModified;
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

            _biz = new GetDbTableColumnInfoService(_mockIBaseDatabaseConnection.Object);

            _tableName = "TableName";
            _tableDescription = "Table description";
            _columnName = "ColumnName";
            _columnDescription = "Column description";
            _columnType = "varchar";
            _columnLength = 255;
            _defaultValue = "Default";
            _allowsNull = false;
            _keySequence = 1;
            _lastModified = new DateTime(2000, 1, 1);
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// GetDbTableColumnInfoService constructed successfully
        /// </summary>
        [TestMethod]
        public void GetDbTableColumnInfoService_ctor_ConstructedSuccessfully()
        {
            // Assert
            Assert.IsInstanceOfType(_biz, typeof(GetDbTableColumnInfoService));
        }

        /// <summary>
        /// ArgumentNullException thrown when no provided IBaseDatabaseConnection
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDbTableColumnInfoService_ctor_ArgumentNullExceptionThrownWhenIBaseDatabaseConnectionNotProvided()
        {
            // Arrange / Act / Assert
            _biz = new GetDbTableColumnInfoService(null);
        }

        /// <summary>
        /// GetTableColumnInformation pulls data from DB and returns an <see cref="IEnumerable{T}"/> of <see cref="TableColumnInfoRaw"/>
        /// </summary>
        [TestMethod]
        public void GetDbTableColumnInfoService_GetTableColumnInformation_SuccessNoDbNulls()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, false);

            // Act
            var results = _biz.GetTableColumnInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_tableName, results.First().TableName, nameof(_tableName));
            Assert.AreEqual(_tableDescription, results.First().TableDescription, nameof(_tableDescription));
            Assert.AreEqual(_columnName, results.First().ColumnName, nameof(_columnName));
            Assert.AreEqual(_columnDescription, results.First().ColumnDescription, nameof(_columnDescription));
            Assert.AreEqual(_columnType, results.First().ColumnType, nameof(_columnType));
            Assert.AreEqual(_columnLength, results.First().ColumnLength, nameof(_columnLength));
            Assert.AreEqual(_defaultValue, results.First().DefaultValue, nameof(_defaultValue));
            Assert.AreEqual(_allowsNull, results.First().AllowsNull, nameof(_allowsNull));
            Assert.AreEqual(_keySequence, results.First().KeySequence, nameof(_keySequence));
            Assert.AreEqual(_lastModified, results.First().LastModified, nameof(_lastModified));
        }

        /// <summary>
        /// GetTableColumnInformation handles null value in Length 
        /// </summary>
        [TestMethod]
        public void GetDbTableColumnInfoService_GetTableColumnInformation_SuccessLengthDbNulls()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(true, false);

            // Act
            var results = _biz.GetTableColumnInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_tableName, results.First().TableName, nameof(_tableName));
            Assert.AreEqual(_tableDescription, results.First().TableDescription, nameof(_tableDescription));
            Assert.AreEqual(_columnName, results.First().ColumnName, nameof(_columnName));
            Assert.AreEqual(_columnDescription, results.First().ColumnDescription, nameof(_columnDescription));
            Assert.AreEqual(_columnType, results.First().ColumnType, nameof(_columnType));
            Assert.AreEqual(null, results.First().ColumnLength, nameof(_columnLength));
            Assert.AreEqual(_defaultValue, results.First().DefaultValue, nameof(_defaultValue));
            Assert.AreEqual(_allowsNull, results.First().AllowsNull, nameof(_allowsNull));
            Assert.AreEqual(_keySequence, results.First().KeySequence, nameof(_keySequence));
            Assert.AreEqual(_lastModified, results.First().LastModified, nameof(_lastModified));
        }

        /// <summary>
        /// GetTableColumnInformation handles null value in KeySeq 
        /// </summary>

        [TestMethod]
        public void GetDbTableColumnInfoService_GetTableColumnInformation_SuccessKeySeqDbNulls()
        {
            // Arrange
            _mockIDataReader = SetupTestForIDataReader(false, true);

            // Act
            var results = _biz.GetTableColumnInformation();

            // Assert
            Assert.AreEqual(1, results.Count(), "results count expected to be 1");
            Assert.AreEqual(_tableName, results.First().TableName, nameof(_tableName));
            Assert.AreEqual(_tableDescription, results.First().TableDescription, nameof(_tableDescription));
            Assert.AreEqual(_columnName, results.First().ColumnName, nameof(_columnName));
            Assert.AreEqual(_columnDescription, results.First().ColumnDescription, nameof(_columnDescription));
            Assert.AreEqual(_columnType, results.First().ColumnType, nameof(_columnType));
            Assert.AreEqual(_columnLength, results.First().ColumnLength, nameof(_columnLength));
            Assert.AreEqual(_defaultValue, results.First().DefaultValue, nameof(_defaultValue));
            Assert.AreEqual(_allowsNull, results.First().AllowsNull, nameof(_allowsNull));
            Assert.AreEqual(null, results.First().KeySequence, nameof(_keySequence));
            Assert.AreEqual(_lastModified, results.First().LastModified, nameof(_lastModified));
        }
        #endregion Public methods/tests

        #region Private methods
        /// <summary>
        /// Setup the data reader return
        /// </summary>
        /// <param name="isLengthDbNull">Should length db null return true or false?</param>
        /// <param name="isKeySeqDbNull">Should keySeq db null return true or false?</param>
        /// <returns></returns>
        private Mock<IDataReader> SetupTestForIDataReader(bool isLengthDbNull, bool isKeySeqDbNull)
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
                .Setup(s => s.GetOrdinal("TABLE_NAME"))
                .Returns(0);
            _mockIDataReader
                .Setup(s => s.GetString(0))
                .Returns(_tableName);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("TABLE_DESCRIPTION"))
                .Returns(1);
            _mockIDataReader
                .Setup(s => s.GetString(1))
                .Returns(_tableDescription);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("COLUMN_NAME"))
                .Returns(2);
            _mockIDataReader
                .Setup(s => s.GetString(2))
                .Returns(_columnName);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("COLUMN_DESCRIPTION"))
                .Returns(3);
            _mockIDataReader
                .Setup(s => s.GetString(3))
                .Returns(_columnDescription);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("ColType"))
                .Returns(4);
            _mockIDataReader
                .Setup(s => s.GetString(4))
                .Returns(_columnType);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("Length"))
                .Returns(5);
            _mockIDataReader
                .Setup(s => s.IsDBNull(5))
                .Returns(isLengthDbNull);
            _mockIDataReader
                .Setup(s => s.GetInt32(5))
                .Returns((int)_columnLength);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("DefaultValue"))
                .Returns(6);
            _mockIDataReader
                .Setup(s => s.GetString(6))
                .Returns(_defaultValue);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("Nulls"))
                .Returns(7);
            _mockIDataReader
                .Setup(s => s.GetBoolean(7))
                .Returns(_allowsNull);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("KeySeq"))
                .Returns(8);
            _mockIDataReader
                .Setup(s => s.IsDBNull(8))
                .Returns(isKeySeqDbNull);
            _mockIDataReader
                .Setup(s => s.GetInt32(8))
                .Returns((int)_keySequence);

            _mockIDataReader
                .Setup(s => s.GetOrdinal("modify_date"))
                .Returns(9);
            _mockIDataReader
                .Setup(s => s.GetDateTime(9))
                .Returns(_lastModified);

            return _mockIDataReader;
        }
        #endregion Private methods
    }
}
