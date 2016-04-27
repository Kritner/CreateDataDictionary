using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Interface for transforming raw <see cref="StoredProcFuncInfoRaw"/> into <see cref="StoredProcFuncInfo"/>
    /// </summary>
    public interface IStoredProcFuncModelObjectCreator
    {
        /// <summary>
        /// Transform Raw into heirachical model objects
        /// </summary>
        /// <param name="rawData">The data to transform</param>
        /// <returns>The transformed data</returns>
        IEnumerable<StoredProcFuncInfo> TransformRawDataIntoFormattedObjects(IEnumerable<StoredProcFuncInfoRaw> rawData);
    }
}
