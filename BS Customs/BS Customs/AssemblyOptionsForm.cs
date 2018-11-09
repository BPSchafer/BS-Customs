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
        UIApplication ui_app = null;
        ProjectSettings projectSettings = new ProjectSettings();
        ProjectSettingsStorage pstore = new ProjectSettingsStorage();

        public AssemblyOptionsForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            ui_app = commandData.Application;
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


            //popCombo(v3dCombo, view3d, ViewCollector3D, doc);
            try
            {
                popCombo(v3dCombo, view3d, ViewCollector3D, doc, pstore.ReadSettings(doc)?.View3DTemplate);
            }
            catch (Exception)
            {
                popCombo(v3dCombo, view3d, ViewCollector3D, doc);

            }

            try
            {
                popCombo(vElCombo, viewEl, ViewCollectorEl, doc, pstore.ReadSettings(doc)?.ViewElTemplate);
            }
            catch (Exception)
            {
                popCombo(vElCombo, viewEl, ViewCollectorEl, doc);
            }

            try
            {
                popCombo(vPlCombo, viewPl, ViewCollectorEl, doc, pstore.ReadSettings(doc)?.ViewPlTemplate);
            }
            catch (Exception)
            {
                popCombo(vPlCombo, viewPl, ViewCollectorEl, doc);
            }

            try
            {
                popCombo(vPaCombo, viewSc, ViewCollectorSc, doc, pstore.ReadSettings(doc)?.ViewPaTemplate);
            }
            catch (Exception)
            {
                popCombo(vPaCombo, viewSc, ViewCollectorSc, doc);
            }

            try
            {
                popCombo(TemplCombo, tlist, tempCollector, doc, pstore.ReadSettings(doc)?.TemplateTemplate);
            }
            catch (Exception)
            {
                popCombo(TemplCombo, tlist, tempCollector, doc);
            }


            try
            {
                popCombo(v3DComboFlr, view3dF, ViewCollector3D, doc, pstore.ReadSettings(doc)?.View3DTemplateFlr);
            }
            catch (Exception)
            {
                popCombo(v3DComboFlr, view3dF, ViewCollector3D, doc);
            }

            try
            {
                popCombo(vPlComboFlr, viewPlF, ViewCollectorEl, doc, pstore.ReadSettings(doc)?.ViewPlanTemplateFlr);
            }
            catch (Exception)
            {
                popCombo(vPlComboFlr, viewPlF, ViewCollectorEl, doc);
            }

            try
            {
                popCombo(JSComboFlr, viewJSF, ViewCollectorEl, doc, pstore.ReadSettings(doc)?.ViewJoistTemplateFlr);
            }
            catch (Exception)
            {
                popCombo(JSComboFlr, viewJSF, ViewCollectorEl, doc);
            }

            try
            {
                popCombo(TSComboFlr, viewTSF, ViewCollectorEl, doc, pstore.ReadSettings(doc)?.ViewTrackTemplateFrl);
            }
            catch (Exception)
            {
                popCombo(TSComboFlr, viewTSF, ViewCollectorEl, doc);
            }

            try
            {
                popCombo(PaComboFlr, viewScF, ViewCollectorSc, doc, pstore.ReadSettings(doc)?.ViewPaTemplateFlr);
            }
            catch (Exception)
            {
                popCombo(PaComboFlr, viewScF, ViewCollectorSc, doc);
            }

            try
            {
                popCombo(ShComboFlr, tlistF, tempCollector, doc, pstore.ReadSettings(doc)?.TemplateTemplateFrl);
            }
            catch (Exception)
            {
                popCombo(ShComboFlr, tlistF, tempCollector, doc);
            }


            try
            {
                popCombo(HorCombo, dimL, dimCollector, doc, pstore.ReadSettings(doc)?.HorizontalDimFlr);
            }
            catch (Exception)
            {
                popCombo(HorCombo, dimL, dimCollector, doc);
            }

            try
            {
                popCombo(VertCombo, VertDimL, dimCollector, doc, pstore.ReadSettings(doc)?.VerticalDimFlr);
            }
            catch (Exception)
            {
                popCombo(VertCombo, VertDimL, dimCollector, doc);
            }
            
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;

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

            projectSettings.View3DTemplate = select3D.Id;
            projectSettings.ViewElTemplate = selectEl.Id;
            projectSettings.ViewPlTemplate = selectPl.Id;
            projectSettings.ViewPaTemplate = selectPa.Id;
            projectSettings.TemplateTemplate = el.Id;

            projectSettings.View3DTemplateFlr = select3DF.Id;
            projectSettings.ViewPlanTemplateFlr = selectPlanF.Id;
            projectSettings.ViewJoistTemplateFlr = selectJSF.Id;
            projectSettings.ViewTrackTemplateFrl = selectTSF.Id;
            projectSettings.ViewPaTemplateFlr = selectPaF.Id;
            projectSettings.TemplateTemplateFrl = elF.Id;

            projectSettings.HorizontalDimFlr = horDim.Id;
            projectSettings.VerticalDimFlr = vertDim.Id;
            projectSettings.HorizontalDimWa = horDim.Id;
            projectSettings.HeightDimWa = vertDim.Id;

            pstore.WriteSettings(doc, projectSettings);

            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void popCombo(System.Windows.Forms.ComboBox v3dC, List<View3D> views, FilteredElementCollector ViewC3D, Document doc, ElementId settings = null)
        {
            if (views != null)
            {
                v3dC.DataSource = views;
                v3dC.DisplayMember = "Name";

                if (settings != null)
                {
                    foreach (View3D item in ViewC3D)
                    {
                        if (item.Name == doc.GetElement(settings).Name)
                        {
                            v3dC.SelectedIndex = v3dC.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox Combo, List<Autodesk.Revit.DB.View> viewt, FilteredElementCollector ViewCEl, Document doc, ElementId settings = null)
        {
            if (viewt != null)
            {
                Combo.DataSource = viewt;
                Combo.DisplayMember = "Name";

                if (settings != null)
                {
                    foreach (Autodesk.Revit.DB.View item in ViewCEl)
                    {
                        if (item.Name == doc.GetElement(settings).Name)
                        {
                            Combo.SelectedIndex = Combo.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox vCombo, List<ViewSchedule> view, FilteredElementCollector ViewCollectSc, Document doc, ElementId settings = null)
        {
            if (view != null)
            {
                vCombo.DataSource = view;
                vCombo.DisplayMember = "Name";

                if (settings != null)
                {
                    foreach (ViewSchedule item in ViewCollectSc)
                    {
                        if (item.Name == doc.GetElement(settings).Name)
                        {
                            vCombo.SelectedIndex = vCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox vCombo, List<DimensionType> view, FilteredElementCollector ViewCollectSc, Document doc, ElementId settings = null)
        {
            if (view != null)
            {
                vCombo.DataSource = view;
                vCombo.DisplayMember = "Name";

                if (settings != null)
                {
                    foreach (DimensionType item in ViewCollectSc)
                    {
                        if (item.Name == doc.GetElement(settings).Name)
                        {
                            vCombo.SelectedIndex = vCombo.FindStringExact(item.Name);
                        }
                    }

                }

            }
        }

        private void popCombo(System.Windows.Forms.ComboBox TCombo, List<string> listt, FilteredElementCollector tCollector, Document doc, ElementId settings = null)
        {
            IList<Element> title = new List<Element>();
            title = tCollector.ToElements();
            if (listt != null)
            {
                TCombo.DataSource = title;
                TCombo.DisplayMember = "Name";

                if (settings != null)
                {
                    foreach (Element i in title)
                    {
                        if (i.Name == doc.GetElement(settings).Name)
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
