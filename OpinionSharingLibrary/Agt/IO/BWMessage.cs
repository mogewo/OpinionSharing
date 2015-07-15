using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using OpinionSharing.Subject;

namespace OpinionSharing.Agt
{
    //いわゆるDTO
    public class BWMessage
    {
        private BlackWhiteSubject subject;
        private IOpinionSender from;

        public BWMessage(BlackWhiteSubject sbj, IOpinionSender a)
        {
            subject = sbj;
            from = a;
        }

       
        public BlackWhiteSubject Subject{
            get 
            {
                return subject;
            }
        }

        public IOpinionSender From
        {
            get
            {
                return from;
            }
        }
    }

}
