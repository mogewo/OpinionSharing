using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;



namespace OpinionSharing.Agt.Algorithm
{
    public class EatingWords : LimitedBelief
    {

        int opinionFormed = 0;
        
        public override void Initialize()
        {
            base.Initialize();
            opinionFormed = 0;
        }
        public override void RoundInit()
        {
            opinionFormed = 0;
            base.RoundInit();

        }

        public override void UpdateOpinion(BlackWhiteSubject sub, BeliefUpdater updater){
            //前の意見を記録
            BlackWhiteSubject? previousOpinion = Opinion;
            
            //ここで意見が変わるかも。
            base.UpdateOpinion(sub, updater);

            //新しい意見
            BlackWhiteSubject? newOpinion = Opinion;

            //意見が変わったら
            if (previousOpinion != newOpinion)
            {
                opinionFormed++ ;
            }

        }


        protected override void NotifyOthers(BlackWhiteSubject? myOpinion = null) 
        {
            BlackWhiteSubject opinion = checkOpinion(myOpinion);

            //今まで溜まってきた意見の分だけ、挽回が必要なので、何回もみんなに知らせる

            /*
            for (int i = 0; i < opinionFormed; i++)
            {
                base.NotifyOthers();
            }
            */
                 
            if(opinionFormed >= 2)
            {
                base.NotifyOthers(opinion);
            }
            base.NotifyOthers(opinion);
        }

        

    }
}
