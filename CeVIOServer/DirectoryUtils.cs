using System.IO;

namespace CeVIOServer
{

    /// <summary>
    /// Directory クラスに関する汎用関数を管理するクラス
    /// https://baba-s.hatenablog.com/entry/2014/06/09/210016 を参照
    /// </summary>
    public static class DirectoryUtils
    {
        /// <summary>
        /// 指定したパスにディレクトリが存在しない場合
        /// すべてのディレクトリとサブディレクトリを作成します
        /// </summary>
        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }
    }
}
