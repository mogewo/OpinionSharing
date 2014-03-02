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
    static class Program
    {
        static void DoExperiment(
            int EXP_ID, 
            string algoStr, 
            int size, 
            double h_trg, 
            int expd, 
            int envSeed,
            string dirname
        ){
            //ディレクトリを作成
            L.gDirectory(dirname);

            //出力するファイルを宣言
            L.gDefine("roundacc,aveacc");
            
            //条件を表示
            {
                L.g("all").WriteLine("");
                L.g("all").Write("envSeed: " + envSeed);
                L.g("all").Write(" EXP_ID: "+ EXP_ID); //実験IDを
                L.g("all").Write(" Algo: " + algoStr);//アルゴリズムの名前を表示

                L.g("all").Write(" agentnum: " + size);
                L.g("all").Write(" h_trg: " + h_trg);
                L.g("all").Write(" expd: " + expd);

                L.g("roundacc").WriteLine("");
            }




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

            }



            //ファイルに書き込み
            L.Flush();

        }

        static void Main(string[] args)
        {

            /* basic settings */
            // SimpleConsole 0 AAT 100 0.9 8 10 dir

            string algoStr = "NewDontReply";
            int seed = 0;
 
            if(args.Length >= 2){
                try
                {
                    algoStr = args[0];
                    seed = int.Parse(args[1]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("引数のフォーマットが不正です。プログラムを終了します.");
                    return;
                }
            }

            int[] sizeList = {100, 250, 500, 750, 1000, 1250, 1500, 1750, 2000};
            double[] h_trgList = {0.88, 0.9 , 0.92, 0.94, 0.96, 0.98, 1.0};
            int[] expdList = {4, 8, 12};

            string dirname = algoStr + "_" + seed; // AAT_0/aveacc.log AAT_1/aveacc.log ... DontReply_1/roundacc.log DontReply_1/roundacc.log
            //*/

            int EXP_ID = 0;

            foreach (int size in sizeList)
            {
                foreach (double h_trg in h_trgList)
                {
                    foreach (int expd in expdList){
                        DoExperiment(EXP_ID,algoStr, size, h_trg, expd, seed, dirname);
                        EXP_ID++;
                    }
                }

            }
        }
    }
}
