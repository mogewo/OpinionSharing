using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    using System.IO;
    /// <summary>
    /// 簡単に複数ファイルにログ出力ができる
    /// 
    /// </summary>
    public static class L
    {

        static public Dictionary<string, ILogger> logDict { get; private set; }
        static public HashSet<string> logNotCache { get; private set; }

        static private string dirName = "";

        /// <summary>
        /// フォルダ名にする日付、時分秒
        /// </summary>
        static private DateTime dt = new DateTime(0);

        /// <summary> ファイルに書き込みをするために初期化する。
        /// フォルダ名は指定がなければ日付、時分秒
        /// </summary>
        static L()
        {
            /*日付のフォルダ作ってたが、これからはpythonでやることにしよう。
            dt = DateTime.Now;
            string routeDir = dt.ToString("yyyyMMdd_HHmmss");
            Directory.CreateDirectory(routeDir);

            dirName = routeDir;
            */

            logDict = new Dictionary<string, ILogger>();
            logDict.Add("console", new ConsoleLogger());
        }

        static public void gDirectory(string _dirName)
        {

            Directory.CreateDirectory(_dirName);

            dirName = _dirName;

        }

        static public bool MakeUndeclaredName { get; set; }

        static private LoggerList all(){
            LoggerList logs = new LoggerList();
            
            foreach (var key in logDict.Keys)
            {
                if ( ! key.Contains(',') )
                {
                    logs.Add(logDict[key]);
                }
            }

            MakeUndeclaredName = false;

            return logs;
            
        }

        static public ILogger g(string filename)
        {
            //allと指定された場合、すべて。
            if (filename == "all")
            {
                return all();
            }



            //すでにキャッシュがあったら
            if (logDict.ContainsKey(filename))
            {
                //即返す
                return logDict[filename];
            }

            //初めて使う場合
            else{
                //初めて使う場合で、もし複数,が含まれてるような文字列だった場合、複数指定。
                if (filename.Contains(','))
                {
                    String[] filenames = filename.Split(',');

                    //logCache.Add(filename, g(filenames));

                    return g(filenames);
                }
                //初めて使う,を含まない普通の文字列だった場合、
                else
                {
                    //宣言されてないものでも登録するオプションならば、
                    if (MakeUndeclaredName)
                    {
                        //登録して、
                        gDefine(filename);
                        //返す
                        return logDict[filename];
                    }
                    //宣言されてないものは無視する設定ならば
                    else
                    {
                        return new DontLogger();
                    }

                }
            }
        }

        static public ILogger g(string[] filenames)//複数のLoggerを返して、一気に書き込める。
        {
            LoggerList loggers = new LoggerList();

            foreach (var filename in filenames)
            {
                loggers.Add(g(filename.Trim()));
            }

            return loggers;
        }

        static public void gDefine(string filename)
        {

            //すでにキャッシュがあったら
            if (logDict.ContainsKey(filename))
            {
                return ;
            }


            //もし複数,が含まれてるような文字列だった場合、複数指定。
            if (filename.Contains(','))
            {
                String[] filenames = filename.Split(',');

                gDefines(filenames);
            }
            else
            {
                logDict.Add(filename, new FileLogger(dirName + "/" + filename));
            }

        }

        static public void gCancel(string filename)
        {
            if (logDict.ContainsKey(filename))
            {
                logDict[filename].Close();
                logDict.Remove(filename);
            }
            
        }

        static public void gDefines(string[] filenames)//複数のLoggerを返して、一気に書き込める。
        {
            foreach (var filename in filenames)
            {
                gDefine(filename.Trim());
            }
        }

        /// <summary> ファイルロガーをすべてクローズする。 </summary>
        static public void Close()
        {
            foreach (string swName in logDict.Keys)
            {
                logDict[swName].Close();
            }
        }

        /// <summary> ファイルロガーをすべて書き出す。 </summary>
        static public void Flush()
        {
            foreach (string swName in logDict.Keys)
            {
                logDict[swName].Flush();
            }
        }

    }//class
}//Log Def
