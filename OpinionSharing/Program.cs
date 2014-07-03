using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using OpinionSharing.Agt;
using OpinionSharing.Env;
using Env = OpinionSharing.Env;

using GraphTheory;
using OpinionSharing.Subject;


using System.Diagnostics;

using System.Windows.Forms;

namespace OpinionSharingForm
{
    class Program
    {
        public static void Main(string[] args)
        {
            //XmlReadTest();

            /* */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyForm());
            //*/


            //logLoopExperiment();
            //networkTest();
            //agentTest();
            //buildCandidateTest();
            //Console.Read();
        }

    }

}
