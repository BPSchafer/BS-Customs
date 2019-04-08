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
    public partial class ParameterSearcher : System.Windows.Forms.Form
    {
        private ExternalCommandData datas;
        private List<Cat> catList = new List<Cat>();
        ICollection<ElementId> eid = new List<ElementId>();
        public ParameterSearcher(Categories catSet, ExternalCommandData data)
        {
            InitializeComponent();
            List<Par> parList = new List<Par>();
            datas = data;
            
            foreach (Category cat in catSet)
            {
                catList.Add(new Cat(){ ID = cat, Name = cat.Name});
                
                
            }
            catList = catList.OrderBy(o => o.Name).ToList();
            foreach (Cat c in catList)
            {
                categoryBox.Items.Add(c.Name);
            }
            
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            UIDocument uidoc = datas.Application.ActiveUIDocument;
            Category selected;
            ElementCategoryFilter ecf;
            FilteredElementCollector fec;
            eid.Clear();
            ElementId invalid = ElementId.InvalidElementId;
            List<ElementId> invalidList = new List<ElementId>();
            invalidList.Add(invalid);
            string mes;
            foreach (Cat c in catList)
            {
                if (c.Name == categoryBox.SelectedItem.ToString())
                {
                    selected = Category.GetCategory(uidoc.Document, c.ID.Id);
                    ecf = new ElementCategoryFilter(selected.Id);
                    if (viewBtn.Checked)
                    {
                        fec = new FilteredElementCollector(uidoc.Document, uidoc.Document.ActiveView.Id).WherePasses(ecf);
                        foreach (Element el in fec)
                        {
                            IList<Parameter> paras = el.GetParameters(paramText.Text);
                            foreach (Parameter pa in paras)
                            {
                                if (el.get_Parameter(pa.Definition).AsValueString() == searchText.Text)
                                {
                                    eid.Add(el.Id);
                                }
                            }
                        }
                        mes = "No elements' parameter values in this view matched your search.";

                    }
                    else
                    {
                        fec = new FilteredElementCollector(uidoc.Document).WherePasses(ecf);
                        mes = "No elements' parameter values in this project matched your search.";
                        foreach (Element el in fec)
                        {
                            IList<Parameter> paras = el.GetParameters(paramText.Text);
                            foreach (Parameter pa in paras)
                            {
                                if (el.get_Parameter(pa.Definition).AsValueString() == searchText.Text)
                                {
                                    eid.Add(el.Id);
                                }
                            }
                        }

                    }
                    if (eid.Count == 0)
                    {
                        uidoc.Selection.SetElementIds(new List<ElementId>());
                        MessageBox.Show(mes);
                    }
                    else
                    {
                        uidoc.Selection.SetElementIds(new List<ElementId>());
                        uidoc.Selection.SetElementIds(eid);
                    }

                    return;

                }
            }
            MessageBox.Show(categoryBox.SelectedItem.ToString());
        }
    }

    public class Cat
    {
        public Category ID { get; set; }
        public string Name { get; set; }
    }
}

