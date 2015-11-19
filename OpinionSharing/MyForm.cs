
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using System.Drawing.Drawing2D;

using OpinionSharing;

using OpinionSharing.Agt;
using OpinionSharing.Agt.Factory;
using OpinionSharing.Agt.Algorithm;
using OpinionSharing.Subject;
using MyRandom;
using GraphTheory.Net;
using OpinionSharing.Env;

using Log;


namespace OpinionSharingForm
{
    public partial class MyForm : Form
    {

        //問題領域クラス
        OSEnvironment env;
        Experiment exp = new Experiment();

        //いずれExperimentにまかせたいs
        double sensorRate = 0.1;

        //アルゴリズム選択用
        string currentCreatorName;

        //*******************************コンストラクタ*******************************
        public MyForm() {

            L.gDirectory("dir");
            L.gDefine("env");

            InitializeComponent();

            //背景を白に
            this.BackColor = Color.White;


            //アルゴリズムを選択するためのListを初期化する。
            initAlgoTable();

            exp.SetEnvsetSeed(0);   //initEnvでseedの設定してるよ
            exp.SetWeightSeed(0);   //重みのシード：
            exp.SetFactSeed(0);     //ちゃんとinitEnvでseedの設定してるよ
            exp.SetSensorSeed(0);   //ラウンドの始まりに初期化されますよ．


            //環境を作成 エージェント数100, センサー数5, 平均リンク数8 , 
            initEnv("WS",100, 5, 8,0.12,0);
            SplitContainer.IsSplitterFixed = false;

        }
        
        private void initAlgoTable()
        {
            List<string> creatorsName = new List<string>{
                 "AAT",              
                 "DontReply",        
                 "NewDontReply",        
                 "LimitedBelief",       
                 "EatingWords",       
                 "PartialLimitedBelief",  
                 "SubOpinion",   
                 "WeightedNeighbour"
            };


            //DataTableオブジェクトを用意
            DataTable creatorTable = new DataTable();

            //DataTableに列を追加
            creatorTable.Columns.Add("NAME", typeof(string));

            foreach (var creator in creatorsName)
            {
                //新しい行を作成
                DataRow row = creatorTable.NewRow();
                //各列に値をセット
                row["NAME"] = creator;      //文字列による名前

                //DataTableに行を追加
                creatorTable.Rows.Add(row);

            }

            //ComboBoxに追加
            AlgoCB.DataSource = creatorTable;
            AlgoCB.DisplayMember = "NAME"; //表示するのは文字列
            AlgoCB.ValueMember = "NAME"; //実際の値は、NodeCreator型。


            AlgoCB.SelectedIndex = 0;

            currentCreatorName = AlgoCB.SelectedValue as string;
        }


        private void initEnv(string generatorstr,int agentNum, int sensorNum, int expectedDegree, double pRewire, int seed)
        {
            StopAnimation();


            GeneratorCB.SelectedItem = generatorstr;
            AgentNumTB.Text = agentNum.ToString();
            SensorNumTB.Text = sensorNum.ToString();
            ExpectedDegreeTB.Text = expectedDegree.ToString();
            PRewireTB.Text = pRewire.ToString();
            SeedTB.Text = seed.ToString();

            //環境つくるためのシードをセット これは環境つくるためのメソッドなのでOK．
            exp.SetEnvsetSeed(seed);

            //重みをつくるためのシードをセット
            //exp.SetWeightSeed(seed);
            
            //新しいネットワークジェネレータで
            //NetworkGenerator generator = new WSmodelNetworkGenerator(agentNum, expectedDegree, pRewire);

            NetworkGenerator generator = new WSmodelNetworkGenerator(agentNum, expectedDegree, pRewire);
            
            if (generatorstr == "WS")
            {
                generator = new WSmodelNetworkGenerator(agentNum, expectedDegree, pRewire);
            }
            else if (generatorstr == "BA")
            {
                generator = new BAModelNetworkGenerator(agentNum, 10,expectedDegree);//, pRewire);
            }
            else if (generatorstr == "Random")
            {
                generator = new RandomNetworkGenerator(agentNum, expectedDegree, pRewire);//, pRewire);
            }

            else if (generatorstr == "Leader")
            {
                generator = new LeaderNetworkGenerator(agentNum, expectedDegree, pRewire,99);//, pRewire);
            }



            

            generator.NodeCreate += ()=> new AgentIO();

            //新たな環境を構築
            env = new OSEnvironment(generator , sensorNum);

            

            exp.Environment = env;

            //figurePanelネットワークのオブザーバ(まだ不完全)に環境を追加
            figurePanel.Environment = env;

            //エージェントのビューも更新
            figurePanel.calcAgentViews();

            //エージェント表示モジュールは状態表示モジュールを参照(選択の情報を受け渡す)
            figurePanel.AgentStatePanel = agentStatePanel;

            //ファイルにネットワークの情報を表示
            L.g("env").WriteLine("" + env);
            L.g("env").Flush();

        }


        public void InitAlgorithm(string algoName, double h_trg)
        {
            //アルゴリズムをセット
            if (env != null && env.Network != null && env.Network.Nodes != null && env.Network.Nodes.Count() > 0)
            {
                //foreach (AATBasedAgentIO aat in env.Network.Nodes)
                foreach (AgentIO aat in env.Network.Nodes)
                {
                    aat.Algorithm = (AlgorithmFactory.CreateAlgorithm(algoName, h_trg));
                }
            }
        }


        /* private void OpinionSharingProcess()
        {
            try
            {
                while (!IsDisposed)
                {
                    /////センサー開始/////
                    while(exp.IsRoundFinished)
                    {
                        execStep();
                        Thread.Sleep( Interval );
                    }
                }
            }
            catch (ThreadAbortException )
            {
                Console.WriteLine("Sensor Thread Aborted.");
            }
        } */

        /*簡単なcsv出力関数*/
        private void WriteCsv()
        {
            var acc = env.EnvAccuracy;//EnvAccuracyで各数値計算（正解数，誤判定数，未決定数）
            Step.Text = "Step: " + exp.Step;
            //var points = AccuracyChart.Series["Accuracy"].Points;
            //points[0].YValues[0] = accCsv.Correct;
            //points[1].YValues[0] = accCsv.Incorrect;
            //points[2].YValues[0] = accCsv.Undeter;

            try
            {
                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = true;
                // 出力用のファイルを開く

                //同じファイルに上書きされているので，クラスの一番上なので宣言して新しくファイルを作成する必要あり
                //もしくは時間を取得してファイル名にする
                using (var sw = new System.IO.StreamWriter(@"./SimpleTest.csv", append))
                {
                        sw.WriteLine("step:{3}, correct:, {0}, incorrrect:, {1}, undeter:, {2}",
                acc.Correct, acc.Incorrect, acc.Undeter, exp.Step);
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Console.WriteLine(e.Message);
            }
        }


        private void updateStepInfo()
        {
            Step.Text = "Step: " + exp.Step;

            updateAccuracyPie();

            figurePanel.Invalidate();
        }

        private void updateAccuracyPie()
        {
            var acc = env.EnvAccuracy;



            var points = AccuracyChart.Series["Accuracy"].Points;
            points[0].YValues[0] = acc.Correct;
            points[1].YValues[0] = acc.Incorrect;
            points[2].YValues[0] = acc.Undeter;


            AccuracyChart.Invalidate();

            
            
        }


        //*******************各種3つのボタン**********************
        private void InitButton_Click(object sender, EventArgs e)
        {
            //アニメーションをストップ
            StopAnimation();

            //アニメーションを初期化
            InitAnimation();

            //表示を更新
            Invoke(new Action(updateStepInfo));

        }

        private void start_Click(object sender, EventArgs e)
        {
            SwitchAnimation();

        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            StopAnimation();
            execStep();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            execStep();

        }






        //******************処理********************
        private void SwitchAnimation()
        {
            AnimationTimer.Enabled = ! AnimationTimer.Enabled ;
            changeEnabled();
        }

        private void StopAnimation()
        {
            AnimationTimer.Enabled = false;
            changeEnabled();
        }

        private void changeEnabled()
        {
            if (AnimationTimer.Enabled == true)
            {
                StartButton.Text = "||";
            }
            else
            {
                StartButton.Text = ">>";
            }
        }

        private void execStep()
        {
            exp.ExecStep(sensorRate);
            //表示を更新
            var acc = env.EnvAccuracy;
            

            //Console.WriteLine("step:{3}, correct:, {0}, incorrrect:, {1}, undeter:, {2}",
            //    acc.Correct, acc.Incorrect, acc.Undeter, exp.Step);
            //csv書き込み開始
            this.WriteCsv();

            Invoke(new Action(updateStepInfo));

        }

        private void InitAnimation()//ラウンドの中身を実行するわけだよ。
        {
            //学習なしで実行初期化処理
            exp.CancelRound();

            int seed = (int)(SeedUpDown.Value);

            //ラウンドを初期化
            exp.StartRound(seed);

        }


        private void PrepareButton_Click(object sender, EventArgs e)
        {
            string agentNumStr = AgentNumTB.Text;
            string sensorNumStr = SensorNumTB.Text;
            string expectedDegreeStr = ExpectedDegreeTB.Text;
            string pRewireStr = PRewireTB.Text;
            string seedStr = SeedTB.Text;
            string generatorStr = GeneratorCB.SelectedItem as String;

            int agentNum;
            int sensorNum;
            int expectedDegree;
            double pRewire;
            int seed;

            try
            {
                agentNum = int.Parse(agentNumStr);
                sensorNum = int.Parse(sensorNumStr);
                expectedDegree = int.Parse(expectedDegreeStr);
                pRewire = double.Parse(pRewireStr);
                seed = int.Parse(seedStr);
            }
            catch (FormatException)
            {
                MessageBox.Show("数値のフォーマットが違います");
                return;
            }

            if (sensorNum > agentNum)
            {
                MessageBox.Show("センサー数が多すぎです。エージェント数より多くすることはできません。");
                return;
            }

            if (expectedDegree > agentNum -1)
            {
                MessageBox.Show("リンク数が多すぎです。エージェント数より多くすることはできません。");
                return;
            }

/*            MessageBox.Show(string.Format("AgentNum = {0}, SensorNum = {1}, ExpectedDegree = {2}",
                agentNum,
                sensorNum,
                expectedDegree ));*/


            initEnv(generatorStr,agentNum,sensorNum, expectedDegree, pRewire, seed);


            figurePanel.Invalidate();
        }


        private void LearningStart_Click(object sender, EventArgs e)
        {
            StopAnimation();

            Task.Factory.StartNew(learning).ContinueWith((Task t) =>
            {
                

                Invoke(new Action(() => LearningProcess.Value = 100));

                MessageBox.Show("learning finished!");

                Invoke(new Action(() => LearningProcess.Value = 0));

                //アニメーションを開始するので初期化
                InitAnimation();
            });



        }


        private void learning()
        {

            //各ラウンドでの事実を決める乱数
            exp.SetFactSeed(0);

            exp.Initialize();

            //1st step
            exp.PrepareAlgorithm();


            while(!exp.LearningIsFinished)
            {
                env.TheFact.randomNext();

                exp.ExecRound();


                //プログレスを表示
                int progress = (int) (exp.ProgressRate * 100.0);

                Invoke(new Action(() => {  
                    LearningProcess.Value = progress;
                }));
            }
        }

        private void MyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //アニメーションをストップ
            StopAnimation();
        }

        private void settings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PrepareButton_Click(sender, e);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            AnimationTimer.Interval = trackBar1.Value;
        }

        private void agentStatePanel_Load(object sender, EventArgs e)
        {

        }

        private void AlgoCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentCreatorName = AlgoCB.SelectedValue as string;
        }

        private void WhiteRadio_CheckedChanged(object sender, EventArgs e)
        {
            env.TheFact.setNext(BlackWhiteSubject.White);
        }

        private void BlackRadio_CheckedChanged(object sender, EventArgs e)
        {
            env.TheFact.setNext(BlackWhiteSubject.Black);
        }

        private void figurePanel_Load(object sender, EventArgs e)
        {

        }

        private void TargetAwarenessRatTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void TargetAwarenessRateLabel_Click(object sender, EventArgs e)
        {

        }

        private void SetAlgo_Button_Click(object sender, EventArgs e)
        {
            double h_trg = double.Parse(TargetAwarenessRatTB.Text);
            string algoName = AlgoCB.SelectedValue as string;

            InitAlgorithm(algoName, h_trg);
        }

        private void GeneratorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void AccuracyChart_Click(object sender, EventArgs e)
        {

        }

        private void NetworkTab_Click(object sender, EventArgs e)
        {

        }

        private void SplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SplitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LearningTab_Click(object sender, EventArgs e)
        {

        }

        //dump用ボタン
        private void NetworkIndeces_Click(object sender, EventArgs e)
        {

            try
            {
                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = false;
                // 出力用のファイルを開く

                //同じファイルに上書きされているので，クラスの一番上なので宣言して新しくファイルを作成する必要あり
                //もしくは時間を取得してファイル名にする
                using (var sw = new System.IO.StreamWriter(@"./NetworkIndecies.csv", append))
                {
                    sw.Write("nodeID,degree,cluster,degreeCentrality,closenessCentrality,AverageDistance,maxDistance,minDistance" + "\r\n");
                    foreach (var node in env.Network.Nodes)
	                {
                        sw.WriteLine(node.CsvStatus);
	                }
                       
                }
            }
                catch (System.Exception a)
                {
                    // ファイルを開くのに失敗したときエラーメッセージを表示
                    System.Console.WriteLine(a.Message);
                }                        
        }

        //private void AgentOpinionStatus_Click(object sender, EventArgs e)
        //{
            
        //}

        private void AgtOpinionStatus_Click(object sender, EventArgs e)
        {
           
            try
            {
                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = false;
                // 出力用のファイルを開く

                //同じファイルに上書きされているので，クラスの一番上なので宣言して新しくファイルを作成する必要あり
                //もしくは時間を取得してファイル名にする
                using (var sw = new System.IO.StreamWriter(@"./AgentOpinionStatus.csv", append))
                {
                    foreach (var node in env.Network.Nodes)
                    {
                        IAATBasedAgent n = node as IAATBasedAgent;

                        sw.Write("nodeID,"+node.ID);
                        sw.WriteLine(agentStatePanel.AgtOpinionStatus(n));
                    }
                }
            }
            catch (System.Exception a)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Console.WriteLine(a.Message);
            }
        }


    }
}
