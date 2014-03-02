using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpinionSharing;

using System.IO;
using Log;

namespace OpinionSharingConsole
{
    using OpinionSharing.Net;
    using OpinionSharing.Agt;
    using OpinionSharing.Agt.Original;
    using OpinionSharing.Subject;
    using OpinionSharing.Env;
    using OpinionSharing.Util;

    using System.Diagnostics;

    class Program
    {
        static void AutoExperiment()
        {
            //ディレクトリを作成
            L.gDirectory(DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            L.gDefine("stepbelief,roundacc,aveacc,time,env");

            //集団規模のリスト

            //int[] sizeList = { 100, 250, 500, 750, 1000, 1500, 2000, 2500 };
            int[] sizeList = {100, 1000};

            //h_trgの種類
            double[] h_trgList = { 0.9, 0.94, 0.98, 1.0 };

            //expected d <d> 
            double[] expdList = { 4, 6, 8, 10, 12 };


            //シードの種類の個数
            int envSeedNum = 10;

            var envSeedList = Enumerable.Range(0,envSeedNum);

            NodeCreaterCreater AATCC = (double h_trg) => (() => new AAT(h_trg));
            NodeCreaterCreater DontReplyCC = (double h_trg) => (() => new DontReply(h_trg));
            NodeCreaterCreater NoMoreBeliefCC = (double h_trg) => (() => new NoMoreBelief(h_trg));
            NodeCreaterCreater EatingWordsCC = (double h_trg) => (() => new EatingWords(h_trg));
            NodeCreaterCreater BelieveOnlySensorCC = (double h_trg) => (() => new BelieveOnlySensor(h_trg));

            Dictionary<string,NodeCreaterCreater> creatorList = new Dictionary<string,NodeCreaterCreater>{
                //{"AAT", AATCC },
                {"NewDontReply", DontReplyCC },
                //{"NoMoreBelief", NoMoreBeliefCC },
                //{"BelieveOnlySensor", BelieveOnlySensorCC},
                //{"EatingWords", EatingWordsCC },
            };


            //**************実験条件を表示***************
            {
                var logAll = L.g("all");
                logAll.WriteLine("////////////////////experimental condition ////////////////////");

                var dt = DateTime.Now;
                string routeDir = dt.ToString("yyyy/MM/dd HH:mm:ss\n");
                logAll.WriteLine("Date: " + routeDir);

                logAll.WriteLine("AgentNum");
                foreach (int size in sizeList)
                {
                    logAll.Write(size + ", ");
                }
                logAll.WriteLine("\n");

                logAll.WriteLine("h_trg");
                foreach (double h_trg in h_trgList)
                {
                    logAll.Write(h_trg + ", ");
                }
                logAll.WriteLine("\n");

                logAll.WriteLine("expd");
                foreach (int expd in expdList)
                {
                    logAll.Write(expd + ", ");
                }
                logAll.WriteLine("\n");


                logAll.WriteLine("algorithm");
                foreach (string creatorName in creatorList.Keys)
                {
                    logAll.Write(creatorName + ", ");
                }
                logAll.WriteLine("\n");

                logAll.WriteLine("envSeedNum: " + envSeedNum);
                logAll.WriteLine("\n");
            }

            
            //じっけんID 
            int EXP_ID = 0;

            //時間をはかるよ
            DateTime prevTime = DateTime.Now;
            DateTime startTime = DateTime.Now;


            //それぞれのアルゴリズムについて
            foreach (var creator in creatorList)
            {
                //それぞれのサイズについて
                foreach (int size in sizeList)
                {
                    //それぞれのh_trgについて
                    foreach (double h_trg in h_trgList)
                    {
                        //それぞれのdについて
                        foreach (int expd in expdList)
                        {
                            //条件を表示
                            {
                                L.g("all").WriteLine("");
                                L.g("all").Write(string.Format("EXP_ID: {0}", EXP_ID++)); //実験IDを
                                L.g("all").Write(" Algo: " + creator.Key);//アルゴリズムの名前を表示

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

                                    RandomPool.Declare("setenv", envSeed);

                                    {//実験一回し

                                        RandomPool.Declare("fact", 0);//固定センサー値までばらつかせたら収集がつかんことを学習した。

                                        //環境を作る
                                        NetworkGenerator generator = new SmallworldRingNetworkGenerator(size, expd, 0.12);

                                        generator.NodeCreate += creator.Value(h_trg);

                                        OSEnvironment env = new OSEnvironment(generator, (int)(size * 0.05));

                                        //ファイルにネットワークの情報を表示
                                        L.g("env").WriteLine("" + env);
                                        L.g("env").Flush();

                                        Experiment exp = new Experiment(env);

                                        //実験を実行
                                        var expResult = exp.Run();

                                        {//平均をとるために和を計算
                                            aveSeedResult[0] += expResult. Correct;
                                            aveSeedResult[1] += expResult. Incorrect;
                                            aveSeedResult[2] += expResult. Undeter;
                                        }

                                    }

                                    { //時間を表示
                                        DateTime nowTime = DateTime.Now;
                                        TimeSpan tp = nowTime.Subtract(prevTime);
                                        TimeSpan fromStart = nowTime.Subtract(startTime);
                                        L.g("time,console").Write("<span: " + tp);             // 処理時間を秒単位で表示
                                        L.g("time,console").WriteLine("  fromStart: " + fromStart +" >"); // 処理時間を秒単位で表示

                                        prevTime = nowTime;
                                    }
                            }//forenvseed


                            { //平均をとる
                                aveSeedResult[0] /= envSeedNum;
                                aveSeedResult[1] /= envSeedNum;
                                aveSeedResult[2] /= envSeedNum;
                            }

                            L.g("aveacc,console").WriteLine(
                                string.Format("SeedAverage: correct: {0} incorrect: {1} undeter: {2}",
                                aveSeedResult[0], aveSeedResult[1], aveSeedResult[2]) );

                            //ファイルに書き込み
                            L.Flush();
                        }//for expd
                    }//for h_trg
                }//for size
            }//
        }//method AutoExperiment

        public static void Main(string[] args)
        {
            AutoExperiment();
            // logLoopExperiment();
            // networkTest();
            // agentTest();
            // buildCandidateTest();

            // Console.Read();
        }

        /*
        static void buildCandidateTest()
        {
            InitableRandom rand = new InitableRandom(0);
            NetworkGenerator generator = new CompleteNetworkGenerator(10,rand);
            generator.NodeCreate = () => (new AAT(rand));

            Network net = generator.create();

            AAT agent = (AAT)(net.Nodes.ElementAt(0));


            //((AAT)agent).PrepareAlgorithm();

            var candidates = agent.PrepareAlgorithm();

            foreach (var can in candidates)
            {
                double nextBelief = 0;
                if (can.JumpNum > 0)
                    nextBelief = Agent.updateFunc(Math.Abs(can.JumpNum), agent.Belief, can.ImportanceLevel);
                else
                    nextBelief = Agent.updateFunc(Math.Abs(can.JumpNum), agent.Belief, 1 - can.ImportanceLevel);


            }
        }

        static void agentTest()
        {

            Agent agent = new Agent(new InitableRandom(0));
            
            double b = 0;


            Console.WriteLine("black");
            for (int i = 0; i < 10; i++)
            {
                agent.ReceiveOpinion(BlackWhiteSubject.Black);

                b = agent.Belief;
                Console.WriteLine(agent.ToString("METER"));
            }

            Console.WriteLine("white");
            for (int i = 0; i < 15; i++)
            {
                agent.ReceiveOpinion(BlackWhiteSubject.White);
                b = agent.Belief;

                Console.WriteLine(agent.ToString("METER"));
            }

            Console.WriteLine("black");
            for (int i = 0; i < 30; i++)
            {
                agent.ReceiveOpinion(BlackWhiteSubject.Black);

                b = agent.Belief;
                Console.WriteLine(agent.ToString("METER"));
            }



        }

        static void opinionTest()
        {
            {// Opinion test
                AgentState opinion = new AgentState();

                Trace.WriteLine(opinion);

                opinion.Belief = (0.7);
                Trace.WriteLine(opinion);

                opinion.Belief = (0.9);
                Trace.WriteLine(opinion);

                opinion.Belief = 0.3;
                Trace.WriteLine(opinion);

                opinion.Belief -= 0.2;
                Trace.WriteLine(opinion);

            }

        }

        static void handler(object sender)
        {

            Trace.WriteLine(string.Format("handler! from sensor{0}", sender));
        }
        

        static void subjectTest()
        {
            {//enum Subject test
                BlackWhiteSubject subject = BlackWhiteSubject.White;

                Trace.WriteLine(string.Format("subject:{0} ", subject));
            }
        }

        static void fileOutputTest()
        {
            using (StreamWriter w = new StreamWriter(@"test.txt"))
            {
                w.WriteLine("基本的に、Trace クラスの文字列出力メソッドと同じ。");
                w.WriteLine("WriteLine では末尾に改行文字が加えられます。");
                int n = 5;
                double x = 3.14;
                w.Write("書式指定出力もできます → n = {0}, x = {1}", n, x);
            }

        }

        static void networkTest()
        {
            NetworkGenerator gen 
                = new SmallworldRingNetworkGenerator(10, 4, 0.12,new InitableRandom(0));
            gen.NodeCreate = ()=> new Node();

            Network net = gen.create();

            Console.WriteLine(net);


            foreach (var link in net.Links)
            {
                Console.WriteLine(link);
            }
            Console.WriteLine("one more time!");

            foreach (var link in net.Links)
            {
                Console.WriteLine(link);
            }


            
        }

        static int inputInt()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("format error. return 0;");
                return 0;
            }
        }

        static double inputDouble()
        {
            try
            {
                return double.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("format error. return 0;");
                return 0;
            }
        }

        public static void XmlTest()
        {

            //保存先のファイル名
            string fileName = @"sample.xml";

            //保存するクラス(SampleClass)のインスタンスを作成
            List<SampleClass> samples = new List<SampleClass>();


            SampleClass obj = new SampleClass();
            obj.Message = "テストです。";
            obj.Number = 123;

            samples.Add(obj);

            SampleClass obj2 = new SampleClass();
            obj2.Message = "テストです。";
            obj2.Number = 123;

            samples.Add(obj2);

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));

            //書き込むファイルを開く
            System.IO.FileStream fs = new System.IO.FileStream(
                fileName, System.IO.FileMode.Create);
            //シリアル化し、XMLファイルに保存する
            serializer.Serialize(fs, obj);
            //ファイルを閉じる
            fs.Close();
        }

        public static void XmlReadTest()
        {
            //保存元のファイル名
            string fileName = @"sample.xml";

            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
            //読み込むファイルを開く
            System.IO.FileStream fs = new System.IO.FileStream(
                fileName, System.IO.FileMode.Open);
            //XMLファイルから読み込み、逆シリアル化する
            SampleClass obj = (SampleClass)serializer.Deserialize(fs);

            Console.WriteLine("Number = " + obj.Number);
            Console.WriteLine("Message = " + obj.Message);

            //ファイルを閉じる
            fs.Close();

        }

        public static Environment InputEnvParam()
        {
            int agentNum = 100;
            int sensorNum = 5;
            int buf = 0;

            NetworkGenerator generator;


            /////////////////////////設定入力//////////////////////////
            Console.WriteLine("let's make network");
            Console.WriteLine("how many agents?");

            buf = inputInt();
            if (buf > 0)
            {
                agentNum = buf;
            }

            Console.WriteLine("{0} agents.", agentNum);

            Console.WriteLine("how many percent of agents have sensor?");
            buf = inputInt();

            if (buf > 0 && buf <= 100)
            {
                sensorNum = (int)(agentNum * ((double)buf / 100));
            }
            Console.WriteLine("Sensor number:{0}", sensorNum);


            Console.WriteLine("Which network topology? {0:complete network, 1: random network 2:small world }");
            buf = inputInt();


            Trace.WriteLine("----static settings----");
            Trace.WriteLine("agent num:\t" + agentNum);
            Trace.WriteLine("sensor agent:\t" + sensorNum);

            //agentNumとsensorNumが決定された。

            //ランダム
            InitableRandom rand = new InitableRandom(0);
            /////////////////////////次はネットワーク構造を決める//////////////////////////


            if (buf == 0)
            {
                Trace.WriteLine(   "topology: CompleteNetwork" );
                Console.WriteLine( "topology: CompleteNetwork" ); 
                generator = new CompleteNetworkGenerator(agentNum,rand);
            }

            else if (buf == 1)
            {
                Trace.WriteLine("topology: RandomNetwork");
                Console.WriteLine("topology: RandomNetwork");
                generator = new RandomNetworkGenerator(agentNum,rand);
            }
            else if (buf == 2)
            {
                Trace.WriteLine("topology: SmallworldNetwork");
                networkGenerator = new SmallworldRingNetworkGenerator(agentNum,5,0.12);
            }

            else//デフォルトはスモールワールド
            {
                Trace.WriteLine("topology: SmallworldNetwork");
                Console.WriteLine("topology: SmallworldNetwork");
                Console.WriteLine("expectedDegree ? ");
                int d = inputInt();

                Console.WriteLine("p_rewire ? ");
                double p_rewire = inputDouble();

                generator = new SmallworldRingNetworkGenerator(agentNum, d, p_rewire, rand);
            }
            generator.NodeCreate += () => new AAT(rand);

            return Experiment.makeEnviroment(agentNum, sensorNum, generator,rand);

        }

        static Experiment InputExpParam(){

            int MAX_ROUND = 150;
            int MAX_SENSOR = 3000;

            int buf = 0;

            ///////////// how many steps does a round have?  /////////////
            Console.WriteLine("How many sensor observation does a round have?");
            buf = inputInt();

            if (buf > 0)
            {
                MAX_SENSOR = buf;
            }

            ///////////// how many rounds?  /////////////
            Console.WriteLine("How many rounds?");
            buf = inputInt();
            if (buf > 0)
            {
                MAX_ROUND = buf;
            }

            Trace.WriteLine("----experiment settings----");
            Trace.WriteLine("round num       :\t" + MAX_ROUND);
            Trace.WriteLine("sensor par round:\t" + MAX_SENSOR);


            return  new Experiment(MAX_ROUND, MAX_SENSOR);
        }

        static void experiment()
        {

            ////////////環境パラメータを設定////////////
            Environment env = InputEnvParam();
            Console.WriteLine("network prepared.");


            #region 初期状態を表示
            Trace.WriteLine(env.Network);
            Trace.WriteLine("---initial state---");
            foreach (Agent a in env.Network.Nodes)
            {
                Trace.WriteLine(a.ToString("METER") + ((AAT)a).UpdatedNum);
            }
            Trace.WriteLine("");
            #endregion



            ////////////実験パラメータを設定////////////
            Experiment exp = InputExpParam();

            //実験を実行
            exp.Run(env);
            

        }

        static void logLoopExperiment()
        {
            //実験ID
            int i = 0;

            //ファイル名
            string filename;

            //つづけるかいなか
            int contin;

            do
            {
                //ファイル名をセット
                filename = @"log" + i + ".txt";

                //テキストファイルを開く
                StreamWriter writer = new StreamWriter(filename);

                var traceListner = new TextWriterTraceListener(writer);
                //Traceクラスにセット
                Trace.Listeners.Add(traceListner);

                //実験を行う
                experiment();
                

                //ファイルを閉じる]

                Trace.Listeners.Remove(traceListner);
                writer.Dispose();
                Console.WriteLine("file saved : " + filename);

                //実験IDをインクリメント
                i++;

                //続けるかいなか？　０か非数だったら終了
                try
                {
                    contin = inputInt();
                }
                catch (FormatException)
                {

                    contin = 1;
                }
            }
            while (contin != 0);

            Console.WriteLine("プログラムを終了します");


        }
        
        //XMLファイルに保存するオブジェクトのためのクラス
        public class SampleClass
        {
            public int Number;
            public string Message;
        }


        */

    }
}
