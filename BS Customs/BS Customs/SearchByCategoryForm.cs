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
    public partial class SearchByCategoryForm : System.Windows.Forms.Form
    {

        public ExternalCommandData datas;
        public List<Cat> catL = new List<Cat>();
        ICollection<ElementId> eid = new List<ElementId>();
        private Document doc;
        public SearchByCategoryForm(Categories cat, ExternalCommandData data)
        {
            InitializeComponent();
            List<Par> parList = new List<Par>();
            datas = data;

            UIApplication ui_app = datas.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = ui_app?.Application;
            doc = ui_doc?.Document;

            foreach (Category cats in cat)
            {
                catL.Add(new Cat() { ID = cats, Name = cats.Name });


            }
            catL = catL.OrderBy(o => o.Name).ToList();
            foreach (Cat c in catL)
            {
                catList.Items.Add(c.Name);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void filterBtn_Click(object sender, EventArgs e)
        {
            IList<Category> cList = new List<Category>();
            Category cg;
            foreach (var c in catList.CheckedItems)
            {
                foreach (Cat cl in catL)
                {
                    if (cl.Name == c.ToString())
                    {
                        cList.Add(Category.GetCategory(doc,cl.ID.Id));
                    }
                }
            }
            SelectCategoriesElements sce = new SelectCategoriesElements();
            sce.Execute(cList, datas, ProjRadio.Checked);
        }

        private void catList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                filterBtn.PerformClick();
            }
        }
    }

}
