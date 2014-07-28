using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpinionSharing.Subject;



namespace OpinionSharing.Agt.Original
{
    public class EatingWords : NoMoreBelief
    {

        int opinionFormed = 0;
        
        

        public EatingWords(double h_trg):base(h_trg)
        {
            
        }

        public EatingWords()
        {
        }



        public override void Initialize()
        {
            base.Initialize();
            opinionFormed = 0;
        }


        public override void UpdateOpinion(BlackWhiteSubject sub, double importanceLevel)
        {
            BlackWhiteSubject? previousOpinion = Opinion;//値型だからできること？
            
            base.UpdateOpinion(sub, importanceLevel);

            BlackWhiteSubject? newOpinion = Opinion;

            //意見が変わったら
            if (previousOpinion != newOpinion)
            {
                opinionFormed++ ;
            }

        }


        protected override void NotifyOthers()
        {
            //今まで溜まってきた意見の分だけ、挽回が必要なので、何回もみんなに知らせる


            /*
            for (int i = 0; i < opinionFormed; i++)
            {
                base.NotifyOthers();
            }
            */
                 
            if(opinionFormed >= 2)
            {
                base.NotifyOthers();
            }
            base.NotifyOthers();
            
        }

        public override void RoundInit()
        {
            opinionFormed = 0;
            base.RoundInit();

        }
        

    }
}
