using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataDictionary.Business.Models
{
    /// <summary>
    /// The parameter information for a db object
    /// </summary>
    public class StoredProcFuncInfoParams
    {

        #region Properties
        /// <summary>
        /// The object to which the parameter belongs
        /// </summary>
        public StoredProcFuncInfo ParentObject { get; set; }

        /// <summary>
        /// The parameter's sequence
        /// </summary>
        public int ParameterId { get; private set; }

        /// <summary>
        /// The parameter's name
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// The parameter's data type
        /// </summary>
        public string ParameterDataType { get; private set; }

        /// <summary>
        /// The parameter's length
        /// </summary>
        public int ParameterMaxLength { get; private set; }

        /// <summary>
        /// Is the parameter an out parameter?
        /// </summary>
        public bool IsOutParameter { get; private set; }
        #endregion Properties

        #region ctor
        /// <summary>
        /// Constructor - builds object
        /// </summary>
        /// <param name="parameterId">The parameter ID</param>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="parameterDataType">The parameter data type</param>
        /// <param name="parameterMaxLength">The parameter length</param>
        /// <param name="isOutParameter">Is the parameter an out parameter?</param>
        public StoredProcFuncInfoParams(int parameterId, string parameterName, string parameterDataType, int parameterMaxLength, bool isOutParameter)
        {
            if (parameterName == null)
                throw new ArgumentNullException(nameof(parameterName));
            if (string.IsNullOrEmpty(parameterDataType))
                throw new ArgumentNullException(nameof(parameterDataType));

            ParameterId = parameterId;
            ParameterName = parameterName;
            ParameterDataType = parameterDataType;
            ParameterMaxLength = parameterMaxLength;
            IsOutParameter = isOutParameter;
        }
        #endregion ctor

    }
}
