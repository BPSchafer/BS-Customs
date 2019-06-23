using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace BIMtrovert.BS_Customs
{
    public partial class ParameterSelector : System.Windows.Forms.Form
    {
        private ExternalCommandData datas;
        private ElementId ids;
        private bool projs;
        public ParameterSelector(ParameterSet param, ExternalCommandData data, ElementId id, bool proj)
        {
            InitializeComponent();
            ToolTip ToolTip1 = new ToolTip();
            ToolTip ToolTip2 = new ToolTip();
            ToolTip1.SetToolTip(andRadio, "Only elements that match all selected parameters will be selected");
            ToolTip2.SetToolTip(orRadio, "Any element that matches any selected parameter will be selected");
            List<Par> parList = new List<Par>();
            datas = data;
            ids = id;
            projs = proj;

            if (projs)
            {
                foreach (Parameter p in param)
                {
                    if (p.AsValueString() != null)
                    {
                        parList.Add(new Par() { ID = p, Name = p.Definition.Name + ": " + p.AsValueString() });
                    }
                    else
                    {
                        parList.Add(new Par() { ID = p, Name = p.Definition.Name + ": " + p.AsString() });
                    }

                }
            }
            else
            {
                foreach (Parameter p in param)
                {
                    if (p.AsValueString() != null)
                    {
                        parList.Add(new Par() { ID = p, Name = p.Definition.Name + ": " + p.AsValueString() });
                    }
                    else
                    {
                        parList.Add(new Par() { ID = p, Name = p.Definition.Name + ": " + p.AsString() });
                    }

                }
            }

            

            List<Par> paramList = parList.OrderBy(o => o.Name).ToList();
            IList<Par> distinctPar = paramList.Distinct().ToList();
            checksParam.DataSource = distinctPar;
            checksParam.ValueMember = "ID";
            checksParam.DisplayMember = "Name";

        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            try
            {

                bool andor = andRadio.Checked;

                Par par = new Par();
                IList<Parameter> pas = new List<Parameter>();

                foreach (var item in checksParam.CheckedItems)
                {
                    par = item as Par;
                    pas.Add(par.ID);

                }
                if (projectBtn.Checked)
                {
                    projs = true;
                }
                else
                {
                    projs = false;
                }
                RunFilter rf = new RunFilter();
                rf.Execute(ids, pas, datas, andor, projs);
                Close();
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Error", exception.Message);
                throw;
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checksParam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                filterButton.PerformClick();
            }
        }
    }

    public class Par : IEquatable<Par>
    {
        public Parameter ID { get; set; }
        public string Name { get; set; }

        public bool Equals(Par other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            int hashParName = Name == null ? 0 : Name.GetHashCode();

            return hashParName;
        }
    }
}
