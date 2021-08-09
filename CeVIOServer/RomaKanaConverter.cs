using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CeVIOServer
{
    /// <summary>
    ///  https://mohayonao.hatenadiary.org/entry/20091129/1259505966 を参考に実装
    /// </summary>
    public class RomajiKanaConverter
    {
        private Dictionary<string, string> RomajiKanaDict;
        public RomajiKanaConverter() 
        {
            var instanst_dict = new Dictionary<string, string>() {
                {"a", "ア"}, {"i", "イ"}, {"u", "ウ"}, {"e", "エ"}, { "o", "オ"},
                {"ka", "カ"}, {"ki" , "キ"}, {"ku", "ク"}, {"ke", "ケ"}, { "ko", "コ"},
                {"sa", "サ"}, {"shi", "シ"}, {"su" , "ス"}, {"se" , "セ"}, { "so" , "ソ"},
                { "ta" , "タ"}, {"chi", "チ"}, {"tu" , "ツ"}, {"te" , "テ"}, { "to" , "ト"},
                { "na" , "ナ"}, {"ni" , "ニ"}, {"nu" , "ヌ"}, {"ne" , "ネ"}, { "no" , "ノ"},
                { "ha" , "ハ"}, {"hi" , "ヒ"}, {"fu" , "フ"}, {"he" , "ヘ"}, { "ho" , "ホ"},
                { "ma" , "マ"}, {"mi" , "ミ"}, {"mu" , "ム"}, {"me" , "メ"}, { "mo" , "モ"},
                { "ya" , "ヤ"}, {"yu" , "ユ"}, { "yo" , "ヨ"},
                { "ra" , "ラ"}, {"ri" , "リ"}, {"ru" , "ル"}, {"re" , "レ"}, { "ro" , "ロ"},
                { "wa" , "ワ"}, {"wo" , "ヲ"}, {"n"  , "ン"}, { "vu" , "ヴ"},
                { "ga" , "ガ"}, {"gi" , "ギ"}, {"gu" , "グ"}, {"ge" , "ゲ"}, { "go" , "ゴ"},
                { "za" , "ザ"}, {"ji" , "ジ"}, {"zu" , "ズ"}, {"ze" , "ゼ"}, { "zo" , "ゾ"},
                { "da" , "ダ"}, {"di" , "ヂ"}, {"du" , "ヅ"}, {"de" , "デ"}, { "do" , "ド"},
                { "ba" , "バ"}, {"bi" , "ビ"}, {"bu" , "ブ"}, {"be" , "ベ"}, { "bo" , "ボ"},
                { "pa" , "パ"}, {"pi" , "ピ"}, {"pu" , "プ"}, {"pe" , "ペ"}, { "po" , "ポ"},

                { "kya", "キャ"}, {"kyi", "キィ"}, {"kyu", "キュ"}, {"kye", "キェ"}, { "kyo", "キョ"},
                { "gya", "ギャ"}, {"gyi", "ギィ"}, {"gyu", "ギュ"}, {"gye", "ギェ"}, { "gyo", "ギョ"},
                { "sha", "シャ"}, {"shu", "シュ"}, {"she", "シェ"}, { "sho", "ショ"},
                { "ja" , "ジャ"}, {"ju" , "ジュ"}, {"je" , "ジェ"}, { "jo" , "ジョ"},
                { "cha", "チャ"}, {"chu", "チュ"}, {"che", "チェ"}, { "cho", "チョ"},
                { "dya", "ヂャ"}, {"dyi", "ヂィ"}, {"dyu", "ヂュ"}, {"dhe", "デェ"}, { "dyo", "ヂョ"},
                { "nya", "ニャ"}, {"nyi", "ニィ"}, {"nyu", "ニュ"}, {"nye", "ニェ"}, { "nyo", "ニョ"},
                { "hya", "ヒャ"}, {"hyi", "ヒィ"}, {"hyu", "ヒュ"}, {"hye", "ヒェ"}, { "hyo", "ヒョ"},
                { "bya", "ビャ"}, {"byi", "ビィ"}, {"byu", "ビュ"}, {"bye", "ビェ"}, { "byo", "ビョ"},
                { "pya", "ピャ"}, {"pyi", "ピィ"}, {"pyu", "ピュ"}, {"pye", "ピェ"}, { "pyo", "ピョ"},
                { "mya", "ミャ"}, {"myi", "ミィ"}, {"myu", "ミュ"}, {"mye", "ミェ"}, { "myo", "ミョ"},
                { "rya", "リャ"}, {"ryi", "リィ"}, {"ryu", "リュ"}, {"rye", "リェ"}, { "ryo", "リョ"},
                { "fa" , "ファ"}, {"fi" , "フィ"}, {"fe" , "フェ"}, { "fo" , "フォ"},
                { "wi" , "ウィ"}, { "we" , "ウェ"},
                { "va" , "ヴァ"}, {"vi" , "ヴィ"}, {"ve" , "ヴェ"}, { "vo" , "ヴォ"},
                { "kwa", "クァ"}, {"kwi", "クィ"}, {"kwu", "クゥ"}, {"kwe", "クェ"}, { "kwo", "クォ"},
                { "kha", "クァ"}, {"khi", "クィ"}, {"khu", "クゥ"}, {"khe", "クェ"}, { "kho", "クォ"},
                { "gwa", "グァ"}, {"gwi", "グィ"}, {"gwu", "グゥ"}, {"gwe", "グェ"}, { "gwo", "グォ"},
                { "gha", "グァ"}, {"ghi", "グィ"}, {"ghu", "グゥ"}, {"ghe", "グェ"}, { "gho", "グォ"},
                { "swa", "スァ"}, {"swi", "スィ"}, {"swu", "スゥ"}, {"swe", "スェ"}, { "swo", "スォ"},
                { "zwa", "ズヮ"}, {"zwi", "ズィ"}, {"zwu", "ズゥ"}, {"zwe", "ズェ"}, { "zwo", "ズォ"},
                { "twa", "トァ"}, {"twi", "トィ"}, {"twu", "トゥ"}, {"twe", "トェ"}, { "two", "トォ"},
                { "dwa", "ドァ"}, {"dwi", "ドィ"}, {"dwu", "ドゥ"}, {"dwe", "ドェ"}, { "dwo", "ドォ"},
                { "mwa", "ムヮ"}, {"mwi", "ムィ"}, {"mwu", "ムゥ"}, {"mwe", "ムェ"}, { "mwo", "ムォ"},
                { "bwa", "ビヮ"}, {"bwi", "ビィ"}, {"bwu", "ビゥ"}, {"bwe", "ビェ"}, { "bwo", "ビォ"},
                { "pwa", "プヮ"}, {"pwi", "プィ"}, {"pwu", "プゥ"}, {"pwe", "プェ"}, { "pwo", "プォ"},
                { "phi", "プィ"}, {"phu", "プゥ"}, {"phe", "プェ"}, { "pho", "フォ"},
                {"si" , "シ"  }, {"ti" , "チ"  }, {"hu" , "フ" }, {"zi", "ジ" },
                { "sya", "シャ"}, {"syu", "シュ"}, {"syo", "ショ" },
                { "tya", "チャ"}, {"tyu", "チュ"}, {"tyo", "チョ" },
                { "cya", "チャ"}, {"cyu", "チュ"}, {"cyo", "チョ" },
                { "jya", "ジャ"}, {"jyu", "ジュ"}, {"jyo", "ジョ"}, {"pha", "ファ" },
                { "qa" , "クァ"}, {"qi" , "クィ"}, {"qu" , "クゥ"}, {"qe" , "クェ"}, {"qo", "クォ" },

                { "ca" , "カ"}, {"ci", "シ"}, {"cu", "ク"}, {"ce", "セ"}, {"co", "コ" },
                { "la" , "ラ"}, {"li", "リ"}, {"lu", "ル"}, {"le", "レ"}, {"lo", "ロ" },

                { "mb" , "ム"}, {"py", "パイ"}, {"tho",  "ソ"}, {"thy", "ティ"}, {"oh", "オウ" },
                { "by", "ビィ"}, {"cy", "シィ"}, {"dy", "ディ"}, {"fy", "フィ"}, {"gy", "ジィ"},
                { "hy", "シー"}, {"ly", "リィ"}, {"ny", "ニィ"}, {"my", "ミィ"}, {"ry", "リィ" },
                { "ty", "ティ"}, {"vy", "ヴィ"}, {"zy", "ジィ" },

                { "b", "ブ"}, {"c", "ク"}, {"d", "ド"}, {"f", "フ"  }, {"g", "グ"}, {"h", "フ"}, {"j", "ジ" },
                { "k", "ク"}, {"l", "ル"}, {"m", "ム"}, {"p", "プ"  }, {"q", "ク"}, {"r", "ル"}, {"s", "ス" },
                { "t", "ト"}, {"v", "ヴ"}, {"w", "ゥ"}, {"x", "クス"}, {"y", "ィ"}, {"z", "ズ" },
                { "'s", "ズ" }

            };
            RomajiKanaDict = instanst_dict.OrderByDescending(pair => pair.Key.Length).ToDictionary(pair => pair.Key, pair => pair.Value);
        }



        public string Romaji2Kana(string text)
        {
            string rtext;

            rtext = text.ToLower();
            rtext = Regex.Replace(rtext, "m(<(h1(b|p))>)(<(h2[aiueo])>)", @"ン</\1></\2>");
            rtext = Regex.Replace(rtext, @"([bcdfghjklmpqrstvwxyz])\1", @"ッ</\1>");
            rtext = Regex.Replace(rtext, @"([aiueo])\1", @"</\1>ー");

            return Regex.Replace(rtext, String.Join("|", RomajiKanaDict.Keys), match => RomajiKanaDict[match.Value]);
        }

    }
}
