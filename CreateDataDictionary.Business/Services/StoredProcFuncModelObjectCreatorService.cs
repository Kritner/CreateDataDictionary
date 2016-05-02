using System;
using System.Collections.Generic;
using System.Linq;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;


namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Creates <see cref="StoredProcFuncInfo"/> from <see cref="StoredProcFuncInfoRaw"/>
    /// </summary>
    public class StoredProcFuncModelObjectCreatorService : IStoredProcFuncModelObjectCreator
    {
        /// <summary>
        /// Transforms <see cref="StoredProcFuncInfoRaw"/> into <see cref="StoredProcFuncInfo"/>
        /// </summary>
        /// <param name="rawData">The data to transform</param>
        /// <returns>The transformed data</returns>
        public IEnumerable<StoredProcFuncInfo> TransformRawDataIntoFormattedObjects(IEnumerable<StoredProcFuncInfoRaw> rawData)
        {
            if (rawData == null)
                throw new ArgumentNullException(nameof(rawData));

            // Transform raw data into model objects
            List<StoredProcFuncInfo> list = rawData
                .GroupBy(gb => gb.ObjectName)
                .Select(obj => new StoredProcFuncInfo(
                    obj.Key,
                    obj.First().ObjectType,
                    obj
                        .Where(w => w.ParameterId.HasValue)
                        .Select(parameter => new StoredProcFuncInfoParams(
                            (int)parameter.ParameterId,
                            parameter.ParameterName,
                            parameter.ParameterDataType,
                            (int)parameter.ParameterMaxLength,
                            (bool)parameter.IsOutParameter
                        )).ToList())
                ).ToList();

            // Give the child (column) a reference to its parent (table)
            list.ForEach(
                obj => obj.Parameters.ForEach(
                    parameter => parameter.ParentObject = obj
                )
            );

            return list;
        }
    }
}