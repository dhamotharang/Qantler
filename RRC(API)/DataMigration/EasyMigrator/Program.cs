using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyMiger
{
    class Program
    {
        static clsReturn isGet;
        static void Main(string[] args)
        {
            Version cuVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Console.Title = "EasyMigrator " + Convert.ToString(cuVersion.Major) + "." + Convert.ToString(cuVersion.Minor);
            // + "." + Convert.ToString(cuVersion.Build) + "." + Convert.ToString(cuVersion.MinorRevision)
            clsReturn isGet = new clsReturn();
            isGet = clsCommon.validateXMLFile();
            if (!isGet.result)
            {
                Console.ReadLine();
                return;
            }
            isGet = clsCommon.validateSharePointSite();
            if (!isGet.result)
            {
                Console.ReadLine();
                return;
            }
            isGet = clsCommon.sqlServerValidation();
            if (!isGet.result)
            {
                Console.ReadLine();
                return;
            }
            isGet = clsCommon.attachmentPathValidation();
            if (!isGet.result)
            {
                Console.ReadLine();
                return;
            }
            isGet = clsCommon.validateRunFromCache();
            if (!isGet.result)
            {
                Console.ReadLine();
                return;
            }
            isGet = clsCommon.processList();
            Console.ReadLine();
        }
    }
}
