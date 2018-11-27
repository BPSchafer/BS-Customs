// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PanelSelectionCommand_Work.cs
 * https://www.bimtrovert.com
 * © BIMtrovert, 2018
 *
 * This file contains the methods which are used by the 
 * command.
 */
#region Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Threading;
//using System.Windows.Forms;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
using System.Linq;
using Bushman.RevitDevTools;
using BIMtrovert.BS_Customs.Properties;
using Point = Autodesk.Revit.DB.Point;


#endregion


namespace BIMtrovert.BS_Customs
{

    public sealed partial class PanelSelectionCommand
    {
        private Viewport AddViewToSheet(Document doc, ViewSheet sheet, ElementType noTitle,XYZ pt, ElementId view )
        {

            if (Viewport.CanAddViewToSheet(doc, sheet.Id, view))
            {
                Viewport p3D = Viewport.Create(doc, sheet.Id, view, pt);
                if (noTitle.Name == "No Title")
                {
                    p3D.ChangeTypeId(noTitle.Id);  
                }
                return p3D;
            }

            return null;
        }



        private bool DoWork(ExternalCommandData commandData,
            ref String message, ElementSet elements)
        {

            if (null == commandData)
            {

                throw new ArgumentNullException(nameof(commandData));
            }

            if (null == message)
            {

                throw new ArgumentNullException(nameof(message));
            }

            if (null == elements)
            {

                throw new ArgumentNullException(nameof(elements));
            }

            ResourceManager res_mng = new ResourceManager(GetType());
            ResourceManager def_res_mng = new ResourceManager(typeof(Properties.Resources));

            UIApplication ui_app = commandData.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;
            Selection selection = ui_doc?.Selection;

            var tr_name = res_mng.GetString("_transaction_name");

            try
            {
                using (var tr = new Transaction(doc, tr_name))
                {

                    if (TransactionStatus.Started == tr.Start()
                        )
                    {
                        ProjectSettingsStorage pStore = new ProjectSettingsStorage();
                        ProjectSettings set = pStore.ReadSettings(doc);
                        
                        ICollection<ElementId> selectedIds = selection.GetElementIds();
                        ICollection<ElementId> panelIds = new List<ElementId>();
                        IList<ElementId> elems = new List<ElementId>();
                        IList<bool> check = new List<bool>();
                        

                        if (0 == selectedIds.Count)
                        {
                            // If no elements selected.
                            TaskDialog.Show("Revit", "You haven't selected any elements.");
                        }
                        else
                        {
                            int totalCount = 0;
                            int currentCount = 0;
                            int localTotal = 10;
                            int localCurrent = 0;
                            String info = "The following assemblies have successfully been created: ";
                            foreach (ElementId id in selectedIds)
                            {
                                Element elem = ui_doc.Document.GetElement(id);
                                elem.LookupParameter("BIMSF_Container");
                                //foreach (Parameter pa in elem.Parameters)
                                //{
                                //if (pa.Definition.Name == "BIMSF_Container" && pa.AsString() != null && elem.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) == null)
                                if (elem.LookupParameter("BIMSF_Container") != null && elem.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) == null)
                                {
                                    panelIds.Add(id);
                                    totalCount += 1;
                                }
                                //}
                            }
                            ProgressForm pf = new ProgressForm(String.Format("{0} of {1} panels processed", currentCount, totalCount), "Creating wall assembly...", (int)100/totalCount, (int)100/localTotal);
                            string localString = "Creating wall assembly...";
                            string globalString =
                                String.Format("{0} of {1} panels processed", currentCount, totalCount);
                            pf.LabelSet(localString, false, false, globalString, false);
                        
                            pf.Show();
                            
                            foreach (ElementId id in panelIds)
                            {
                                
                                Element elem = ui_doc.Document.GetElement(id);
                                //foreach (Parameter pa in elem.Parameters)
                                //{
                                Parameter pa = elem.LookupParameter("BIMSF_Container");
                                if (pa.Definition.Name == "BIMSF_Container" && pa.AsString() != null && elem.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) == null)
                                {

                                    ElementId ida = new ElementId(pa.Id.IntegerValue);
                                    elems.Clear();

                                    ParameterValueProvider provider = new ParameterValueProvider(ida);

                                    FilterStringRuleEvaluator eval = new FilterStringEquals();

                                    FilterRule rule = new FilterStringRule(provider, eval, elem.get_Parameter(pa.Definition).AsString(),false);
                                    ElementParameterFilter filter = new ElementParameterFilter( rule );
                                    FilteredElementCollector fec = new FilteredElementCollector(ui_doc.Document).WhereElementIsNotElementType().WherePasses(filter);

                                    foreach (Element e in fec)
                                    {
                                        elems.Add(e.Id);
                                    }
                                    FilteredElementCollector fecC = new FilteredElementCollector(doc, elems).OfCategory(BuiltInCategory.OST_StructuralColumns);
                                    FilteredElementCollector fecF = new FilteredElementCollector(doc, elems).OfCategory(BuiltInCategory.OST_StructuralFraming);

                                    ElementId categoryId = doc.GetElement(elems.First()).Category.Id; // use category of one of the assembly elements
                                    if (AssemblyInstance.IsValidNamingCategory(doc, categoryId, elems))
                                    {
                                        TransactionStatus tStat = tr.HasStarted() ? tr.GetStatus() : tr.Start();

                                        pf.LabelSet(localString, true, false, globalString, false);

                                        //IList<Element> el = new List<Element>();
                                        //IList<Element> col = new List<Element>();
                                        ReferenceArray colArray = new ReferenceArray();
                                        ReferenceArray framArray = new ReferenceArray();
                                        colArray.Clear();
                                        framArray.Clear();
                                        AssemblyInstance assemblyInstance = AssemblyInstance.Create(doc, elems, categoryId);
                                        XYZ pt1 = assemblyInstance.GetTransform().BasisZ;
                                        XYZ pt2 = assemblyInstance.GetCenter();
                                        /*el.Clear();
                                        col.Clear();

                                        foreach (Element co in fecC)
                                        {
                                            col.Add(co);
                                        }*/

                                        foreach (Element fr in fecF)
                                        {
                                            //el.Add(fr);
                                            FamilyInstance fi = fr as FamilyInstance;
                                            framArray.Append(fi.GetReferenceByName("HardSide"));
                                        }
                                        /*foreach (ElementId item in elems)
                                        {
                                            if (doc.GetElement(item).Category.Name == "Structural Framing")
                                            {
                                                el.Add(doc.GetElement(item));
                                                FamilyInstance fi = doc.GetElement(item) as FamilyInstance;
                                                framArray.Append(fi.GetReferenceByName("HardSide"));
                                            }else if (doc.GetElement(item).Category.Name == "Structural Columns")
                                            {
                                                col.Add(doc.GetElement(item));
                                            }
                                        }*/
                                        //Element il = el.First();
                                        LocationCurve lc = fecF.FirstElement().Location as LocationCurve;
                                        Curve line = lc.Curve;
                                        XYZ start = line.GetEndPoint(0);
                                        XYZ end = line.GetEndPoint(1);
                                        XYZ v = (end - start);
                                        double angle = v.AngleTo(XYZ.BasisX);

                                        foreach (Element column in fecC)
                                        {
                                            FamilyInstance fi = column as FamilyInstance;
                                            LocationPoint colLc = column.Location as LocationPoint;
                                            double colAngle = colLc.Rotation;
                                            if (Math.Round(colAngle,2) == Math.Round(angle,2) || Math.Round(colAngle,2) == Math.Round(angle + Math.PI,2) || Math.Round(colAngle,2) == Math.Round(angle - Math.PI,2))
                                            {
                                                colArray.Append(fi.GetReferenceByName("HardSide"));
                                                //TaskDialog.Show("HardSide", colAngle + "\n" + angle);
                                            }
                                            else
                                            {
                                                colArray.Append(fi.GetReferenceByName("LeftSide"));
                                                //TaskDialog.Show("Revit",colAngle + "\n" + angle);
                                            }

                                        }

                                        //TaskDialog.Show("Revit", angle.ToString());
                                        Transform trf = Transform.CreateRotationAtPoint(pt1, angle, pt2);
                                        assemblyInstance.SetTransform(trf);
                                        tr.Commit(); // commit the transaction that creates the assembly instance before modifying the instance's name
                                        localCurrent += 1;
                                        localString = "Creating 3D view...";
                                        pf.LabelSet(localString, true, false, globalString, false);
                                        if (tr.GetStatus() == TransactionStatus.Committed)
                                        {
                                            tr.Start("Set Assembly Name");
                                            assemblyInstance.AssemblyTypeName = elem.get_Parameter(pa.Definition).AsString();
                                            info += "\n" + assemblyInstance.AssemblyTypeName;
                                            tr.Commit();
                                        }

                                        if (assemblyInstance.AllowsAssemblyViewCreation()) // create assembly views for this assembly instance
                                        {
                                            if (tr.GetStatus() == TransactionStatus.Committed)
                                            {
                                                tr.Start("View Creation");

                                                View3D view3D = AssemblyViewUtils.Create3DOrthographic(doc, assemblyInstance.Id);

                                                localCurrent += 1;
                                                localString = "Creating elevation view...";
                                                pf.LabelSet(localString, true, false, globalString, false);

                                                View elView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationFront);

                                                localCurrent += 1;
                                                localString = "Populating elevation view...";
                                                pf.LabelSet(localString, true, false, globalString, false);

                                                TagMode tm = TagMode.TM_ADDBY_CATEGORY;
                                                bool addLead = false;
                                                XYZ center;
                                                TagOrientation to;

                                                foreach (Element co in fecC)
                                                {
                                                    to = TagOrientation.Vertical;
                                                    LocationPoint cols = co.Location as LocationPoint;
                                                    XYZ colPt = cols.Point;
                                                    double colX = colPt.X;
                                                    double colY = colPt.Y;
                                                    ElementId blId = co.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();
                                                    Level bl = doc.GetElement(blId) as Level;
                                                    double zb = bl.Elevation;
                                                    double blo = co.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).AsDouble();
                                                    double bot = zb + blo;

                                                    ElementId tlId = co.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();
                                                    Level tl = doc.GetElement(tlId) as Level;
                                                    double zt = tl.Elevation;
                                                    double tlo = co.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).AsDouble();
                                                    double top = zt + tlo;
                                                    double zCent = (top + bot) / 2;

                                                    center = new XYZ(colX, colY, zCent);
                                                    Reference refer = new Reference(co);
                                                    IndependentTag.Create(doc, elView.Id, refer, addLead, tm, to, center);
                                                }

                                                foreach (Element fr in fecF)
                                                {
                                                    to = TagOrientation.Horizontal;
                                                    LocationCurve loc = fr.Location as LocationCurve;
                                                    XYZ start2 = loc.Curve.GetEndPoint(0);
                                                    XYZ end2 = loc.Curve.GetEndPoint(1);
                                                    center = (start2 + end2) / 2;
                                                    Reference refer = new Reference(fr);
                                                    IndependentTag.Create(doc, elView.Id, refer, addLead, tm, to, center);
                                                }
                                                /*foreach (ElementId eI in elems)
                                                {
                                                    Element e = doc.GetElement(eI);
                                                    
                                                    if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_StructuralColumns || (BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_StructuralFraming)
                                                    {
                                                        XYZ center;
                                                        TagOrientation to;
                                                        if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_StructuralColumns)
                                                        {
                                                            to = TagOrientation.Vertical;
                                                            LocationPoint cols = e.Location as LocationPoint;
                                                            XYZ colPt = cols.Point;
                                                            double colX = colPt.X;
                                                            double colY = colPt.Y;
                                                            ElementId blId = e.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();
                                                            Level bl = doc.GetElement(blId) as Level;
                                                            double zb = bl.Elevation;
                                                            double blo = e.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).AsDouble();
                                                            double bot = zb + blo;

                                                            ElementId tlId = e.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();
                                                            Level tl = doc.GetElement(tlId) as Level;
                                                            double zt = tl.Elevation;
                                                            double tlo = e.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).AsDouble();
                                                            double top = zt + tlo;
                                                            double zCent = (top + bot) / 2;

                                                            center = new XYZ(colX,colY,zCent);
                                                        }
                                                        else
                                                        {
                                                            to = TagOrientation.Horizontal;
                                                            LocationCurve loc = e.Location as LocationCurve;
                                                            XYZ start2 = loc.Curve.GetEndPoint(0);
                                                            XYZ end2 = loc.Curve.GetEndPoint(1);
                                                            center = (start2 + end2) / 2;
                                                        }

                                                        
                                                        Reference refer = new Reference(e);
                                                        IndependentTag.Create(doc, elView.Id, refer, addLead, tm, to, center);

                                                    }
                                                    
                                                }*/

                                                try
                                                {
                                                    
                                                    //LocationCurve framC = fecF.FirstElement().Location as LocationCurve;
                                                    FamilyInstance fi = fecC.FirstElement() as FamilyInstance;
                                                    //Transform fiP = fi.GetTransform();
                                                    
                                                    LocationPoint col1 = fecC.FirstElement().Location as LocationPoint;
                                                    //XYZ test = fiP.OfPoint(col1.Point);

                                                    ElementId col1Bot = fecC.FirstElement().get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();
                                                    Level col1LB = doc.GetElement(col1Bot) as Level;
                                                    double bottom = col1LB.Elevation;

                                                    ElementId col1Top = fecC.FirstElement().get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();
                                                    Level col1LT = doc.GetElement(col1Top) as Level;
                                                    double top = col1LT.Elevation;

                                                    //Line framT = framC.Curve as Line;
                                                    //XYZ framP = framT.Origin;
                                                    Line framL = Line.CreateBound(new XYZ(col1.Point.X, col1.Point.Y, bottom), new XYZ(col1.Point.X, col1.Point.Y, top));
                                                    Dimension di = doc.Create.NewDimension(elView, framL, framArray);
                                                    di.DimensionType = doc.GetElement(set.HorizontalDimFlr) as DimensionType;
                                                }
                                                catch (Exception e)
                                                {
                                                    TaskDialog.Show("Revit", e.Message + "\n" + e.Source);
                                                }

                                                try
                                                {
                                                    LocationCurve framC = fecF.FirstElement().Location as LocationCurve;

                                                    ElementId col1Bot = fecC.FirstElement().get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();
                                                    Level col1LB = doc.GetElement(col1Bot) as Level;
                                                    double bottom = col1LB.Elevation;

                                                    Line framT = framC.Curve as Line;
                                                    XYZ framS = framT.GetEndPoint(0);
                                                    XYZ framE = framT.GetEndPoint(1);
                                                    //Line framL = framT.CreateOffset(-1, XYZ.BasisZ) as Line;
                                                    //XYZ framP = framT.Origin;
                                                    Line framL = Line.CreateBound(new XYZ(framS.X, framS.Y, bottom-1.5), new XYZ(framE.X, framE.Y, bottom-1.5));
                                                    Dimension di = doc.Create.NewDimension(elView, framL, colArray);
                                                    di.DimensionType = doc.GetElement(set.HeightDimWa) as DimensionType;
                                                    LocationPoint clp = fecC.FirstElement().Location as LocationPoint;

                                                    
                                                }
                                                catch (Exception e)
                                                {
                                                    TaskDialog.Show("Revit", e.Message + "\n" + e.Source);
                                                }
                                                localCurrent += 1;
                                                localString = "Creating plan view...";
                                                pf.LabelSet(localString, true, false, globalString, false);
                                                View plView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationTop);

                                                localCurrent += 1;
                                                localString = "Creating material takeoff schedule...";
                                                pf.LabelSet(localString, true, false, globalString, false);
                                                ViewSchedule partList = AssemblyViewUtils.CreatePartList(doc, assemblyInstance.Id);

                                                XYZ pt3D = new XYZ(0.1, 0.45, 0);
                                                XYZ ptEl = new XYZ(0.5, 0.5, 0);
                                                XYZ ptPl = new XYZ(0.5, 0.5, 0);
                                                XYZ ptPa = new XYZ(0.03, 0.1, 0);

                                                if (set != null)
                                                {
                                                    view3D.ViewTemplateId = set.View3DTemplate;
                                                }
                                                
                                                if (set != null)
                                                {
                                                    elView.ViewTemplateId = set.ViewElTemplate;
                                                }
                                                
                                                if (set != null)
                                                {
                                                    plView.ViewTemplateId = set.ViewPlTemplate;
                                                }
                                                
                                                if (set != null)
                                                {
                                                    partList.ViewTemplateId = set.ViewPaTemplate;
                                                }
                                                if (set != null)
                                                {
                                                    try
                                                    {
                                                        localCurrent += 1;
                                                        localString = "Creating spool sheet...";
                                                        pf.LabelSet(localString, true, false, globalString, false);
                                                        ElementType noTitle = null;
                                                        FilteredElementCollector fen = new FilteredElementCollector(doc).OfClass(typeof(ElementType));
                                                        foreach (Element item in fen)
                                                        {
                                                            if (item.Name == "No Title")
                                                            {
                                                                noTitle = item as ElementType;
                                                            }
                                                        }

                                                        ViewSheet sheet = AssemblyViewUtils.CreateSheet(doc, assemblyInstance.Id, set.TemplateTemplate);
                                                        localCurrent += 1;
                                                        localString = "Adding views to sheet...";
                                                        pf.LabelSet(localString, true, false, globalString, false);
                                                        Viewport v3D = AddViewToSheet(doc, sheet, noTitle, pt3D, view3D.Id);
                                                                
                                                        Viewport vEl = AddViewToSheet(doc, sheet, noTitle, ptEl, elView.Id);
                                                        BoundingBoxXYZ bbEl = vEl.get_BoundingBox(sheet);
                                                                

                                                        Viewport vPl = AddViewToSheet(doc, sheet, noTitle, ptPl, plView.Id);
                                                        BoundingBoxXYZ bbPl = vPl.get_BoundingBox(sheet);
                                                        
                                                                

                                                        ScheduleSheetInstance vPa =  ScheduleSheetInstance.Create(doc, sheet.Id, partList.Id, ptPa);
                                                        localCurrent += 1;
                                                        localString = "Cleaning up spool sheet...";
                                                        pf.LabelSet(localString, true, false, globalString, false);
                                                        BoundingBoxXYZ bb = sheet.CropBox;
                                                        BoundingBoxXYZ bbvPa = vPa.get_BoundingBox(sheet);
                                                        double minX = bb.Min.X;
                                                        double maxX = bb.Max.X;
                                                        double xLen = maxX - minX;
                                                        //TaskDialog.Show("Revit",minX + ", " + maxX + "\n" +bbvPa.Min.Y + ", " + bbvPa.Max.Y +  "\n" + bbPl.Min.X + ", " + bbPl.Min.Y);
                                                        vPa.Point = new XYZ(vPa.Point.X , vPa.Point.Y - bbvPa.Min.Y , vPa.Point.Z);
                                                        double schedX = bbvPa.Max.X + 0.03;
                                                        double centEl = (bbEl.Max.X- bbEl.Min.X)/2;
                                                        double centElY = (bbEl.Max.Y - bbEl.Min.Y) / 2;
                                                        double centPlY = (bbPl.Max.Y - bbPl.Min.Y) / 2;
                                                        vEl.SetBoxCenter(new XYZ(schedX+centEl, (centPlY + 0.05)*2 + centElY, vEl.GetBoxCenter().Z));
                                                        vPl.SetBoxCenter(new XYZ(schedX + centEl, centPlY + 0.05, vPl.GetBoxCenter().Z));


                                                        sheet.SheetNumber = elem.get_Parameter(pa.Definition).AsString();
                                                        sheet.Name = "Framing";
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        TaskDialog.Show("Error", ex.Message);
                                                        break;
                                                    }
                                                }
                                                
                                                localCurrent += 1;
                                                localString = String.Format("Wall assembly {0} completed...",
                                                    elem.get_Parameter(pa.Definition).AsString());
                                                pf.LabelSet(localString, true, false, globalString, false);
                                                tr.Commit();
                                                localCurrent = 0;
                                            }
                                        }
                                        currentCount += 1;
                                        globalString = String.Format("{0} of {1} panels processed", currentCount, totalCount);
                                        pf.LabelSet(localString, false, true, globalString, true);

                                    }
                                    //info += "\n\t" + pa.Definition.Name + ": " + pa.AsString();
                                }                                   
                                //}
                            }
                            pf.Close();
                            tr.Start();
                            doc.Regenerate();
                            tr.Commit();
                            TaskDialog.Show("Revit", info);
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                
            }
            finally
            {
                res_mng.ReleaseAllResources();
                def_res_mng.ReleaseAllResources();
            }
            return false;
        }
    }
}
