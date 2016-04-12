using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataDictionary.Business.Common
{

    /// <summary>
    /// SQL Queries used in the application
    /// </summary>
    public static class SqlQuery
    {
        #region const
        
        #region Table and column descriptions query
        /// <summary>
        /// This query returns all table names, their descriptions, their columns, their column descriptions, 
        /// along with primary key, precision, and data type information.
        /// 
        /// This could probably be optimised, but I was going on of a base query found elsewhere in the system.
        /// And it seems to get the job done.
        /// </summary>
        public const string _TABLE_COLUMN_DESCRIPTIONS =
            @"
Select 
	c.TABLE_NAME,
	isNull(epTables.value, '') as TABLE_DESCRIPTION,
	C.Column_Name as COLUMN_NAME, 
	isNull(epColumns.value, '') as COLUMN_DESCRIPTION,
	Data_TYPE as ColType, 
	case Character_Maximum_Length 
		when null then isnull(Numeric_Precision,0) 
		else Character_Maximum_Length 
	End as Length, 
	isNull(COLUMN_DEFAULT, '') as DefaultValue, 
	sc.IS_NULLABLE as Nulls, 
	K.Ordinal_Position as KeySeq,
	modify_date
From INFORMATION_SCHEMA.COLUMNS c 
left join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc on tc.TABLE_NAME = c.TABLE_NAME 
	and CONSTRAINT_TYPE='PRIMARY KEY' 
left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE k on k.TABLE_NAME = c.TABLE_NAME 
	and k.COLUMN_NAME = c.COLUMN_NAME 
	and k.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
left join INFORMATION_SCHEMA.TABLES it on tc.table_name = it.table_name
inner	join sys.tables st on c.table_name = st.name
left	join sys.columns sc on st.object_id = sc.object_id
	and c.column_name = sc.name
left	join sys.extended_properties epTables on st.object_id = epTables.major_id
	and epTables.minor_id = 0
left	join sys.extended_properties epColumns on st.object_id = epColumns.major_id
	and epColumns.minor_id = sc.column_id 
where	it.TABLE_TYPE='BASE TABLE'
Order BY c.TABLE_NAME, C.Ordinal_Position
            ";
        #endregion Table and column descriptions query
        
        #region Create description for table
        /// <summary>
        /// Create (or update) an extended property for a table
        /// </summary>
        /// <remarks>
        /// {0} is table name
        /// {1} is table description
        /// </remarks>
        /// <example>
        /// <code>
        ///     string.Format(_SCRIPT_TEMPLATE_FOR_TABLE, "MyTable", "My Table Description");
        /// </code>
        /// </example>
        public const string _SCRIPT_TEMPLATE_FOR_TABLE =
            @"
IF NOT EXISTS (
    SELECT NULL 
    FROM SYS.EXTENDED_PROPERTIES 
    WHERE [major_id] = OBJECT_ID('{0}') 
        AND [name] = N'MS_Description' 
        AND [minor_id] = 0
)
    EXECUTE sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'{1}', 
        @level0type = N'SCHEMA', 
        @level0name = N'dbo', 
        @level1type = N'TABLE', 
        @level1name = N'{0}';
ELSE
    EXECUTE sp_updateextendedproperty 
        @name = N'MS_Description', 
        @value = N'{1}', 
        @level0type = N'SCHEMA', 
        @level0name = N'dbo', 
        @level1type = N'TABLE', 
        @level1name = N'{0}';
            ";
        #endregion Create description for table
        
        #region Create description for table column
        /// <summary>
        /// Create (or update) an extended property for a table's column
        /// </summary>
        /// <remarks>
        /// {0} is table name
        /// {1} is column name
        /// {2} is column description
        /// </remarks>
        /// <example>
        /// <code>
        ///     string.Format(_SCRIPT_TEMPLATE_FOR_TABLE_COLUMN, "MyTable", "MyColumn", "My Column Description");
        /// </code>
        /// </example>
        public const string _SCRIPT_TEMPLATE_FOR_TABLE_COLUMN =
            @"
IF NOT EXISTS (
    SELECT NULL 
    FROM SYS.EXTENDED_PROPERTIES 
    WHERE [major_id] = OBJECT_ID('{0}') 
        AND [name] = N'MS_Description' 
        AND [minor_id] = (
            SELECT [column_id] 
            FROM SYS.COLUMNS 
            WHERE [name] = '{1}' 
                AND [object_id] = OBJECT_ID('{0}')
        )
)
    EXECUTE sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'{2}', 
        @level0type = N'SCHEMA', 
        @level0name = N'dbo', 
        @level1type = N'TABLE', 
        @level1name = N'{0}', 
        @level2type = N'COLUMN', 
        @level2name = N'{1}';
ELSE
    EXECUTE sp_updateextendedproperty 
        @name = N'MS_Description', 
        @value = N'{2}', 
        @level0type = N'SCHEMA', 
        @level0name = N'dbo', 
        @level1type = N'TABLE', 
        @level1name = N'{0}', 
        @level2type = N'COLUMN', 
        @level2name = N'{1}';
            ";
        #endregion Create description for table column
        
        #endregion const
    }
}
