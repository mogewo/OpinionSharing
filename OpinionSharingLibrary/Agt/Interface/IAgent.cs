using System;

using OpinionSharing.Subject;
namespace OpinionSharing.Agt
{
    public interface IAgent :IOpinionSender
    {
        void Listen();
        void ProcessMessages();

        void Initialize();

        event EventHandler<OpinionEventArgs> OpinionChanged;
        BlackWhiteSubject? Opinion { get; }
        
        

        void ReceiveOpinion(BWMessage message);
        void ProcessMessage(BWMessage message);
        //void updateEdgeWeight();//重みをセットする 具体的な処理はweightedNeighbourで
        void PrepareAlgorithm();
        void RoundInit();
        void RoundFinished(BlackWhiteSubject? thefact);

    }
}
