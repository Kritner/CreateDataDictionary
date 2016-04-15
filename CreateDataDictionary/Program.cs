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
            string fileName = Console.ReadLine();

            // Create the report
            DataDictionaryCreationService service = new DataDictionaryCreationService(
                new GetDbTableColumnInfoService(
                    new BaseDatabaseConnection()
                ),
                new DataDictionaryExclusionRulesService(),
                new DataDictionaryObjectCreatorService(),
                new DataDictionaryCreateClosedXMLReport()
            );

            service.Execute(fileName);
            //service.Execute(@"C:\test.xlsx");

            Console.WriteLine("");
        }
    }
}
