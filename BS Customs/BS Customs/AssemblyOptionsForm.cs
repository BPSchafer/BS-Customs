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

namespace BIMtrovert.BS_Customs
{
    public partial class AssemblyOptionsForm : System.Windows.Forms.Form
    {
        Properties.Settings set = Properties.Settings.Default;

        View3D select3D = null;
        Autodesk.Revit.DB.View selectEl = null;
        Autodesk.Revit.DB.View selectPl = null;
        ViewSchedule selectPa = null;
        Element el = null;

        View3D select3DF = null;
        Autodesk.Revit.DB.View selectPlanF = null;
        Autodesk.Revit.DB.View selectJSF = null;
        Autodesk.Revit.DB.View selectTSF = null;
        ViewSchedule selectPaF = null;
        Element elF = null;

        DimensionType horDim = null;
        DimensionType vertDim = null;

        public AssemblyOptionsForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            UIApplication ui_app = commandData.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;

            List<View3D> view3d = new List<View3D>(); 
            List<Autodesk.Revit.DB.View> viewEl = new List<Autodesk.Revit.DB.View>();
            List<Autodesk.Revit.DB.View> viewPl = new List<Autodesk.Revit.DB.View>();
            List<ViewSchedule> viewSc = new List<ViewSchedule>();
            List<string> tlist = new List<string>();

            List<View3D> view3dF = new List<View3D>();
            List<Autodesk.Revit.DB.View> viewPlF = new List<Autodesk.Revit.DB.View>();
            List<Autodesk.Revit.DB.View> viewJSF = new List<Autodesk.Revit.DB.View>();
            List<Autodesk.Revit.DB.View> viewTSF = new List<Autodesk.Revit.DB.View>();
            List<ViewSchedule> viewScF = new List<ViewSchedule>();
            List<string> tlistF = new List<string>();

            List<DimensionType> dimL = new List<DimensionType>();
            List<DimensionType> VertDimL = new List<DimensionType>();

            FilteredElementCollector ViewCollector3D = new FilteredElementCollector(doc).OfClass(typeof(View3D));
            FilteredElementCollector ViewCollectorEl = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View));
            FilteredElementCollector ViewCollectorSc = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule));
            FilteredElementCollector tempCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks);
            FilteredElementCollector dimCollector = new FilteredElementCollector(doc).OfClass(typeof(DimensionType));


            addList(ViewCollector3D, view3d);
            addList(ViewCollectorEl, viewEl);
            addList(ViewCollectorEl, viewPl);
            addList(ViewCollectorSc, viewSc);
            addList(tempCollector, tlist);

            addList(ViewCollector3D, view3dF);
            addList(ViewCollectorEl, viewPlF);
            addList(ViewCollectorEl, viewJSF);
            addList(ViewCollectorEl, viewTSF);
            addList(ViewCollectorSc, viewScF);
            addList(tempCollector, tlistF);

            addList(dimCollector, dimL);
            addList(dimCollector, VertDimL);


            popCombo(v3dCombo, view3d, ViewCollector3D, set.View3DTemplate);

            popCombo(vElCombo, viewEl, ViewCollectorEl, set.ViewElTemplate);

            popCombo(vPlCombo,viewPl,ViewCollectorEl, set.ViewPlTemplate);

            popCombo(vPaCombo, viewSc, ViewCollectorSc, set.ViewPaTemplate);

            popCombo(TemplCombo, tlist, tempCollector, set.TemplTemplate);


            popCombo(v3DComboFlr, view3dF, ViewCollector3D, set.View3DTemplateFlr);

            popCombo(vPlComboFlr, viewPlF, ViewCollectorEl, set.ViewPlanTEmplateFlr);

            popCombo(JSComboFlr, viewJSF, ViewCollectorEl, set.ViewJoistTemplateFlr);

            popCombo(TSComboFlr, viewTSF, ViewCollectorEl, set.ViewTrackTemplateFlr);

            popCombo(PaComboFlr, viewScF, ViewCollectorSc, set.ViewPaTemplateFlr);

            popCombo(ShComboFlr, tlistF, tempCollector, set.TemplTemplateFlr);


            popCombo(HorCombo, dimL, dimCollector, set.HorDim);
            popCombo(VertCombo, VertDimL, dimCollector, set.VertDim);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            select3D = (View3D)v3dCombo.SelectedValue;
            selectEl = (Autodesk.Revit.DB.View)vElCombo.SelectedValue;
            selectPl = (Autodesk.Revit.DB.View)vPlCombo.SelectedValue;
            selectPa = (ViewSchedule)vPaCombo.SelectedValue;
            el = (Element)TemplCombo.SelectedValue;

            select3DF = (View3D)v3DComboFlr.SelectedValue;
            selectPlanF = (Autodesk.Revit.DB.View)vPlComboFlr.SelectedValue;
            selectJSF = (Autodesk.Revit.DB.View)JSComboFlr.SelectedValue;
            selectTSF = (Autodesk.Revit.DB.View)TSComboFlr.SelectedValue;
            selectPaF = (ViewSchedule)PaComboFlr.SelectedValue;
            elF = (Element)ShComboFlr.SelectedValue;

            horDim = (DimensionType)HorCombo.SelectedValue;
            vertDim = (DimensionType)VertCombo.SelectedValue;


            set.View3DTemplate = select3D.Name;
            set.ViewElTemplate = selectEl.Name;
            set.ViewPlTemplate = selectPl.Name;
            set.ViewPaTemplate = selectPa.Name;
            set.TemplTemplate = el.Name;

            set.View3DTemplateFlr = select3DF.Name;
            set.ViewPlanTEmplateFlr = selectPlanF.Name;
            set.ViewJoistTemplateFlr = selectJSF.Name;
            set.ViewTrackTemplateFlr = selectTSF.Name;
            set.ViewPaTemplateFlr = selectPaF.Name;
            set.TemplTemplateFlr = elF.Name;

            set.HorDim = horDim.Name;
            set.VertDim = vertDim.Name;

            set.Save();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Close();
        }

        private void popCombo(System.Windows.Forms.ComboBox v3dC, List<View3D> views, FilteredElementCollector ViewC3D, string settings)
        {
            if (views != null)
            {
                v3dC.DataSource = views;
                v3dC.DisplayMember = "Name";

                if (settings != "")
                {
                    foreach (View3D item in ViewC3D)
                    {
                        if (item.Name == settings)
                        {
                            v3dC.SelectedIndex = v3dC.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox Combo, List<Autodesk.Revit.DB.View> viewt, FilteredElementCollector ViewCEl, string settings)
        {
            if (viewt != null)
            {
                Combo.DataSource = viewt;
                Combo.DisplayMember = "Name";

                if (settings != "")
                {
                    foreach (Autodesk.Revit.DB.View item in ViewCEl)
                    {
                        if (item.Name == settings)
                        {
                            Combo.SelectedIndex = Combo.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox vCombo, List<ViewSchedule> view, FilteredElementCollector ViewCollectSc, string settings)
        {
            if (view != null)
            {
                vCombo.DataSource = view;
                vCombo.DisplayMember = "Name";

                if (settings != "")
                {
                    foreach (ViewSchedule item in ViewCollectSc)
                    {
                        if (item.Name == settings)
                        {
                            vCombo.SelectedIndex = vCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox vCombo, List<DimensionType> view, FilteredElementCollector ViewCollectSc, string settings)
        {
            if (view != null)
            {
                vCombo.DataSource = view;
                vCombo.DisplayMember = "Name";

                if (settings != "")
                {
                    foreach (DimensionType item in ViewCollectSc)
                    {
                        if (item.Name == settings)
                        {
                            vCombo.SelectedIndex = vCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox TCombo, List<string> listt, FilteredElementCollector tCollector, string settings)
        {
            IList<Element> title = new List<Element>();
            title = tCollector.ToElements();
            if (listt != null)
            {
                TCombo.DataSource = title;
                TCombo.DisplayMember = "Name";

                if (settings != "")
                {
                    foreach (Element i in title)
                    {
                        if (i.Name == settings)
                        {
                            TCombo.SelectedIndex = TemplCombo.FindStringExact(i.Name);
                        }
                    }
                }
            }
        }

        private void addList(FilteredElementCollector fec, List<View3D> views)
        {
            foreach (View3D v3d in fec)
            {
                if (v3d.IsTemplate)
                {
                    views.Add(v3d);
                }

            }
        }

        private void addList(FilteredElementCollector fec, List<Autodesk.Revit.DB.View> views)
        {
            foreach (Autodesk.Revit.DB.View v3d in fec)
            {
                if (v3d.IsTemplate)
                {
                    views.Add(v3d);
                }

            }
        }

        private void addList(FilteredElementCollector fec, List<ViewSchedule> views)
        {
            foreach (ViewSchedule v3d in fec)
            {
                if (v3d.IsTemplate)
                {
                    views.Add(v3d);
                }

            }
        }

        private void addList(FilteredElementCollector fec, List<string> views)
        {
            IList<Element> title = new List<Element>();
            title = fec.ToElements();
            foreach (Element i in title)
            {
                views.Add(i.Name);
            }
        }

        private void addList(FilteredElementCollector fec, List<DimensionType> views)
        {
            foreach (DimensionType v3d in fec)
            {                
                views.Add(v3d);
            }
        }

    }
}
