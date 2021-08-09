using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CeVIO.Talk.RemoteService2;

namespace CeVIOServer
{
    public class CeVIODriver
    {

        private Talker2 talker;
        string Wav_path { get; } = Path.GetTempPath() + @"CeVIO_WAV\";
        string Wav_file { get => Wav_path + "temp_file.wav"; }
        private string temp_path = Path.GetTempPath();
        string DefaultConfigPath { get => temp_path.Substring(0, temp_path.Length - 19) + @"CeVIO\" ; }

        private RomajiKanaConverter rkconverter;
        public CeVIODriver(string cast, string userdic_path, string dic_path)
        {
            ServiceControl2.StartHost(false);
            talker = new Talker2();
			talker.Cast = cast;
            DirectoryUtils.SafeCreateDirectory(Wav_path);
            talker.Components[0].Value = 60;
            talker.Components[1].Value = 50;
            talker.Components[2].Value = 0;
            talker.Components[3].Value = 40;

            // 辞書の読み込み
            if (dic_path != "")
            {
                BEP_ENG.Init_Dict(dic_path);
            }
            else
            {
                BEP_ENG.Init_Dict(DefaultConfigPath + "bep-eng.dic");
            }

            if (userdic_path != "")
            {
                CeVIOTalkDictUtils.Init_Dict(userdic_path);
            }
            else
            {
                CeVIOTalkDictUtils.Init_Dict(DefaultConfigPath + "TalkDictionary.dic");
            }

            rkconverter = new RomajiKanaConverter();
        }

        public string CreateWAV(string text)
        {
            string rtext = CeVIOTalkDictUtils.ReplaceUserDict(text);
            Console.Write("After UserDict: ");
            Console.WriteLine(rtext);
            rtext = BEP_ENG.ReplaceEng2Bep(rtext);
            rtext = rkconverter.Romaji2Kana(rtext);
            Console.Write("After Romaji2Kana: ");
            Console.WriteLine(rtext);
            rtext = rtext.Replace(" ", "");
            if (talker.OutputWaveToFile(rtext, Wav_file))
            {
                return Wav_file;
            }
            else
            {
                Console.WriteLine("Ouch!");
                return "";
            }
        }

        ~CeVIODriver()
        {
            ServiceControl2.CloseHost();
        }
    }
}
