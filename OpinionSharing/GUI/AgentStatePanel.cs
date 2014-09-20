using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpinionSharing.Agt;
using OpinionSharing.Subject;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using GraphTheory.Net;
using GraphTheory;

namespace OpinionSharingForm.GUI
{
    public partial class AgentStatePanel : DoubleBufferedPanel
    {
        IAATBasedAgent registeredAgent;

        UserSensor mySensor = new UserSensor();

        public AgentStatePanel()
        {
            InitializeComponent();

            BackColor = Color.WhiteSmoke;
        }

        public IAATBasedAgent Agent
        {
            get
            {
                return registeredAgent;
            }
        }

        public void RegisterAgent(IAgent agent)//エージェントの内部状態を見るもの
        {
            //******すでに登録されていて、違うエージェントだったら既存のイベントを削除******
            if (registeredAgent != null && registeredAgent != agent)
            {
                removeEvents(registeredAgent);

            }

            //******引数がnullだったら 表示を空っぽにして終了******
            if(agent==null)
            {
                //nullが渡されたなら，nullを登録
                registeredAgent = null;

                //候補をクリア
                Candidates_CB.DataSource = null;
                Candidates_CB.Items.Add(new object());
                Candidates_CB.Items.Clear();


                //表示を更新
                Invoke(new Action(() =>
                {
                    IDLabel.Text = "ID";
                    AlgorithmLabel.Text = "Algorithm";
                    TargetAwarenessRateLabel.Text = "h_trg";
                }));


                //再描画
                Invalidate();
                return;
            }

            //******型が合ってるかチェック***
            IAATBasedAgent aat = null;

            if (agent is IAATBasedAgent && agent is AgentAlgorithm)
            {
                aat = agent as IAATBasedAgent;
            }
            else
            {
                Console.WriteLine("このパネルでは，AATベースのエージェントアルゴリズムしか監視できません．");
                return;
            }


            //****** ここまで来たら，晴れて合格．AATベースのエージェントを登録 ******


            //新たなエージェントを追加

            registeredAgent = aat;
            
            //イベントを監視開始
            addEvents(registeredAgent);

            //ネットワークの指標を計算
            INode bodyAsINode = ((aat as AgentAlgorithm).Body as INode);

            double cluster = NetworkIndexes.cluster(bodyAsINode, bodyAsINode.Network);


            //表示を変更する
            Invoke(new Action(() =>  {
                //ちょっと複雑・・・
                IDLabel.Text = "ID" + (aat as AgentAlgorithm).ID;//結局Body=AgentIOを参照してる
                AlgorithmLabel.Text = aat.GetType().Name;
                TargetAwarenessRateLabel.Text = aat.TargetAwarenessRate.ToString();
                PrepareCandidatesCB(aat);

                otherStates.Text = "cluster = " + cluster + "\r\n" +                               
                                   "friend = " + bodyAsINode.Neighbours.Count;

            }));
            
            Invalidate();

        #region SHOW
            Console.WriteLine("registered :"+ registeredAgent);


            if (registeredAgent.CandidateSelector != null && registeredAgent.CandidateSelector.Candidates != null)
            {
                var candidates_greater = registeredAgent.CandidateSelector.Candidates.OrderBy((c) => c.ImportanceLevel);

                int i = 0;
                foreach (var candidate in candidates_greater)
                {
                    Console.Write("[0] l: {1} r: {2} t: {3} h: {4}",
                        i++,
                        candidate.JumpNumLeft,
                        candidate.JumpNumRight,
                        candidate.ImportanceLevel,
                        candidate.AwarenessRate);

                    if (candidate == registeredAgent.CandidateSelector.CurrentCandidate)
                    {
                        Console.Write("<==== this one!");
                    }

                    Console.WriteLine();
                }
            }

        #endregion

        }

    #region イベント系

        private void addEvents(IAATBasedAgent a)
        {
            a.BeliefChanged += AgentBeliefChanged;
            a.OpinionChanged += AgentOpinionChanged;
            a.CandidateChanged += AgentCandidateChanged;//イベントも追加
        }

        private void removeEvents(IAATBasedAgent a)
        {
            a.BeliefChanged -= AgentBeliefChanged;
            a.OpinionChanged -= AgentOpinionChanged;
            a.CandidateChanged -= AgentCandidateChanged;//イベントも追加
        }

        public void AgentBeliefChanged(object sender, BeliefEventArgs e)
        {
            //Console.WriteLine("belief " + e.Belief);
            Invalidate();
        }

        void AgentOpinionChanged(object sender, OpinionEventArgs e)
        {
            //Console.WriteLine("opinion " + e.Opinion);


        }
        
        public void AgentCandidateChanged(Object sender, CandidateEventArgs e)
        {
            if (registeredAgent != null)
            {
                Invoke(new Action(() =>
                {
                    PrepareCandidatesCB(registeredAgent);
                }));
            }
        }

    #endregion イベント系
        
        //このメソッドはInvokeの中から呼ばれる
        private void PrepareCandidatesCB(IAATBasedAgent aat)
        {
            if (aat.Candidates == null)
            {
                return;
            }

            Candidates_CB.SelectedValueChanged -= CandidateCB_SelectedValueChanged;

            //Comboboxの中身をまず空にする。
            Candidates_CB.DataSource = null;


            
            //毎回列の設定してる。もうすこし最適化できるはず。
            DataTable CandidateTable = new DataTable();
            CandidateTable.Columns.Add("STR", typeof(string));
            CandidateTable.Columns.Add("CAND", typeof(Candidate));


            lock (aat.CandidateLock)
            {
                var cands = aat.Candidates;
                var orderedCands = cands.OrderBy((c) => c.ImportanceLevel);

                foreach (Candidate cand in orderedCands)
                {
                    DataRow row = CandidateTable.NewRow();

                    row["STR"] =
                    string.Format("<< {0,-2}  t: {1,-4:f2}  h: {2,-4:f2} {3,2} >>"
                                    , cand.JumpNumLeft, cand.ImportanceLevel, cand.AwarenessRate, cand.JumpNumRight);

                    row["CAND"] = cand;

                    CandidateTable.Rows.Add(row);

                }
            }

            Candidates_CB.DataSource = CandidateTable;

            Candidates_CB.DisplayMember = "STR";
            Candidates_CB.ValueMember = "CAND";

            Candidates_CB.SelectedValue = aat.CurrentCandidate;

            //Candidates_CB.SelectedValueChanged += CandidateCB_SelectedValueChanged;
        }
        


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            var g = e.Graphics;

            //pen & brush
            Pen blackPen = new Pen(Color.Black);
            Brush WhiteBrush = new SolidBrush(Color.White);

            Pen whitePen = new Pen(Color.White);
            Brush blackBrush = new SolidBrush(Color.Black);
            SolidBrush opinionBrush = new SolidBrush(Color.Black);

            //sizes
            int Width = this.ClientSize.Width;
            int Height = this.ClientSize.Height;

            Size rectSize = new Size(100,10);
            Size ellipseSize = new Size(10, 10);



            //matrix
            Matrix mat = new Matrix();

            Matrix def = g.Transform;

            /////////////////////////描画//////////////////////////////
            mat.Translate(Width / 2, 60);
            g.Transform = mat;


            //メータを表示
            g.DrawRectangle (blackPen,
                - rectSize.Width / 2,
                - rectSize.Height / 2,
                  rectSize.Width,
                  rectSize.Height);

            /////////////////////////四角形描画//////////////////////////////
            mat.Translate(-rectSize.Width / 2, -rectSize.Height / 2);
            g.Transform = mat;


            if (registeredAgent != null)
            {
                if (registeredAgent.Opinion == null)
                {
                    opinionBrush.Color = Color.Gray;
                }
                else
                {
                    if (registeredAgent.Opinion == BlackWhiteSubject.White)
                    {
                        opinionBrush.Color = Color.White;
                    }
                    else
                    {
                        opinionBrush.Color = Color.Black;
                    }
                }

                int beliefPosition = (int)(rectSize.Width * registeredAgent.Belief);
                int priorBeliefPosition = (int)(rectSize.Width * registeredAgent.PriorBelief);
                int sigmaPosition  = (int)(rectSize.Width * registeredAgent.Sigma);
                int sigmaPosition2 = (int)(rectSize.Width * (1 - registeredAgent.Sigma));

                //sigma：線
                g.DrawLine(blackPen,
                    sigmaPosition2,
                    0,
                    sigmaPosition2,
                    10);

                    
                //sigma：線
                g.DrawLine(blackPen,
                    sigmaPosition,
                    0,
                    sigmaPosition,
                    10);

                

                /////////////////////////belief描画//////////////////////////////
                mat.Translate(beliefPosition,0);
                g.Transform = mat;
                
                //belief塗
                g.FillEllipse(opinionBrush,
                    -ellipseSize.Width/2,
                    0,
                    ellipseSize.Width,
                    ellipseSize.Height);
                
                //belief線
                g.DrawEllipse(blackPen,
                    -ellipseSize.Width/2,
                    0,
                    ellipseSize.Width,
                    ellipseSize.Height);

                mat.Translate(-beliefPosition,0);
                g.Transform = mat;
                /////////////////////////belief描画終了//////////////////////////////

                //Priorbelief塗
                g.DrawLine(blackPen,
                    priorBeliefPosition,
                    0,
                    priorBeliefPosition,
                    5);
            }
        }

        private void WhiteButton_Click(object sender, EventArgs e)
        {
            //agent：はオブザーバーで描画される可能性がある。Invokeが起こる。
            //UIスレッドがブロックしてる状態でInvokeが呼ばれてはいけない。
            //UIスレッドをブロックしなければよい。　なので、別スレッドで処理する。
            //本当は、モデルの処理はある一つのスレッドで処理するべきなのかも？
            //Task学ぼう？
            Task.Factory.StartNew(() => 
            {
                if (registeredAgent != null) 
                {
                    mySensor.SendWhite( (registeredAgent as AgentAlgorithm).Body );
                }
            });
        }

        private void BlackButton_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (registeredAgent != null)
                {
                    mySensor.SendBlack( (registeredAgent as AgentAlgorithm).Body );
                }
            });
        }


        private void CandidateCB_SelectedValueChanged(object sender, EventArgs e)
        {
            Candidate newcand = Candidates_CB.SelectedValue as Candidate;
            if (newcand != null)
            {
                IAATBasedAgent  aat = registeredAgent as IAATBasedAgent;

                /*
                aat.CurrentCandidate = newcand;
                */
            }

        }

        private void Candidates_CB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
