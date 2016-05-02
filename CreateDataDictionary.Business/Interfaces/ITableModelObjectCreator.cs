using System.Collections.Generic;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Creates the DataDictionary objects based on the raw data
    /// </summary>
    public interface ITableModelObjectCreator
    {
        /// <summary>
        /// Transforms the raw SQL data into a form that can be used by the report generation
        /// </summary>
        /// <param name="rawData">The raw data to transform</param>
        /// <returns>The transformed data</returns>
        IEnumerable<TableInfo> TransformRawDataIntoFormattedObjects(IEnumerable<TableColumnInfoRaw> rawData);
    }
}