using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpinionSharingForm.GUI
{
    public partial class DoubleBufferedPanel : UserControl
    {
        public DoubleBufferedPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

        }

        private void DoubleBufferedPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
