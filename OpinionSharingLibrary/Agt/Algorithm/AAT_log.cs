using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt.Algorithm
{
    //学習してround毎に，特定のエージェントログ取るだけ（特定部分はAATのroundfinish参照）
    class AAT_log:AAT
    {
        public override void own_log(int? round)
        {
            try
            {
                //近隣がsensorかどうかのフラグ                
                string s = "0";
                
                foreach (var n in this.Neighbours)
                {
                    AgentIO neighbor = n as AgentIO;
                    if (neighbor.HasSensor)
                    {
                        s = "1";
                    }
                }
                using (var sw = new System.IO.StreamWriter(@"AAT_log"+ round +  "_agent_" + this.ID + "_inf" + ".csv", false))
                {
                    sw.WriteLine("round,agent_ID,this_Awareness Rate, this_Importance Level, sensor");
                }

                using (var sw = new System.IO.StreamWriter(@"AAT_log" + round + "_agent_" + this.ID + "_inf" + ".csv", true))
                {
                    sw.WriteLine("{0},{1},{2},{3},{4}", round, this.ID, this.CurrentCandidate.AwarenessRate, this.CurrentCandidate.ImportanceLevel, s);
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
