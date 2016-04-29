using System;

namespace CreateDataDictionary.Business.Models
{
    /// <summary>
    /// Raw data from the database describing stored procedures and functions
    /// </summary>
    public class StoredProcFuncInfoRaw
    {

        #region Properties
        /// <summary>
        /// The object's name
        /// </summary>
        public string ObjectName { get; private set; }
        /// <summary>
        /// The object's type (stored proc or function)
        /// </summary>
        public string ObjectType { get; private set; }
        /// <summary>
        /// The parameter sequence
        /// </summary>
        /// <remarks>
        /// <see cref="null"/> when not a parameter
        /// </remarks>
        public int? ParameterId { get; private set; }
        /// <summary>
        /// The parameter's name
        /// </summary>
        /// <remarks>
        /// <see cref="null"/> when not a parameter
        /// </remarks>
        public string ParameterName { get; private set; }
        /// <summary>
        /// The parameter's data type
        /// </summary>
        /// /// <remarks>
        /// <see cref="null"/> when not a parameter
        /// </remarks>
        public string ParameterDataType { get; private set; }
        /// <summary>
        /// The parameter's max length
        /// </summary>
        /// /// <remarks>
        /// <see cref="null"/> when not a parameter
        /// </remarks>
        public int? ParameterMaxLength { get; private set; }
        /// <summary>
        /// Is the parameter an out parameter?
        /// </summary>
        /// /// <remarks>
        /// <see cref="null"/> when not a parameter
        /// </remarks>
        public bool? IsOutParameter { get; private set; }
        #endregion Properties

        #region ctor
        /// <summary>
        /// Constructor - builds object
        /// </summary>
        /// <param name="objectName">The object name, cannot be null/empty</param>
        /// <param name="objectType">The object type, cannot be null/empty</param>
        /// <param name="parameterId">The parameter id</param>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="parameterDataType">The parameter data type</param>
        /// <param name="parameterMaxLength">The parameter max length</param>
        /// <param name="isOutPutParameter">Is the parameter an out parameter?</param>
        public StoredProcFuncInfoRaw(
            string objectName,
            string objectType,
            int? parameterId,
            string parameterName,
            string parameterDataType,
            int? parameterMaxLength,
            bool? isOutPutParameter
        )
        {
            if (string.IsNullOrEmpty(objectName))
                throw new ArgumentNullException(nameof(objectName));
            if (string.IsNullOrEmpty(objectType))
                throw new ArgumentNullException(nameof(objectType));

            ObjectName = objectName;
            ObjectType = objectType;
            ParameterId = parameterId;
            ParameterName = parameterName;
            ParameterDataType = parameterDataType;
            ParameterMaxLength = parameterMaxLength;
            IsOutParameter = isOutPutParameter;
        }
        #endregion ctor
    }
}