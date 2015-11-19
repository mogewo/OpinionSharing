using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Agt.Algorithm;
using OpinionSharing.Agt.Updater;
using MyRandom;

namespace OpinionSharing.Agt.Factory
{
    static public class AlgorithmFactory
    {

        //Algorithmであることも保証してくれるようなInterfaceがほしい。
        static Dictionary<string,Func<IAATBasedAgent>> creators = new Dictionary<string,Func<IAATBasedAgent>>(){  
             {"AAT",              () => new AAT()},  
             {"DontReply",        () => new DontReply()},  
             {"NewDontReply",        () => new NewDontReply()},  
             {"LimitedBelief",     () => new LimitedBelief()},  
             {"PartialLimitedBelief",() => new PartialLimitedBelief()},  
             {"EatingWords",      () => new EatingWords()},  
             {"SubOpinion",       () => new SubOpinion()},
             {"WeightedNeighbour", () => new WeightedNeighbour()},
             //{"StackOpinion",     () => new StackOpinion()},
        };


        static public AgentAlgorithm CreateAlgorithm(string name , double h_trg)
        {
            IAATBasedAgent algo = creators[name]();

            //信念など
            if (algo is SubOpinion) //今はまだ場合分けが少ないからいいけどいずれはFactory Methodになるかも。
            {
                algo.Thought = new ThoughtTwin(RandomPool.Get("envset").NextNormal(0.5, 0.1));
            }
            else
            {
                algo.Thought = new Thought(RandomPool.Get("envset").NextNormal(0.5, 0.1));
            }

            //候補集合
            algo.CandidateSelector = new CandidateUpdaterSelector();

            algo.TargetAwarenessRate = h_trg;

            //最後に初期化
            algo.Initialize();

            return algo as AgentAlgorithm;
        }
    }
}
