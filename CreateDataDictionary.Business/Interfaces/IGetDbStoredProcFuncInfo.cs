using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Interfaces
{
    /// <summary>
    /// Interface for retrieving Stored procedure and function information from the database
    /// </summary>
    public interface IGetDbStoredProcFuncInfo
    {
        IEnumerable<StoredProcFuncInfoRaw> GetStoredProcFunctionInformation();
    }
}
