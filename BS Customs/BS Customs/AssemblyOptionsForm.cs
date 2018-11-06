using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using View = System.Windows.Forms.View;

namespace BIMtrovert.BS_Customs
{
    public partial class AssemblyOptionsForm : System.Windows.Forms.Form
    {
        View3D selected = null;
        Autodesk.Revit.DB.View selecting = null;
        Autodesk.Revit.DB.View selector = null;
        ViewSchedule selects = null;
        Element el = null;
        private string path = @"C:\Users\bscha\AppData\Roaming\Autodesk\Revit\Addins\2018\BS Customs\Solid Wall - A3.rfa";


        public AssemblyOptionsForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            UIApplication ui_app = commandData.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;
            List<View3D> views = new List<View3D>();
            List<Autodesk.Revit.DB.View> viewing = new List<Autodesk.Revit.DB.View>();
            List<Autodesk.Revit.DB.View> viewed = new List<Autodesk.Revit.DB.View>();
            List<ViewSchedule> viewer = new List<ViewSchedule>();
            IList<Element> title = new List<Element>();
            List<string> tlist = new List<string>();

            FilteredElementCollector ViewCollector3D = new FilteredElementCollector(doc).OfClass(typeof(View3D));
            FilteredElementCollector ViewCollectorEl = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View));
            FilteredElementCollector ViewCollectorPl = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View));
            FilteredElementCollector ViewCollectorSc = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule));
            FilteredElementCollector tempCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks);
            title = tempCollector.ToElements();
            foreach (View3D v3d in ViewCollector3D)
            {
                if (v3d.IsTemplate)
                {
                    views.Add(v3d);
                }

            }
            foreach (Autodesk.Revit.DB.View vEl in ViewCollectorEl)
            {
                if (vEl.IsTemplate)
                {
                    viewing.Add(vEl);
                }

            }
            foreach (Autodesk.Revit.DB.View vPl in ViewCollectorPl)
            {
                if (vPl.IsTemplate)
                {
                    viewed.Add(vPl);
                }

            }
            foreach (ViewSchedule vSc in ViewCollectorSc)
            {
                if (vSc.IsTemplate)
                {
                    viewer.Add(vSc);
                }

            }
            foreach (Element i in title)
            {
                tlist.Add(i.Name);
            }

            if (views != null)
            {
                v3dCombo.DataSource = views;
                v3dCombo.DisplayMember = "Name";

                if (Properties.Settings.Default.View3DTemplate != "")
                {
                    foreach (View3D item in ViewCollector3D)
                    {
                        if (item.Name == Properties.Settings.Default.View3DTemplate)
                        {
                            v3dCombo.SelectedIndex = v3dCombo.FindStringExact(item.Name);
                        }
                    }
                    
                }
                
            }
            if (viewing != null)
            {
                vElCombo.DataSource = viewing;
                vElCombo.DisplayMember = "Name";

                if (Properties.Settings.Default.ViewElTemplate != "")
                {
                    foreach (Autodesk.Revit.DB.View item in ViewCollectorEl)
                    {
                        if (item.Name == Properties.Settings.Default.ViewElTemplate)
                        {
                            vElCombo.SelectedIndex = vElCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }

            if (viewed != null)
            {
                vPlCombo.DataSource = viewed;
                vPlCombo.DisplayMember = "Name";

                if (Properties.Settings.Default.ViewPlTemplate != "")
                {
                    foreach (Autodesk.Revit.DB.View item in ViewCollectorEl)
                    {
                        if (item.Name == Properties.Settings.Default.ViewPlTemplate)
                        {
                            vPlCombo.SelectedIndex = vPlCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }

            if (viewer != null)
            {
                vPaCombo.DataSource = viewer;
                vPaCombo.DisplayMember = "Name";

                if (Properties.Settings.Default.ViewPaTemplate != "")
                {
                    foreach (ViewSchedule item in ViewCollectorSc)
                    {
                        if (item.Name == Properties.Settings.Default.ViewPaTemplate)
                        {
                            vPaCombo.SelectedIndex = vPaCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }

            if (tlist != null)
            {
                TemplCombo.DataSource = title;
                TemplCombo.DisplayMember = "Name";

                if (Properties.Settings.Default.TemplTemplate != "")
                {
                    foreach  (Element i in title)
                    {
                        if (i.Name == Properties.Settings.Default.TemplTemplate)
                        {
                            TemplCombo.SelectedIndex = TemplCombo.FindStringExact(i.Name);
                        }
                    }
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            selected = (View3D)v3dCombo.SelectedValue;
            selecting = (Autodesk.Revit.DB.View)vElCombo.SelectedValue;
            selector = (Autodesk.Revit.DB.View)vPlCombo.SelectedValue;
            selects = (ViewSchedule)vPaCombo.SelectedValue;
            el = (Element)TemplCombo.SelectedValue; 
            Properties.Settings.Default.View3DTemplate = selected.Name;
            Properties.Settings.Default.ViewElTemplate = selecting.Name;
            Properties.Settings.Default.ViewPlTemplate = selector.Name;
            Properties.Settings.Default.ViewPaTemplate = selects.Name;
            Properties.Settings.Default.TemplTemplate = el.Name;
            Properties.Settings.Default.Save();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Close();
        }

    }
}
