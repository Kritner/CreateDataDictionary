using System;
using System.Collections.Generic;
using System.Linq;

namespace CreateDataDictionary.Business.Models
{
    /// <summary>
    /// Table information for use in the table portion of the data dictionary
    /// </summary>
    public class TableInfo
    {
        #region Properties
        /// <summary>
        /// The table's name
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// The table's description
        /// </summary>
        public string TableDescription { get; private set; }

        /// <summary>
        /// The table's last modified date
        /// </summary>
        public DateTime LastModified{ get; private set; }

        /// <summary>
        /// The columns that belong to the table
        /// </summary>
        public List<TableColumnInfo> TableColumns { get; private set; }
        #endregion Properties

        #region ctor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="tableDescription">The table description</param>
        /// <param name="lastModified">The last modified date</param>
        /// <param name="tableColumns">The table columns</param>
        public TableInfo(string tableName, string tableDescription, DateTime lastModified, List<TableColumnInfo> tableColumns)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            if (string.IsNullOrEmpty(tableDescription))
                tableDescription = string.Empty;
            if (tableColumns == null)
                throw new ArgumentNullException(nameof(tableColumns));
            if (tableColumns.Count() == 0)
                throw new ArgumentException(nameof(tableColumns));

            TableName = tableName;
            TableDescription = tableDescription;
            LastModified = lastModified;
            TableColumns = tableColumns;
        }
        #endregion ctor
    }
}