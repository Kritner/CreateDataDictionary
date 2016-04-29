using System;
using System.Collections.Generic;
using System.Data;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;

namespace CreateDataDictionary
{
    /// <summary>
    /// Get Stored Procedure and Function data from the database.
    /// </summary>
    public class GetDbStoredProcFuncInfo : IGetDbStoredProcFuncInfo
    {
        #region private
        private readonly IBaseDatabaseConnection _iBaseDatabaseConnection;
        #endregion private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iBaseDatabaseConnection">The IBaseDatabaseConnection implementation</param>
        public GetDbStoredProcFuncInfo(IBaseDatabaseConnection iBaseDatabaseConnection)
        {
            if (iBaseDatabaseConnection == null)
                throw new ArgumentNullException(nameof(iBaseDatabaseConnection));

            _iBaseDatabaseConnection = iBaseDatabaseConnection;
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Get stored procedure and function information from the db
        /// </summary>
        /// <returns>Raw info on stored procs and functions</returns>
        public IEnumerable<StoredProcFuncInfoRaw> GetStoredProcFunctionInformation()
        {
            List<StoredProcFuncInfoRaw> list = new List<StoredProcFuncInfoRaw>();

            using (IDbConnection conn = _iBaseDatabaseConnection.GetDatabaseConnection())
            {
                conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = Business.Common.SqlQuery._STORED_PROCEDURE_AND_FUNCTIONS;

                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(
                                new StoredProcFuncInfoRaw(
                                    rdr.GetString(rdr.GetOrdinal("ObjectName")),
                                    rdr.GetString(rdr.GetOrdinal("ObjectType")),
                                    rdr.IsDBNull(rdr.GetOrdinal("ParameterId")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ParameterId")),
                                    rdr.GetString(rdr.GetOrdinal("ParameterName")),
                                    rdr.GetString(rdr.GetOrdinal("ParameterDataType")),
                                    rdr.IsDBNull(rdr.GetOrdinal("ParameterMaxLength")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ParameterMaxLength")),
                                    rdr.IsDBNull(rdr.GetOrdinal("IsOutPutParameter")) ? (bool?)null : rdr.GetBoolean(rdr.GetOrdinal("IsOutPutParameter"))
                                )
                            );
                        }
                    }
                }
            }

            return list;
        }
        #endregion Public methods
    }
}