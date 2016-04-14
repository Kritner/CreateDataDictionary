using System;

namespace CreateDataDictionary.Business.Models
{

    /// <summary>
    /// Table information for use in the column portion of the data dictionary
    /// </summary>
    public class TableColumnInfo
    {
        #region Properties
        /// <summary>
        /// The table to which this column belongs
        /// </summary>
        public TableInfo Table { get; set; }

        /// <summary>
        /// The column name
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// The column's description
        /// </summary>
        public string ColumnDescription { get; private set; }
        
        /// <summary>
        /// The column data type
        /// </summary>
        public string ColumnDataType { get; private set; }

        /// <summary>
        /// The column's length (not applicable for all column data types)
        /// </summary>
        public int? ColumnLength { get; private set; }

        /// <summary>
        /// The column's default value
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Does the column allow nulls?
        /// </summary>
        public bool AllowsNulls { get; private set; }

        /// <summary>
        /// The key sequence.  When not part of a primary key, set as 0.
        /// </summary>
        public int? KeySequence { get; private set; }
        #endregion Properties

        #region ctor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="columnDescription">The column description</param>
        /// <param name="columnDataType">The column data type</param>
        /// <param name="columnLength">The column length</param>
        /// <param name="defaultValue">The default value</param>
        /// <param name="allowsNulls">Allows nulls?</param>
        /// <param name="keySequence">The key sequence</param>
        public TableColumnInfo(string columnName, string columnDescription, string columnDataType, int? columnLength, string defaultValue, bool allowsNulls, int? keySequence)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));
            if (string.IsNullOrEmpty(columnDescription))
                columnDescription = string.Empty;
            if (string.IsNullOrEmpty(columnDataType))
                throw new ArgumentNullException(nameof(columnDataType));
            if (string.IsNullOrEmpty(defaultValue))
                defaultValue = string.Empty;

            ColumnName = columnName;
            ColumnDescription = columnDescription;
            ColumnDataType = columnDataType;
            ColumnLength = columnLength;
            DefaultValue = defaultValue;
            AllowsNulls = allowsNulls;
            KeySequence = keySequence;
        }
        #endregion ctor
    }
}