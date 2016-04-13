using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for BaseDatabaseConnection
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseDatabaseConnectionTests
    {

        #region private
        private BaseDatabaseConnection _biz;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _biz = new BaseDatabaseConnection();
        }
        #endregion Setup

        #region Public methods/tests
        [TestMethod]
        public void BaseDatabaseConnection_GetDatabaseConnection_ReturnsIDbConnection()
        {
            // Act
            var results = _biz.GetDatabaseConnection();

            // Assert
            Assert.IsInstanceOfType(results, typeof(IDbConnection), "results interface");
            Assert.IsInstanceOfType(results, typeof(SqlConnection), "results concretion");
        }
        #endregion Public methods/tests
    }
}
