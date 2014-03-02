using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpinionSharing;
using OpinionSharing.Agt;
using OpinionSharing.Env;
using OpinionSharing.Net;
using OpinionSharing.Subject;

using System.Drawing.Drawing2D;

namespace OpinionSharingForm.GUI
{

    public partial class FigurePanel : UserControl
    {

        //エージェント描画用の追加の情報
        struct AgentView
        {
            public AgentView(float x, float y, float r, float w)
            {
                X = x;
                Y = y;
                R = r;
                PenWidth = w;
            }

            public float X;
            public float Y;
            public float R;
            public float PenWidth;

            public bool includes(int x, int y)
            {
                int R2 = (int)R * 2;
                if (x > X - R2 && x < X + R2)
                {
                    if (y > Y - R2 && y < Y + R2)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        //それぞれのエージェントに対して、Viewオブジェクトを対応させる
        Dictionary<AgentIO, AgentView> agentViews;

        //モデルへの参照
        public OpinionSharing.Env.OSEnvironment Environment{get;set;}

        //AgentPanelへの参照
        public AgentStatePanel AgentStatePanel{get;set;}

        private float scale = 1;

        //リンクを描画するかどうか
        public bool DrawLinks{get;set;}

        public FigurePanel()
        {
            InitializeComponent();

            this.Click += FigurePanel_Click;

            this.MouseWheel += FigurePanel_MouseWheel;

            this.DrawLinks = true;
        }


        void FigurePanel_MouseWheel(object sender, MouseEventArgs e)
        {
            scale += (float)e.Delta * 0.001f;

            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if(Environment != null && Environment.Network != null)
                calcAgentViews();
        }

        //float plustheta = 0;//無駄コード
        public void calcAgentViews()
        {

            //エージェント数からレイヤー数を決めるべき

            //ノードの数
            int N = Environment.Network.Nodes.Count();

            int layerNum = (int)Math.Ceiling((double)N/90);//層の分割数

            //画面幅
            int ActualWidth = this.ClientSize.Width;
            int ActualHeight = this.ClientSize.Height;
            //小さい方 = 使うサイズ
            int ActualSize = Math.Min(ActualWidth, ActualHeight);

            //View
            agentViews = new Dictionary<AgentIO, AgentView>();

            

            //レイヤーにどう割り振るかの配列

            var divideArray = Enumerable.Range(1, layerNum);// 1, 2, 3, 4=layerNum
            var cumDivideArray = cum(divideArray);// 1, 3, 6, 10=divideNum

            int divideNum = divideArray.Sum();// 1+2+3+4


            //それぞれのレイヤーの数
            int[] Ns = new int[layerNum];
            for (int i = 0; i < layerNum; i++)
            {
                Ns[i] = (int)(Math.Ceiling( (float)N * (layerNum - i) / divideNum));//layerNum=4 = divideArray.Last
            }

            //エージェントを均等に配置したときの角度
            float[] dthetas = new float[layerNum];
            for (int i = 0; i < layerNum; i++)
            {
                dthetas[i] = (float)1 / Ns[i] * 2*  (float)Math.PI;//小
            }

            //集団の丸
            float R = (float)ActualSize / 2F * 0.95F;
            float R2 = R/2;

            //エージェントの半径
            float r = dthetas[0] * R / 4;//
            float w = r / 4;


            float[] thetas = new float[layerNum];

            int agent_i = 0;
            foreach (AgentIO agent in Environment.Network.Nodes)
            {
                IAATBasedAgent aat = agent.Algorithm as IAATBasedAgent;
                float theta = 0;

                float newR = R;

                //どう移動するか、図的には決まってるのでアルゴリズム的に考えなくては。
                //まずは、divideNumで%する。出てきた数字を4444333221となめていく。
                //変数使わずにできるか？

                int mod = agent_i++ % divideNum;
                mod += 1;

                for (int i = 0; i < layerNum; i++)
                {
                    if (mod <= cumDivideArray.ElementAt(i)){
                        newR *= (i + 1f)/layerNum;
                        thetas[layerNum - 1 - i] += dthetas[layerNum - 1 - i];
                        theta = thetas[layerNum - 1 - i];
                        break;
                    }
                }

                float x = (float)Math.Sin(theta) * newR + ActualWidth / 2;
                float y = (float)Math.Cos(theta) * newR + ActualHeight / 2;

                agentViews[agent] = new AgentView(x, y, r, w);

            }

        }

        private IEnumerable<int> cum(IEnumerable<int> divideArray)
        {
            int n = divideArray.Count();
            int[] cum = new int[n];

            cum[0] = divideArray.ElementAt(0);

            for (int i = 1; i < n; i++)
            {
                cum[i] = cum[i - 1] + divideArray.ElementAt(i);
            }

            return cum;
        }


        //リサイズが起こったら再描画！！
        private void FigurePanel_Resize(object sender, EventArgs e)
        {
            Invalidate();

        }

        void FigurePanel_Click(object sender, EventArgs e)
        {
            Point p = this.PointToClient(Control.MousePosition);

            foreach (var pair in agentViews)
            {
                if (pair.Value.includes(p.X, p.Y))
                {  
                    AgentStatePanel.RegisterAgent(pair.Key.Algorithm as IAATBasedAgent);

                    this.Invalidate();
                    return;
                }

            }

            AgentStatePanel.RegisterAgent(null);
            this.Invalidate();
        }



        private void FigurePanel_Paint(object sender, PaintEventArgs e)
        {
            if (agentViews == null)
            {
                return;
            }

            //***************************基本的な設定***************************
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int ActualWidth =  this.ClientSize.Width;
            int ActualHeight = this.ClientSize.Height;

            
            //***************************ブラシなどの準備***************************

            //色の基本
            float hue = 200; //色相 青系：200
            float vOffset = 0.3f;//Vのオフセット  高いほど明るくなる

            //色
            Color OpWhiteColor = Util.StaticColor.ConvertHSBtoARGB(hue, 1f - 1f, 1f);
            Color OpBlackColor = Util.StaticColor.ConvertHSBtoARGB(hue, 1f - 0f, vOffset);
            Color OpUndeterColor = Util.StaticColor.ConvertHSBtoARGB(hue, 1f - 0.5f, (1f - vOffset) * 0.5f + vOffset);
            Color OrangeColor = Util.StaticColor.ConvertHSBtoARGB(40F, 0.9F, 0.9F);
            Color GreenColor = Util.StaticColor.ConvertHSBtoARGB(91F, 0.62F, 0.80F);
            Color PriorColor = Util.StaticColor.ConvertHSBtoARGB(hue, 0.5f, 0f);

            //ペン
            Pen blackPen = new Pen(Color.Black);
            Pen blackThinPen = new Pen(Color.Black);
                blackThinPen.Width = 1;

            Pen grayPen = new Pen(Color.Gray);
                grayPen.Width = 2;
                grayPen.StartCap = grayPen.EndCap = LineCap.Round;
            Pen priorBeliefPen = new Pen(PriorColor);
                priorBeliefPen.StartCap = priorBeliefPen.EndCap = LineCap.Round;
                priorBeliefPen.Width = 1;
            Pen linkPen = new Pen(GreenColor);
                linkPen.Width = 1.5f;
            Pen selectedLinkPen = new Pen(new SolidBrush(Color.Red));
                selectedLinkPen.Width = 3f;
            Pen sensorPen = new Pen(OrangeColor);
                sensorPen.Width = 10;
            Pen redPen = new Pen(Color.Red);
                redPen.Width = 3;
            Pen meterPen = new Pen(Util.StaticColor.ConvertHSBtoARGB(hue, 1f - 0f, vOffset));
                meterPen.Width = 2;
                meterPen.StartCap = meterPen.EndCap = LineCap.Round;

            //ブラシ
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush grayBrush = new SolidBrush(Color.Gray);
            SolidBrush lightGrayBrush = new SolidBrush(Color.LightGray);
            SolidBrush beliefBrush = new SolidBrush(Util.StaticColor.ConvertHSBtoARGB(100F, 0.9F, 0.8F));
            SolidBrush opinionBrush = new SolidBrush(Util.StaticColor.ConvertHSBtoARGB(0F, 0F, 1F));



            //四隅をためしに描画
            g.FillRectangle(beliefBrush, 0, 0, 10, 10);
            g.FillRectangle(beliefBrush, ActualWidth - 10, 0, 10, 10);
            g.FillRectangle(beliefBrush, ActualWidth - 10, ActualHeight - 10, 10, 10);
            g.FillRectangle(beliefBrush, 0, ActualHeight - 10, 10, 10);


            //***************************縮尺を変更***************************
            Matrix scaleMat = new Matrix(); 
            scaleMat .Scale(scale, scale);
            g.Transform = scaleMat;
            
            //AgentPanelに監視されてるエージェント
            IAATBasedAgent selected = AgentStatePanel.Agent;

            //リンクを描く 緑の線
            if(DrawLinks){
                foreach (Link li in Environment.Network.Links)
                {
                    AgentIO agent1 = li.Node1 as AgentIO;
                    AgentIO agent2 = li.Node2 as AgentIO;

                    Point p1 = new Point((int)agentViews[agent1].X, (int)agentViews[agent1].Y);
                    Point p2 = new Point((int)agentViews[agent2].X, (int)agentViews[agent2].Y);

                    if (agent1 == selected || agent2 == selected)
                    {
                        g.DrawLine(selectedLinkPen, p1, p2);
                    }
                    else
                    {
                        g.DrawLine(linkPen, p1, p2);
                    }
                }
            }



            //絶対行列を記録
            Matrix absoluteMatrix = g.Transform;

            //エージェントを描く
            foreach (AgentIO agentIO in Environment.Network.Nodes)
            {

                //******************各種サイズ**************************
                float r = agentViews[agentIO].R;
                float r2 = r * 2;
                float r3 = r * 3;
                float r4 = r * 4;

                float r_outer = r3;



                { //エージェントの位置に移動
                    //絶対行列に戻す
                    g.Transform = absoluteMatrix;

                    //エージェントの位置に移動
                    Matrix agentMatrix = g.Transform;
                    agentMatrix.Translate(agentViews[agentIO].X, agentViews[agentIO].Y);
                    g.Transform = agentMatrix;
                }


                //エージェントが描けない場合は丸書いて終わり
                if (!(agentIO as AgentIO).Drawable)
                {

                    g.FillEllipse(grayBrush, -r_outer / 2, -r_outer / 2, r_outer, r_outer);

                    continue;//break;にすべきかも
                }

                IAATBasedAgent aat = agentIO.Algorithm as IAATBasedAgent;


                //エージェントの内部状態
                float agentBelief = (float)(aat.Belief);
                float priorBelief = (float)(aat.PriorBelief);
                float sigmaRight = (float)(aat.Sigma);
                float sigmaLeft = 1 - sigmaRight;


                //ペンの太さを設定
                priorBeliefPen.Width = agentViews[agentIO].PenWidth;
                sensorPen.Width = priorBeliefPen.Width;






                //意見によってブラシの色を変更
                if (agentIO.Opinion == BlackWhiteSubject.White)
                    opinionBrush.Color = OpWhiteColor;

                else if (agentIO.Opinion == BlackWhiteSubject.Black)
                    opinionBrush.Color = OpBlackColor;

                else
                    opinionBrush.Color = OpUndeterColor;
                
                //信念によってブラシの色を変更
                //beliefBrush.Color = Util.StaticColor.ConvertHSBtoARGB(240 - (240 * agentBelief), 1f, 1f);
                beliefBrush.Color = Util.StaticColor.ConvertHSBtoARGB(hue, 1f - agentBelief, (1 - vOffset) * agentBelief + vOffset);

                //初期値は今は使ってない
                priorBeliefPen.Color = Util.StaticColor.ConvertHSBtoARGB(hue, 1f - priorBelief, (1 - vOffset) * priorBelief + vOffset);







                //panelによって注目しているエージェントを描画
                if (agentIO == selected)//|| i.ChangedOpinion)
                {
                    g.DrawEllipse(redPen, -r2, -r2, r2 * 2, r2 * 2);

                }

                //意見の円
                g.FillEllipse(opinionBrush, -r_outer / 2, -r_outer / 2, r_outer, r_outer);

                //扇型を表示
                {
                    float zeroDeg = 180f;
                    float sigmaLeftDeg = (sigmaLeft * 180f + zeroDeg);
                    float sigmaRightDeg = (sigmaRight * 180f + zeroDeg);
                    float sigmaSweepDeg = (sigmaRight - sigmaLeft) * 180f;


                    g.FillPie(lightGrayBrush, -r_outer/2, -r_outer/2, r_outer, r_outer , zeroDeg, 180);
                    g.FillPie(grayBrush, -r_outer/2, -r_outer/2, r_outer, r_outer , sigmaLeftDeg, sigmaSweepDeg);
                }

                //髪の毛を生成．しかし，毎回つくるのはしゃくだな・・・
                var beliefList = calcBeliefList(aat.PriorBelief, aat.Sigma, aat.CandidateSelector.BeliefUpdater);

                /* 将来なるであろうBelief */
                foreach (double futureBelief in beliefList)
                {
                    g.DrawLine(blackThinPen,
                        0,
                        0,
                        r_outer/2 * (float)Math.Cos(Math.PI * futureBelief + Math.PI),
                        r_outer/2 * (float)Math.Sin(Math.PI * futureBelief + Math.PI));
                }

                //priorBelief
                    g.DrawLine(blackThinPen, 0, 0,
                        r_outer/2 * (float)Math.Cos(Math.PI * priorBelief + Math.PI),
                        r_outer/2 * (float)Math.Sin(Math.PI * priorBelief + Math.PI));
                //*/


                //外枠の円
                g.DrawEllipse(priorBeliefPen, -r_outer / 2, -r_outer / 2, r_outer, r_outer);

                //信念のmeter
                g.DrawLine(meterPen, 0, 0,
                    r3 * (float)Math.Cos(Math.PI * agentBelief + Math.PI),
                    r3 * (float)Math.Sin(Math.PI * agentBelief + Math.PI));

                //信念の円
                g.FillEllipse(beliefBrush, -r / 2, -r / 2, r2 / 2, r2 / 2);



            }

            g.Transform = absoluteMatrix;

            //センサーを描く
            foreach (var s in Environment.Sensors)
            {
                var view = agentViews[s.Agent as AgentIO];
                float r2 = view.R * 2;

                g.DrawRectangle(sensorPen, view.X - r2, view.Y - r2, r2 * 2, r2 * 2);

            }
        }


        //Viewのために計算
        static private IEnumerable<double> calcBeliefList(double prior,double sigma, BeliefUpdater updater)
        {
            //ImportanceLevelが変わったら実行
            //priorbeliefとupdaterがあればできる

            List<double> beliefList = new List<double>();

            //更新しないようなパラメータでは、無限ループになってしまう
            if (updater.ImportanceLevel == 0.5)
            {
                return beliefList;
            }

            double goright = prior;
            double goleft = prior;

            
            //初期値を目盛に追加
            beliefList.Add(prior);

            do{
                goright = updater.updateBelief(BlackWhiteSubject.White, goright);
                beliefList.Add(goright);
            } while(goright < sigma );//&& i < 5);


            do{
                goleft = updater.updateBelief(BlackWhiteSubject.Black, goleft);
                beliefList.Add(goleft);
            } while(goleft > (1-sigma)  );//&& i < 5);

            return beliefList;
        }


    }
}
