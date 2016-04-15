using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Models
{

    /// <summary>
    /// Tests for TableColumnInfo
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableColumnInfoTests
    {

        #region private
        private string _columnName;
        private string _columnDescription;
        private string _columnDataType;
        private int? _columnLength;
        private string _defaultValue;
        private bool _allowsNulls;
        private int? _keySequence;
        private TableColumnInfo _biz;
        private TableInfo _tableInfo;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _columnName = "ColumnName";
            _columnDescription = "Column description";
            _columnDataType = "varchar";
            _columnLength = 255;
            _defaultValue = "Default";
            _allowsNulls = true;
            _keySequence = 1;
            _biz = new TableColumnInfo(
                _columnName, 
                _columnDescription, 
                _columnDataType, 
                _columnLength, 
                _defaultValue, 
                _allowsNulls, 
                _keySequence
            );
            _biz.Table = _tableInfo;
            _tableInfo = new TableInfo(
                "TableName", 
                "TableDescription", 
                new DateTime(2000, 1, 1), 
                new List<TableColumnInfo>()
                {
                    new TableColumnInfo(
                        _columnName, 
                        _columnDescription, 
                        _columnDataType, 
                        _columnLength, 
                        _defaultValue, 
                        _allowsNulls, 
                        _keySequence
                    )
                }
           );
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException is thrown when providing a null ColumnName
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableColumnInfo_ctor_ArgumentNullExceptionThrownWithNullColumnName()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfo(
                null,
                _columnDescription,
                _columnDataType,
                _columnLength,
                _defaultValue,
                _allowsNulls,
                _keySequence
            );
        }
        
        /// <summary>
        /// null ColumnDescription sets as empty string
        /// </summary>
        [TestMethod]
        public void TableColumnInfo_ctor_EmptyStringWithNullColumnDescription()
        {
            // Arrange / Act
            _biz = new TableColumnInfo(
                _columnName,
                null,
                _columnDataType,
                _columnLength,
                _defaultValue,
                _allowsNulls,
                _keySequence
            );

            Assert.AreEqual(string.Empty, _biz.ColumnDescription, nameof(_biz.ColumnDescription));
        }

        /// <summary>
        /// ArgumentNullException is thrown when providing a null DataType
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableColumnInfo_ctor_ArgumentNullExceptionThrownWithNullDataType()
        {
            // Arrange / Act / Assert
            _biz = new TableColumnInfo(
                _columnName,
                _columnDescription,
                null,
                _columnLength,
                _defaultValue,
                _allowsNulls,
                _keySequence
            );
        }

        /// <summary>
        /// null DefaultValue sets as empty string
        /// </summary>
        [TestMethod]
        public void TableColumnInfo_ctor_NotApplicableWithNullDefaultValue()
        {
            // Arrange / Act
            _biz = new TableColumnInfo(
                _columnName,
                _columnDescription,
                _columnDataType,
                _columnLength,
                null,
                _allowsNulls,
                _keySequence
            );

            Assert.AreEqual("N/A", _biz.DefaultValue, nameof(_biz.DefaultValue));
        }

        /// <summary>
        /// Tests constructor parameters become properties
        /// </summary>
        [TestMethod]
        public void TableColumnInfo_ctor_ParametersBecomeProperties()
        {
            // Act
            _biz.Table = _tableInfo;

            // Assert
            Assert.AreEqual(_columnName, _biz.ColumnName, nameof(_biz.ColumnName));
            Assert.AreEqual(_columnDescription, _biz.ColumnDescription, nameof(_biz.ColumnDescription));
            Assert.AreEqual(_columnDataType, _biz.ColumnDataType, nameof(_biz.ColumnDataType));
            Assert.AreEqual(_columnLength, _biz.ColumnLength, nameof(_biz.ColumnLength));
            Assert.AreEqual(_defaultValue, _biz.DefaultValue, nameof(_biz.DefaultValue));
            Assert.AreEqual(_allowsNulls, _biz.AllowsNulls, nameof(_biz.AllowsNulls));
            Assert.AreEqual(_keySequence, _biz.KeySequence, nameof(_biz.KeySequence));

            Assert.AreEqual(_tableInfo, _biz.Table, nameof(_biz.Table));

            Assert.AreEqual(string.Format("Max Length: {0}", _columnLength), _biz.AdditionalInfoFormatted, nameof(_biz.KeySequence));
            Assert.AreEqual(true, _biz.PartOfKeyFormatted, nameof(_biz.PartOfKeyFormatted));
        }

        /// <summary>
        /// AdditionalInfoFormatted returns empty string when no length is present
        /// PartOfKeyFormatted returns false when KeySeq is null
        /// </summary>
        [TestMethod]
        public void TableColumnInfo_Property_Default()
        {
            // Arrange / Act
            _biz = new TableColumnInfo(
                _columnName,
                _columnDescription,
                _columnDataType,
                null,
                _defaultValue,
                _allowsNulls,
                null
            );

            // Assert
            Assert.AreEqual(string.Empty, _biz.AdditionalInfoFormatted, nameof(_biz.AdditionalInfoFormatted));
            Assert.AreEqual(false, _biz.PartOfKeyFormatted, nameof(_biz.PartOfKeyFormatted));
        }
        #endregion Public methods/tests

    }
}
