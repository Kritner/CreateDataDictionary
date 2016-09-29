using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Interfaces;
using CreateDataDictionary.Business.Models;

namespace CreateDataDictionary.Business.Services
{
    
    /// <summary>
    /// Get the table, table description, column, column description, key, and type information from the database
    /// </summary>
    public class GetDbTableColumnInfoService : IGetDbTableColumnInfo
    {
        #region Private
        private readonly IBaseDatabaseConnection _iBaseDatabaseConnection;
        #endregion Private

        #region ctor
        /// <summary>
        /// Constructor - takes in dependencies
        /// </summary>
        /// <param name="iBaseDatabaseConnection">The IBaseDatabaseConnection implementation</param>
        public GetDbTableColumnInfoService(IBaseDatabaseConnection iBaseDatabaseConnection)
        {
            if (iBaseDatabaseConnection == null)
                throw new ArgumentNullException(nameof(iBaseDatabaseConnection));

            _iBaseDatabaseConnection = iBaseDatabaseConnection;
        }
        #endregion ctor

        #region Public methods
        /// <summary>
        /// Get the data from the database, and return as an <see cref="IEnumerable{T}"/> of <see cref="TableColumnInfoRaw"/>
        /// </summary>
        /// <returns>IEnumerable of TableColumnInfoRaw</returns>
        public IEnumerable<TableColumnInfoRaw> GetTableColumnInformation()
        {
            List<TableColumnInfoRaw> list = new List<TableColumnInfoRaw>();

            using (IDbConnection conn = _iBaseDatabaseConnection.GetDatabaseConnection())
            {
                conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = Common.SqlQuery._TABLE_COLUMN_DESCRIPTIONS;

                    using (IDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(
                                new TableColumnInfoRaw(
                                    rdr.GetString(rdr.GetOrdinal("TABLE_NAME")),
                                    rdr.GetString(rdr.GetOrdinal("TABLE_DESCRIPTION")),
                                    rdr.GetString(rdr.GetOrdinal("COLUMN_NAME")),
                                    rdr.GetString(rdr.GetOrdinal("COLUMN_DESCRIPTION")),
                                    rdr.GetString(rdr.GetOrdinal("ColType")),
                                    rdr.IsDBNull(rdr.GetOrdinal("Length")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("Length")),
                                    rdr.GetString(rdr.GetOrdinal("DefaultValue")),
                                    rdr.GetBoolean(rdr.GetOrdinal("Nulls")),
                                    rdr.IsDBNull(rdr.GetOrdinal("KeySeq")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("KeySeq")),
                                    rdr.GetDateTime(rdr.GetOrdinal("modify_date"))
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
