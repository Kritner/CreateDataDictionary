using System;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Models
{
    /// <summary>
    /// Tests for TableColumnInfoRaw
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableColumnInfoRawTests
    {
        #region private
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
        private TableColumnInfoRaw _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
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

        #region public methods/tests
        /// <summary>
        /// ArgumentNullException thrown when TableName is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableColumnInfoRaw_ctor_ArgumentNullExceptionThrownWhenTableNameNull()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfoRaw(
                null,
                _tableDescription,
                _columnName,
                _columnDescription,
                _columnType,
                _columnLength,
                _defaultValue,
                _allowsNull,
                _keySequence,
                _lastModified
            );
        }

        /// <summary>
        /// ArgumentNullException thrown when ColumnName is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableColumnInfoRaw_ctor_ArgumentNullExceptionThrownWhenColumnNameNull()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfoRaw(
                _tableName,
                _tableDescription,
                null,
                _columnDescription,
                _columnType,
                _columnLength,
                _defaultValue,
                _allowsNull,
                _keySequence,
                _lastModified
            );
        }

        /// <summary>
        /// ArgumentNullException thrown when ColumnType is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableColumnInfoRaw_ctor_ArgumentNullExceptionThrownWhenColumnTypeNull()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfoRaw(
                _tableName,
                _tableDescription,
                _columnName,
                _columnDescription,
                null,
                _columnLength,
                _defaultValue,
                _allowsNull,
                _keySequence,
                _lastModified
            );
        }

        /// <summary>
        /// When TableDescription is null, set to empty string
        /// </summary>
        [TestMethod]
        public void TableColumnInfoRaw_ctor_WhenTableDescriptionNullSetAsEmptyString()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfoRaw(
                _tableName,
                null,
                _columnName,
                _columnDescription,
                _columnType,
                _columnLength,
                _defaultValue,
                _allowsNull,
                _keySequence,
                _lastModified
            );

            // Assert
            Assert.AreEqual(string.Empty, _biz.TableDescription);
        }

        /// <summary>
        /// When TableDescription is null, set to empty string
        /// </summary>
        [TestMethod]
        public void TableColumnInfoRaw_ctor_WhenColumnDescriptionNullSetAsEmptyString()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfoRaw(
                _tableName,
                _tableDescription,
                _columnName,
                null,
                _columnType,
                _columnLength,
                _defaultValue,
                _allowsNull,
                _keySequence,
                _lastModified
            );

            // Assert
            Assert.AreEqual(string.Empty, _biz.ColumnDescription);
        }

        /// <summary>
        /// Parameters passed in ctor become properties
        /// </summary>
        [TestMethod]
        public void TableColumnInfoRaw_ctor_ParametersBecomeProperties()
        {
            // Arrange / Act
            _biz = new TableColumnInfoRaw(
                _tableName,
                _tableDescription,
                _columnName,
                _columnDescription,
                _columnType,
                _columnLength,
                _defaultValue,
                _allowsNull,
                _keySequence,
                _lastModified
            );

            // Assert
            Assert.AreEqual(_tableName, _biz.TableName, nameof(_biz.TableName));
            Assert.AreEqual(_tableDescription, _biz.TableDescription, nameof(_biz.TableDescription));
            Assert.AreEqual(_columnName, _biz.ColumnName, nameof(_biz.ColumnName));
            Assert.AreEqual(_columnDescription, _biz.ColumnDescription, nameof(_biz.ColumnDescription));
            Assert.AreEqual(_columnType, _biz.ColumnType, nameof(_biz.ColumnType));
            Assert.AreEqual(_columnLength, _biz.ColumnLength, nameof(_biz.ColumnLength));
            Assert.AreEqual(_defaultValue, _biz.DefaultValue, nameof(_biz.DefaultValue));
            Assert.AreEqual(_allowsNull, _biz.AllowsNull, nameof(_biz.AllowsNull));
            Assert.AreEqual(_keySequence, _biz.KeySequence, nameof(_biz.KeySequence));
            Assert.AreEqual(_lastModified, _biz.LastModified, nameof(_biz.LastModified));
        }
        #endregion public methods/tests
    }
}
