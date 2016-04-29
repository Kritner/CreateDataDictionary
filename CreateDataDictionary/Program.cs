using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDataDictionary.Business.Services;

namespace CreateDataDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a path, filename, and extension (.xlsx) for where to write the data dictionary.  Then press <ENTER>");
            //string fileName = Console.ReadLine();

            // Create the report
            DataDictionaryCreationService service = new DataDictionaryCreationService(
                new DataDictionaryTableDataProvider(
                    new GetDbTableColumnInfoService(
                        new BaseDatabaseConnection()
                    ),
                    new TableExclusionRulesService(),
                    new TableModelObjectCreatorService()
                ),
                new DataDictionaryStoredProcFuncDataProvider(
                    new GetDbStoredProcFuncInfo(
                        new BaseDatabaseConnection()
                    ),
                    new StoredProcFuncModelObjectCreatorService()
                ),
                new DataDictionaryCreateClosedXMLReport(
                    new MissingDescriptionsSheetCreator(),
                    new StoredProcFuncSheetCreator()
                )
            );

            //service.Execute(fileName);
            service.Execute(@"C:\test.xlsx");

            Console.WriteLine("");
        }
    }
}
