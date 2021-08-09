using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace CeVIOServer
{
    public class CeVIOTalkDictUtils
    {
        public static Dictionary<string, string> UserDict;

        public static void Init_Dict(string dic_path)
        {
            UserDict = new Dictionary<string, string>();
            using (FileStream fs = new FileStream(dic_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                try
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        while (reader.Peek() >= 0)
                        {
                            var line = reader.ReadLine();
                            var line_list = line.Replace('\n', ' ').Split(' ');
                            UserDict[Strings.StrConv(line_list[0], VbStrConv.Narrow)] = line_list[1].Trim('[').Trim(']');
                        }
                    }
                }

                catch
                {
                    fs.Close();
                }
            // 文字数の降順にソート
            UserDict = UserDict.OrderByDescending(pair => pair.Key.Length).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static string ReplaceUserDict(string text)
        {
            return Regex.Replace(Strings.StrConv(text, VbStrConv.Narrow), String.Join("|", UserDict.Keys), match => UserDict[match.Value]);

        }
    }
}
