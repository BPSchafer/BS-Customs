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
        private Document doc;
        public ParameterSearcher(Categories catSet, ExternalCommandData data)
        {
            InitializeComponent();
            List<Par> parList = new List<Par>();
            datas = data;

            UIApplication ui_app = datas.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = ui_app?.Application;
            doc = ui_doc?.Document;

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
            bool addElement;
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
                                string defName = "";
                                defName = ElementValue(pa, defName, doc);
                                if (defName != null)
                                {
                                    if (partialMatch.Checked)
                                    {
                                        addElement = defName.Contains(searchText.Text);
                                    }
                                    else
                                    {
                                        addElement = defName == searchText.Text;
                                    }
                                }
                                else
                                {
                                    addElement = false;
                                }


                                //addElement = partialMatch.Checked ? defName.Contains(searchText.Text) : defName == searchText.Text;
                                if (addElement) eid.Add(el.Id);

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
                                string defName = "";
                                defName = ElementValue(pa, defName, doc);
                                if (defName != null)
                                {
                                    if (partialMatch.Checked)
                                    {
                                        addElement = defName.Contains(searchText.Text);
                                    }
                                    else
                                    {
                                        addElement = defName == searchText.Text;
                                    }
                                }
                                else
                                {
                                    addElement = false;
                                }
                                if (addElement) eid.Add(el.Id);

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

        public string ElementValue(Parameter pa, string defName, Document doc)
        {
            switch (pa.StorageType)
            {
                case StorageType.Double:
                    defName = pa.AsValueString();
                    break;
                case StorageType.ElementId:
                    Autodesk.Revit.DB.ElementId id = pa.AsElementId();
                    if (id.IntegerValue >= 0)
                    {
                        defName = doc.GetElement(id).Name;
                    }
                    else
                    {
                        defName = id.IntegerValue.ToString();
                    }
                    break;
                case StorageType.Integer:
                    if (ParameterType.YesNo == pa.Definition.ParameterType)
                    {
                        if (pa.AsInteger() == 0)
                        {
                            defName = "False";
                        }
                        else
                        {
                            defName = "True";
                        }
                    }
                    else
                    {
                        defName = pa.AsInteger().ToString();
                    }
                    break;
                case StorageType.String:
                    defName = pa.AsString();
                    break;
                default:
                    defName = "Unexposed parameter.";
                    break;

            }

            return defName;
        }

        private void ParameterSearcher_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    if (e.Modifiers == Keys.Shift)
                        this.ProcessTabKey(false);
                    else
                        this.ProcessTabKey(true);
                    break;
                case Keys.Down:
                    if (viewBtn.Focused)
                    {
                        viewBtn.Checked = !viewBtn.Checked;
                        projectBtn.Checked = !projectBtn.Checked;
                    }
                    break;
                case Keys.Up:
                    if (viewBtn.Focused)
                    {
                        viewBtn.Checked = !viewBtn.Checked;
                        projectBtn.Checked = !projectBtn.Checked;
                    }
                    break;
            }

        }

        private void searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchBtn.PerformClick();
            }
        }

        private void viewBtn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchBtn.PerformClick();
            }
        }

        private void partialMatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchBtn.PerformClick();
            }
        }
    }

    public class Cat
    {
        public Category ID { get; set; }
        public string Name { get; set; }
    }
}

