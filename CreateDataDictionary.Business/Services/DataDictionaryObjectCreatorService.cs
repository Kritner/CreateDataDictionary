using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Implementation of IDataDictionaryObjectCreator.  
    /// Used to transform raw SQL data into a format to be used by the DataDictionary creator.
    /// </summary>
    public class DataDictionaryObjectCreatorService : IDataDictionaryObjectCreator
    {

        /// <summary>
        /// Transform <see cref="TableColumnInfoRaw"/> into <see cref="TableInfo"/>
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public IEnumerable<TableInfo> TransformRawDataIntoFormattedObjects(IEnumerable<TableColumnInfoRaw> rawData)
        {
            if (rawData == null)
                throw new ArgumentNullException(nameof(rawData));

            // Transform raw into Tables and TableColumns
            List<TableInfo> list = rawData
                .GroupBy(gb => gb.TableName)
                .Select(table => new TableInfo(
                    table.Key,
                    table.First().TableDescription,
                    table.First().LastModified,
                    table.Select(column => new TableColumnInfo(
                        column.ColumnName,
                        column.ColumnDescription,
                        column.ColumnType,
                        column.ColumnLength,
                        column.DefaultValue,
                        column.AllowsNull,
                        column.KeySequence
                    )).ToList())
                ).ToList();

            // Give the child (column) a reference to its parent (table)
            list.ForEach(
                table => table.TableColumns.ForEach(
                    column => column.Table = table
                )
            );

            return list;
        }

    }
}
