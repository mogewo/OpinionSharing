using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpinionSharing;
using OpinionSharing.Agt;
using OpinionSharing.Agt.Factory;
using OpinionSharing.Agt.Algorithm;
using MyRandom;
using GraphTheory.Net;
using OpinionSharing.Env;

using Log;


namespace SimpleConsole
{
    static class Program
    {
        static void DoExperiment(
            int EXP_ID, 
            string algoStr, 
            string netStr,
            int size, 
            double h_trg, 
            int expd, 
            int envSeed,//環境のシード            
            int linkNum,
            double cluster,//クラスタ係数
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
                L.g("all").Write(" linkNum: " + linkNum);
                L.g("all").Write(" cluster: " + cluster);//クラスタ係数
                L.g("all").Write(" h_trg: " + h_trg);
                L.g("all").Write(" expd: " + expd);

                L.g("roundacc").WriteLine("");
            }


            {//実験一回し
                Experiment exp = new Experiment(150,2000);//150ラウンド，2000ステップの実験

                exp.SetEnvsetSeed(envSeed);
                exp.SetFactSeed(0);               

                NetworkGenerator generator = null;
                //環境を作る
                if (netStr == "WS")
                {
                    generator = new WSmodelNetworkGenerator(size, expd, 0.12);
                }
                else if (netStr == "BA")
                {
                    generator = new BAModelNetworkGenerator(size, 4,2);
                }
                else if (netStr == "Random")
                {
                    generator = new RandomNetworkGenerator(size,16, 0.4);
                }
                //リーダーネットワークの導入　2014_0707追加
                else if (netStr == "Leader")
                {
                    generator = new LeaderNetworkGenerator(size, expd, 0.12, linkNum);
                }


                //シェルを作る
                generator.NodeCreate += ()=> new AgentIO();

                //generateしてくれる。
                OSEnvironment env = new OSEnvironment(generator, (int)(size * 0.05));

                //エージェントに脳みそをいれてやる
                foreach (AgentIO agentIO in env.Network.Nodes)
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

            //string algoStr = "NewDontReply";
            //string netStr = "WS";
            string algoStr = "AAT";//リーダーネットワークでAAT　2014_0707追加
            string netStr = "Leader";
            int envseed = 0;
            double cluster = 0;
 
            if(args.Length >= 3){
                try
                {
                    //外から読めるのは，アルゴリズム，乱数シード，ネットワークトポロジーのみ．
                    algoStr = args[0];
                    envseed = int.Parse(args[1]); //今は使えないよ！
                    netStr = args[2];
                }
                catch (FormatException)
                {
                    Console.WriteLine("引数のフォーマットが不正です。プログラムを終了します.");
                    return;
                }
            }

 //           int[] sizeList = { 100, 250, 500, 750, 1000, 1250, 1500, 1750, 2000 };
            int[] sizeList = { 100};
            int[] linkNumList = { 99 ,90, 80, 70, 60, 50, 40, 30, 20, 10, 0 };
            //double[] h_trgList = {0.88, 0.9 , 0.92, 0.94, 0.96, 0.98, 1.0};
            double[] h_trgList = {0.94};
            int[] expdList = {8};

            //string dirname = netStr + "_" + algoStr + "_" + seed; // AAT_0/aveacc.log AAT_1/aveacc.log ... DontReply_1/roundacc.log DontReply_1/roundacc.log
            //*/

            int EXP_ID = 0;


            for (int i = 0; i < 10; i++)
            {
                envseed = i;

                string dirname = netStr + "_" + algoStr + "_" + envseed + "_";

                foreach (int size in sizeList)
                {
                    foreach (double h_trg in h_trgList)
                    {
                        foreach (int expd in expdList){
                            foreach (int linkNum in linkNumList)
                            {
                                DoExperiment(EXP_ID, algoStr, netStr, size, h_trg, expd, envseed, linkNum, cluster, dirname);
                                EXP_ID++;  
                            }                                           
                        }
                    }

                }
                
            }
        }
    }
}
