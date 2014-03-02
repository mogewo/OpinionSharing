using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpinionSharing;
using OpinionSharing.Agt;
using OpinionSharing.Agt.Factory;
using OpinionSharing.Agt.Algorithm;
using OpinionSharing.Net;
using OpinionSharing.Util;
using OpinionSharing.Env;

using Log;


namespace SimpleConsole
{
    /*
    static class Program
    {
        static void DoExperiment(
            int EXP_ID, 
            string algoStr, 
            int size, 
            double h_trg, 
            int expd, 
            int seedStart,
            int envSeedNum, 
            string dirName
        ){
            //ディレクトリを作成
            L.gDirectory(dirName);
            //出力するファイルを宣言
            L.gDefine("stepbelief,roundacc,aveacc");

            //シードのリストを作成
            var envSeedList = Enumerable.Range(seedStart, envSeedNum);//seedStart は通常は0

            
            //条件を表示
            {
                L.g("all").WriteLine("");
                L.g("all").Write(string.Format("EXP_ID: {0}", EXP_ID++)); //実験IDを
                L.g("all").Write(" Algo: " + algoStr);//アルゴリズムの名前を表示

                L.g("all").Write(" agentnum: " + size);
                L.g("all").Write(" h_trg: " + h_trg);
                L.g("all").Write(" expd: " + expd);
                L.g("all").Write(" envSdN: " + envSeedNum);

                L.g("all").WriteLine("");
            }


            //シードの平均をとるため
            double[] aveSeedResult = new double[3];

            foreach (int envSeed in envSeedList)//環境のシード
            {
                L.g("roundacc,time,console,stepbelief").Write("\nenvSeed: " + envSeed + " ");

                {//実験一回し
                    Experiment exp = new Experiment(150,3000);//150ラウンド，3000ステップの実験

                    exp.SetEnvsetSeed(envSeed);
                    exp.SetFactSeed(0);

                    //環境を作る
                    NetworkGenerator generator = new SmallworldRingNetworkGenerator(size, expd, 0.12);

                    //シェルを作る
                    generator.NodeCreate += ()=> new AATBasedAgentIO();

                    //generateしてくれる。
                    OSEnvironment env = new OSEnvironment(generator, (int)(size * 0.05));

                    //エージェントに脳みそをいれてやる
                    foreach (AATBasedAgentIO agentIO in env.Network.Nodes)
                    {
                        agentIO.Algorithm = (AlgorithmFactory.CreateAlgorithm(algoStr, h_trg));
                    }
                    

                    //ファイルにネットワークの情報を表示
                    L.g("env").WriteLine("" + env);
                    L.g("env").Flush();

                    exp.Environment = env;

                    //実験を実行
                    var expResult = exp.Run();

                    {//平均をとるために和を計算
                        aveSeedResult[0] += expResult.Correct;
                        aveSeedResult[1] += expResult.Incorrect;
                        aveSeedResult[2] += expResult.Undeter;
                    }
                }
            }//forenvseed

            { //平均をとる
                aveSeedResult[0] /= envSeedNum;
                aveSeedResult[1] /= envSeedNum;
                aveSeedResult[2] /= envSeedNum;
            }

            L.g("aveacc,console").WriteLine(
                string.Format("SeedAverage: correct: {0} incorrect: {1} undeter: {2}",
                aveSeedResult[0], aveSeedResult[1], aveSeedResult[2]));

            //ファイルに書き込み
            L.Flush();

        }

        static void Main(string[] args)
        {

            // SimpleConsole 0 AAT 100 0.9 8 10 dir

            int EXP_ID = 0;
            string algoStr = "AAT";
            int size = 100;
            double h_trg = 0.9;
            int expd = 8;
            int seedStart = 0;
            int envSeedNum = 3;
            string dirName = "dir";


            if(args.Length >= 7){
                try
                {
                    EXP_ID = int.Parse(args[0]);
                    algoStr = args[1];
                    size = int.Parse(args[2]);
                    h_trg = double.Parse(args[3]);
                    expd = int.Parse(args[4]);
                    envSeedNum = int.Parse(args[5]);
                    dirName = args[6];
                }
                catch (FormatException)
                {
                    Console.WriteLine("引数のフォーマットが不正です。プログラムを終了します.");
                    return;
                }
            }

            DoExperiment(EXP_ID, algoStr, size, h_trg, expd, seedStart, envSeedNum, dirName);
        }
    }
*/
}
