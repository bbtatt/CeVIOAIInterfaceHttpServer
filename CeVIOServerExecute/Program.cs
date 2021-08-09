using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CeVIOServer;
using System.Text.RegularExpressions;

namespace CeVIOServerExecute
{
    class Program
    {
        static void Main(string[] args)
        {
            // 実行部
            string url = UserParam.Default.url;
            string cast = UserParam.Default.cast;
            string userdic_path = UserParam.Default.userdic_path;
            string ebdic_path = UserParam.Default.ebdic_path;
            Console.WriteLine(cast);
            
            CeVIOAIServer.Main(url, cast, userdic_path, ebdic_path);
        }
	}
}
