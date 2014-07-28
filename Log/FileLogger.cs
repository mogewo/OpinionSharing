using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    /// <summary>
    /// ファイル書き込みクラス。
    /// 
    /// version 2013/11/4
    /// </summary>

    public class FileLogger : ILogger
    {

        /// <summary>csvファイル書き込みストリーム</summary>
        public StreamWriter Writer { get; private set; }

        /// <summary> csvファイルへの書き込みの呼び出し </summary>
        /// <param name="path"></param>
        /// <param name="header"></param>
        /// <param name="isAppend"></param>

        public FileLogger(string path)
        {
            // ファイル名に許されない文字列のチェック
            //char[] invalidch = Path.GetInvalidFileNameChars();
            //if (path.LastIndexOfAny(invalidch) >= 0)
            //{
            //    Console.WriteLine("Logger ファイル名に許されない文字列が発生しました。 '_' に置き換えます。");
            //    foreach (char c in invalidch)
            //        path = path.Replace(c, '_');
            //}

            //ディレクトリを作る必要があるのは、ディレクトリが存在しないとき

            path += ".log";

            if (!System.IO.Directory.Exists(path) && System.IO.Path.GetDirectoryName(path) != "")
                //パスが存在しなくて、ディレクトリが必要なとき
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            }

            Writer = new StreamWriter(path, true, System.Text.Encoding.GetEncoding("shift_jis"));

            Writer.NewLine = "\n";

//            if (!header.Equals("")) { Writer.WriteLine(header); }
        }

        public void WriteLine(string str) { Writer.WriteLine(str); }
        public void Write(string str) { Writer.Write(str); }

        /*
        /// <summary>
        /// 配列の中身を1行に書き込む。区切り文字の指定必要あり。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="strArr"></param>
        /// <param name="mode"> 区切り文字の指定 </param>
        public void WriteRow(string[] strArr, string delim)
        {
            for (int i = 0; i < strArr.Length; i++)
            {
                Writer.Write(strArr[i] + delim);
            }
            Writer.WriteLine();
        }


        /// <summary>
        /// 配列の中身を1行に書き込む。区切り文字の指定必要あり。
        /// </summary>
        /// <param name="strList"></param>
        /// <param name="mode"> 区切り文字の指定 </param>
        public void WriteRow(IEnumerable<string> strList, string delim)
        {
            foreach (var str in strList)
            {
                Writer.Write(str + delim);
            }
            Writer.WriteLine();
        }
        */

        /// <summary>
        /// クローズ
        /// </summary>
        public void Close() { Writer.Close(); }

        public void Flush() { Writer.Flush(); }
    }


}