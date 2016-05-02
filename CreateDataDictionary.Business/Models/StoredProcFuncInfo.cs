using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataDictionary.Business.Models
{
    /// <summary>
    /// Represents that information related to a database stored procedure or function
    /// </summary>
    public class StoredProcFuncInfo
    {
        #region Properties
        /// <summary>
        /// The object's name
        /// </summary>
        public string ObjectName { get; private set; }
        
        /// <summary>
        /// The object's type
        /// </summary>
        public string ObjectType { get; private set; }
        
        /// <summary>
        /// The parameters associated with the object
        /// </summary>
        public List<StoredProcFuncInfoParams> Parameters { get; private set; }
        #endregion Properties

        #region ctor
        /// <summary>
        /// Constructor - constructs object
        /// </summary>
        /// <param name="objectName">The object name, cannot be null</param>
        /// <param name="objectType">The object type, cannot be null</param>
        /// <param name="parameters">The parameters to the object, can be 0 count</param>
        public StoredProcFuncInfo(string objectName, string objectType, IEnumerable<StoredProcFuncInfoParams> parameters)
        {
            if (string.IsNullOrEmpty(objectName))
                throw new ArgumentNullException(nameof(objectName));
            if (string.IsNullOrEmpty(objectType))
                throw new ArgumentNullException(nameof(objectType));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ObjectName = objectName;
            ObjectType = objectType;
            Parameters = parameters.ToList();
        }
        #endregion ctor
    }
}
