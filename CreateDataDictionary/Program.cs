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
            GetDbTableColumnInfoService service = new GetDbTableColumnInfoService(
                new BaseDatabaseConnection()
            );

            var results = service.GetTableColumnInformation();
            Console.WriteLine("");
        }
    }
}
