using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Models
{

    /// <summary>
    /// Tests for TableInfo
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableInfoTests
    {
        #region private
        private string _tableName;
        private string _tableDescription;
        private DateTime _lastModified;
        private List<TableColumnInfo> _tableColumns;
        private TableInfo _biz;
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
            _lastModified = new DateTime(2000, 1, 1);
            _tableColumns = new List<TableColumnInfo>()
            {
                new TableColumnInfo(
                    "ColumnName",
                    "Column Description",
                    "varchar",
                    255,
                    "Default",
                    true,
                    1
                )
            };
            _biz = new TableInfo(
                _tableName,
                _tableDescription,
                _lastModified,
                _tableColumns
            );
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException thrown when TableName is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableInfo_ctor_ArgumentNullExceptionWhenNullTableName()
        {
            // Arrange / Act / Assert
            _biz = new TableInfo(
                null,
                _tableDescription,
                _lastModified,
                _tableColumns
            );
        }

        /// <summary>
        /// Empty string is set when TableDescription is passed as null
        /// </summary>
        [TestMethod]
        public void TableInfo_ctor_EmptyStringForNullTableDescription()
        {
            // Act
            _biz = new TableInfo(
                _tableName,
                null,
                _lastModified,
                _tableColumns
            );

            // Assert
            Assert.AreEqual(string.Empty, _biz.TableDescription);
        }

        /// <summary>
        /// ArgumentNullException thrown when TableColumns is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableInfo_ctor_ArgumentNullExceptionWhenNullTableColumns()
        {
            // Arrange / Act / Assert
            _biz = new TableInfo(
                _tableName,
                _tableDescription,
                _lastModified,
                null
            );
        }

        /// <summary>
        /// ArgumentException thrown when TableColumns has 0 records
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TableInfo_ctor_ArgumentExceptionWhenTableColumnsZeroRecords()
        {
            // Arrange / Act / Assert
            _biz = new TableInfo(
                _tableName,
                _tableDescription,
                _lastModified,
                new List<TableColumnInfo>()
            );
        }

        /// <summary>
        /// Parameters become properties
        /// </summary>
        [TestMethod]
        public void TableInfo_ctor_ParametersBecomeProperties()
        {
            // Assert
            Assert.AreEqual(_tableName, _biz.TableName, nameof(_biz.TableName));
            Assert.AreEqual(_tableDescription, _biz.TableDescription, nameof(_biz.TableDescription));
            Assert.AreEqual(_lastModified, _biz.LastModified, nameof(_biz.LastModified));
            Assert.AreEqual(_tableColumns, _biz.TableColumns, nameof(_biz.TableColumns));
        }
        #endregion Public methods/tests
    }
}
