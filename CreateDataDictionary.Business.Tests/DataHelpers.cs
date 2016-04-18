using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Tests
{
    /// <summary>
    /// Provides test data
    /// </summary>
    public static class DataHelpers
    {

        #region GetSampleTableColumnInfoRaw
        /// <summary>
        /// Gets sample data for testing with GetSampleTableColumnInfoRaw
        /// </summary>
        /// <remarks>
        /// Tests are dependant on specific data, changing data in function may impact tests.
        /// </remarks>
        /// <returns>List of TableColumnInfoRaw</returns>
        public static List<TableColumnInfoRaw> GetSampleTableColumnInfoRaw()
        {
            List<TableColumnInfoRaw> list = new List<TableColumnInfoRaw>();

            list.Add(new TableColumnInfoRaw(
                "SampleTableShouldNotBeRemoved",
                "This is a sample description",
                "SampleColumnName",
                "Column Description",
                "varchar",
                255,
                null,
                true,
                null,
                new DateTime(2000, 1, 1)
            ));
            list.Add(new TableColumnInfoRaw(
                "aspnet_Paths",
                "This is a table that should be removed due to DataDictionaryTableExcluderList",
                "SampleColumnName",
                string.Empty,
                "varchar",
                255,
                null,
                true,
                null,
                new DateTime(2000, 1, 1)
            ));
            list.Add(new TableColumnInfoRaw(
                "RemoveDueToCtorParameter",
                "This is a table that should be removed due to DataDictionaryTableExcluderList's constructor parameter",
                "SampleColumnName",
                string.Empty,
                "varchar",
                255,
                null,
                true,
                null,
                new DateTime(2000, 1, 1)
            ));
            list.Add(new TableColumnInfoRaw(
                "ToBeRemovedbak",
                "This is a table that should be removed due to DataDictionaryTableExcluderRegex",
                "SampleColumnName",
                string.Empty,
                "varchar",
                255,
                null,
                true,
                null,
                new DateTime(2000, 1, 1)
            ));
            list.Add(new TableColumnInfoRaw(
                "TableWithDescNoColumnDesc",
                "this is a table with a description",
                "SampleColumnNameWithNoDescription",
                string.Empty,
                "varchar",
                255,
                null,
                true,
                null,
                new DateTime(2001, 1, 1)
            ));
            list.Add(new TableColumnInfoRaw(
                "TableWithDescNoColumnDesc",
                "this is a table with a description",
                "ColumnWithDescription",
                "This is a column with a description",
                "varchar",
                255,
                null,
                true,
                null,
                new DateTime(2001, 1, 1)
            ));
            list.Add(new TableColumnInfoRaw(
                "TableWithDescNoDesc",
                string.Empty,
                "ColumnWithDescription",
                "This is a column with a description",
                "varchar",
                255,
                "Default",
                false,
                1,
                new DateTime(2002, 1, 1)
            ));

            return list;
        }
        #endregion GetSampleTableColumnInfoRaw
    }
}
