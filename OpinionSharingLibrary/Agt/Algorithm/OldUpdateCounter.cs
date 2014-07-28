using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt.Algorithm
{
    public class UpdateCounter
    {
        //whiteをもらったら++, Blackをもらったら--
        protected int  updateNum; 
        protected int updateRight; // >= 0
        protected int updateLeft; // >= 0

        public UpdateCounter()
        {
            Initialize();
        }

        public void Initialize()
        {
            updateNum = 0;
            updateRight = 0;
            updateLeft = 0;
        }

        public void CountUp()
        {
            UpdateNum ++ ;
        }
        
        public void CountDown()
        {
            UpdateNum -- ;
        }

        public int UpdateNum
        {
            get {
                return updateNum;
            }

            set {

                updateNum = value;

                if (updateNum > 0 && updateNum >= updateRight)
                {
                    updateRight = updateNum;
                }
                else if (updateNum < 0 && -updateNum >= updateLeft)
                {
                    updateLeft = -updateNum;
                }
            }
        }

        public int UpdateLeft //正の数
        {
            get
            {
                return updateLeft;
            }
        }

        public int UpdateRight //正の数
        {
            get
            {
                return updateRight;
            }
        }

        public int UpdateMax //正の数
        {
            get
            {
                return Math.Max(updateRight, updateLeft);
            }
        }

        
    }
}
