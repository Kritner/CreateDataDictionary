using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;

namespace CreateDataDictionary.Business.Services
{
    public class StoredProcFuncSheetCreator : IStoredProcFuncSheetCreator
    {
        #region const
        const int _NUMBER_OF_COLUMNS_WIDE = 5;
        #endregion const

        #region private
        private List<StoredProcFuncInfo> _data;
        #endregion private

        /// <summary>
        /// Create sheet describing Stored Procedures and functions
        /// </summary>
        /// <param name="workbook">The workbook to create the sheet in</param>
        /// <param name="storedProcFuncs">The stored procedures and functions to document</param>
        public void CreateSheetInWorkbook(ref XLWorkbook workbook, IEnumerable<StoredProcFuncInfo> storedProcFuncs)
        {
            if (storedProcFuncs == null)
                throw new ArgumentNullException(nameof(storedProcFuncs));
            if (storedProcFuncs.Count() == 0)
                throw new ArgumentException(nameof(storedProcFuncs));

            _data = storedProcFuncs.ToList();
        }
    }
}