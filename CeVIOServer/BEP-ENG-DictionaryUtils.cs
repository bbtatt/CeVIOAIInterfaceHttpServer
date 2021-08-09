using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CeVIOServer
{
    public class BEP_ENG
    {
        public static Dictionary<string, string> BEDict;

        public static void Init_Dict(string dic_path)
        {
            BEDict = new Dictionary<string, string>();
            using (FileStream fs = new FileStream(dic_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                try
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        for (int i =0; i<6; i++)
                        {
                            reader.ReadLine();
                        }
                        while (reader.Peek() >= 0)
                        {
                            var line = reader.ReadLine();
                            var line_list = line.Replace('\n', ' ').Split(' ');
                            if (line_list[0].Length > 1 || line_list[0] == "I")
                            {
                                BEDict[line_list[0]] = line_list[1];
                            }
                        }
                    }
                }

                catch
                {
                    fs.Close();
                }
        }

        public static string ReplaceEng2Bep(string text)
        {
            string rtext = text;
            var eword_set = PickupEword(text);
            foreach(string eword in eword_set)
            {
                Console.WriteLine(eword);
                if (BEDict.ContainsKey(eword.ToUpper()))
                {
                    rtext = rtext.Replace(eword, BEDict[eword.ToUpper()]);
                    Console.Write("After EtoB: ");
                    Console.WriteLine(BEDict[eword.ToUpper()]);
                }
            }
            return rtext;

        }

        public static List<string> PickupEword(string text)
        {
            MatchCollection matches = Regex.Matches(text, "([a-zA-Z]+)");
            var eword_enu = matches.Cast<Match>().Select(x => x.Value);
            var eword_list = new HashSet<string>(eword_enu);

            return eword_list.OrderByDescending(value => value.Length).ToList();
        }

    }

}
