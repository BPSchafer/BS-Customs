using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Forms;

namespace BIMtrovert.BS_Customs
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(string globalLabel1, string localLabel1, int globalStep, int localStep)
        {
            InitializeComponent();
            globalLabel.Text = globalLabel1;

            localLabel.Text = localLabel1;

            globalProgress.Step = globalStep;
            localProgress.Step = localStep;

        }

        public void LabelSet(string llabel, bool localStep, bool localReset, string glabel, bool globalStep)
        {
            
            globalLabel.Text = glabel;
            if (globalStep)
            {
                globalProgress.PerformStep();
            }

            localLabel.Text = llabel;
            if (localReset)
            {
                localProgress.Value = 0;
            }

            if (localStep)
            {
                localProgress.PerformStep();
            }

            Application.DoEvents();
        }

    }
}
