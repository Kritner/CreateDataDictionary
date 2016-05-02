using System;
using System.Collections.Generic;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    /// <summary>
    /// Implementation 
    /// </summary>
    public class DataDictionaryStoredProcFuncDataProvider : IDataDictionaryStoredProcFuncDataProvider
    {
        #region private
        private IGetDbStoredProcFuncInfo _iGetDbStoredProcFuncInfo;
        private IStoredProcFuncModelObjectCreator _iStoredProcFuncModelObjectCreator;
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iGetDbStoredProcFuncInfo">The IGetDbStoredProcFuncInfo implementation</param>
        /// <param name="iStoredProcFuncModelObjectCreator">The IStoredProcFuncModelObjectCreator implementation</param>
        public DataDictionaryStoredProcFuncDataProvider(IGetDbStoredProcFuncInfo iGetDbStoredProcFuncInfo, IStoredProcFuncModelObjectCreator iStoredProcFuncModelObjectCreator)
        {
            if (iGetDbStoredProcFuncInfo == null)
                throw new ArgumentNullException(nameof(iGetDbStoredProcFuncInfo));
            if (iStoredProcFuncModelObjectCreator == null)
                throw new ArgumentNullException(nameof(iStoredProcFuncModelObjectCreator));

            _iGetDbStoredProcFuncInfo = iGetDbStoredProcFuncInfo;
            _iStoredProcFuncModelObjectCreator = iStoredProcFuncModelObjectCreator;
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Gets and returns data based on stored procs/functions within the database
        /// </summary>
        /// <returns>IEnumerable of StoredProcFuncInfo</returns>
        public IEnumerable<StoredProcFuncInfo> Execute()
        {
            // Get raw data
            var rawStoredProcFuncData = _iGetDbStoredProcFuncInfo.GetStoredProcFunctionInformation();

            return _iStoredProcFuncModelObjectCreator.TransformRawDataIntoFormattedObjects(rawStoredProcFuncData);
        }
        #endregion Public methods
    }
}