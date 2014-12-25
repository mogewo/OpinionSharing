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
        
        //agentは目的をもつ，真ならばもち，偽ならもたない
        //bool puropose { get; }

        void ReceiveOpinion(BWMessage message);
        void ProcessMessage(BWMessage message);

        void PrepareAlgorithm();
        void RoundInit();
        void RoundFinished();

    }
}
