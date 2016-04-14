using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataDictionary.Business.Models
{
    
    /// <summary>
    /// Represents the raw information from the DB regarding db table/column/schema information
    /// </summary>
    public class TableColumnInfoRaw
    {

        #region Properties
        /// <summary>
        /// The table's name - should always be provided
        /// </summary>
        public string TableName { get; private set; }
        
        /// <summary>
        /// The table's description - <see cref="string.Empty"/> for not provided
        /// </summary>
        public string TableDescription { get; private set; }
        
        /// <summary>
        /// The column's name - should always be provided
        /// </summary>
        public string ColumnName { get; private set; }
        
        /// <summary>
        /// The columns's description - <see cref="string.Empty"/> for not provided
        /// </summary>
        public string ColumnDescription { get; private set; }
        
        /// <summary>
        /// The column's type (bit, varchar, ect)
        /// </summary>
        public string ColumnType { get; private set; }
        
        /// <summary>
        /// The column's length (not applicable to column types like bit)
        /// </summary>
        public int? ColumnLength { get; private set; }
        
        /// <summary>
        /// The default value - <see cref="string.Empty"/> for not provided
        /// </summary>
        public string DefaultValue { get; private set; }
        
        /// <summary>
        /// Does the column allow null values?
        /// </summary>
        public bool AllowsNull { get; private set; }
        
        /// <summary>
        /// The key sequence, null indicates column is not a part of the primary key
        /// </summary>
        public int? KeySequence { get; private set; }
        
        /// <summary>
        /// When the table was last modified
        /// </summary>
        public DateTime LastModified { get; private set; }
        #endregion Properties

        #region ctor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">The table's name - should always be provided</param>
        /// <param name="tableDescription">The table's description - will sometimes be null</param>
        /// <param name="columnName">The column name - should always be provided</param>
        /// <param name="columnDescription">The column description - will sometimes be null</param>
        /// <param name="columnType">The column type - should always be provided</param>
        /// <param name="columnLength">The column length - sometimes null (for bit columnType as example)</param>
        /// <param name="defaultValue">The default value - will sometimes be null</param>
        /// <param name="allowsNull">Does the column allow null values?</param>
        /// <param name="keySequence">The primary key sequence, null means not a part of the primary key</param>
        /// <param name="lastModified">The last time the table was modified</param>
        public TableColumnInfoRaw(
            string tableName,
            string tableDescription,
            string columnName,
            string columnDescription,
            string columnType,
            int? columnLength,
            string defaultValue,
            bool allowsNull,
            int? keySequence,
            DateTime lastModified
        )
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (string.IsNullOrEmpty(tableDescription))
                tableDescription = string.Empty;
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));
            if (string.IsNullOrEmpty(columnDescription))
                columnDescription = string.Empty;
            if (string.IsNullOrEmpty(columnType))
                throw new ArgumentNullException(nameof(columnType));
            if (string.IsNullOrEmpty(defaultValue))
                defaultValue = string.Empty;

            TableName = tableName;
            TableDescription = tableDescription;
            ColumnName = columnName;
            ColumnDescription = columnDescription;
            ColumnType = columnType;
            ColumnLength = columnLength;
            DefaultValue = defaultValue;
            AllowsNull = allowsNull;
            KeySequence = keySequence;
            LastModified = lastModified;
        }
        #endregion ctor
    }
}
