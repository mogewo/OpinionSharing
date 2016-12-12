using OpinionSharing.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt.Updater
{
    class WeightedBeliefUpdater :BeliefUpdater
    {

        public double Weight{get;set;}

        public WeightedBeliefUpdater(double v = 0.55, double w = 0.5):base(v)
        {
            Weight = w;
        }

        public override double updateBelief(BlackWhiteSubject sbj, double belief)
        {
            //同じ式を使ってるので，意味は違うけど活用しちゃう．
            var newImportanceLevel = BeliefUpdater.updateFunc(ImportanceLevel, Weight);

            double cupd = decideCupd(sbj, newImportanceLevel);

            return updateFunc(belief, cupd);
        }
    }
}
