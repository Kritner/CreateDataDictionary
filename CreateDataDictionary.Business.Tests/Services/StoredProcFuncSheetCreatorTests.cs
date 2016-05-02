using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClosedXML.Excel;
using CreateDataDictionary.Business.Models;
using CreateDataDictionary.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateDataDictionary.Business.Tests.Services
{

    /// <summary>
    /// Tests for StoredProcFuncSheetCreator
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StoredProcFuncSheetCreatorTests
    {

        #region private
        private StoredProcFuncSheetCreator _biz;
        private XLWorkbook _workbook;
        private List<StoredProcFuncInfo> _data;
        #endregion private

        #region Setup
        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _biz = new StoredProcFuncSheetCreator();
            _workbook = new XLWorkbook();

            StoredProcFuncModelObjectCreatorService service = new StoredProcFuncModelObjectCreatorService();

            _data = service
                .TransformRawDataIntoFormattedObjects(
                    DataHelpers.GetSampleObjectInfoRaw()
                )
                .ToList();
        }
        #endregion Setup

        #region Public methods/tests
        /// <summary>
        /// ArgumentNullException is thrown when storedProcFuncs is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoredProcFuncSheetCreator_CreateSheetInWorkbook_ArgumentNullExceptionNullTableInfo()
        {
            // Arrange / Act / Assert
            _biz.CreateSheetInWorkbook(ref _workbook, null);
        }

        /// <summary>
        /// ArgumentException is thrown when storedProcFuncs has 0 records
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoredProcFuncSheetCreator_CreateSheetInWorkbook_ArgumentExceptionZeroCountTableInfo()
        {
            // Arrange / Act / Assert
            _biz.CreateSheetInWorkbook(ref _workbook, new List<StoredProcFuncInfo>());
        }

        /// <summary>
        /// A worksheet is created
        /// </summary>
        [TestMethod]
        public void StoredProcFuncSheetCreator_CreateSheetInWorkbook_WorksheetCreatedWhenDescriptionsMissing()
        {
            // Arrange
            if (_workbook.Worksheets.Count != 0)
                Assert.Fail("{0} worksheet count should have been 0", nameof(_workbook));

            // Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);

            // Assert
            Assert.IsTrue(_workbook.Worksheets.Count == 1);
        }

        /// <summary>
        /// Checks there is a row per "object" for each object, and each parameter for that object.
        /// e.g. if an object has no parameters, it is listed once.  
        /// If it has 2 parameters, it is listed 3 times - 1 for the object itself, and 2 times for the parameters.
        /// </summary>
        [TestMethod]
        public void StoredProcFuncSheetCreator_CreateSheetInWorkbook_RowCountPerObject()
        {
            // Arrange
            int expectedCountNoParameters = 1;
            int countNoParameters = 0;
            string noParametersObjectName = "ObjectNameNoParams";
            int expectedCountTwoParameters = 3;
            int countTwoParameters = 0;
            string twoParametersObjectName = "ObjectNameParams";
            int maxRows = 0; // Cannot be more rows than (test data objects count + parameters count) * 2

            foreach (var obj in _data)
            {
                maxRows++;
                foreach (var param in obj.Parameters)
                    maxRows++;
            }
            maxRows = maxRows * 2;

            // Act
            _biz.CreateSheetInWorkbook(ref _workbook, _data);

            for (int iterator = 1; iterator < maxRows; iterator++)
            {
                if (_workbook.Worksheets.First().Cell(iterator, 1).Value.ToString() == noParametersObjectName)
                    countNoParameters++;
                if (_workbook.Worksheets.First().Cell(iterator, 1).Value.ToString() == twoParametersObjectName)
                    countTwoParameters++;
            }

            // Assert
            Assert.AreEqual(expectedCountNoParameters, countNoParameters, nameof(countNoParameters));
            Assert.AreEqual(expectedCountTwoParameters, countTwoParameters, nameof(countTwoParameters));
        }

        #endregion Public methods/tests

    }
}
