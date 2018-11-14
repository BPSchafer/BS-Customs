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
#endregion


namespace BIMtrovert.BS_Customs
{

    public sealed partial class PanelSelectionCommand
    {
        private Viewport AddViewToSheet(Document doc, ViewSheet sheet, ElementType notitle,XYZ pt, ElementId view )
        {

            if (Viewport.CanAddViewToSheet(doc, sheet.Id, view))
            {
                Viewport p3d = Viewport.Create(doc, sheet.Id, view, pt);

                /*foreach (ElementId eid in p3d.GetValidTypes())
                {
                    ElementType typ = doc.GetElement(eid) as ElementType;
                    if (typ.Name == "No Title")
                    {
                        notitle = typ;
                        break;
                    }

                }*/
                if (notitle.Name == "No Title")
                {
                    p3d.ChangeTypeId(notitle.Id);  
                }
                //BoundingBoxXYZ bbp = p3d.get_BoundingBox()
                return p3d;
            }

            return null;
        }

        private bool DoWork(ExternalCommandData commandData,
            ref String message, ElementSet elements)
        {

            if (null == commandData)
            {

                throw new ArgumentNullException(nameof(
                    commandData));
            }

            if (null == message)
            {

                throw new ArgumentNullException(nameof(message)
                    );
            }

            if (null == elements)
            {

                throw new ArgumentNullException(nameof(elements
                    ));
            }

            ResourceManager res_mng = new ResourceManager(
                  GetType());
            ResourceManager def_res_mng = new ResourceManager(
                typeof(Properties.Resources));

            UIApplication ui_app = commandData.Application;
            UIDocument ui_doc = ui_app?.ActiveUIDocument;
            Application app = ui_app?.Application;
            Document doc = ui_doc?.Document;
            Selection selection = ui_doc?.Selection;

            var tr_name = res_mng.GetString("_transaction_name"
                );

            try
            {
                using (var tr = new Transaction(doc, tr_name)
                    )
                {

                    if (TransactionStatus.Started == tr.Start()
                        )
                    {
                        ProjectSettingsStorage pstore = new ProjectSettingsStorage();
                        ProjectSettings set = pstore.ReadSettings(doc);
                        
                        ICollection<ElementId> selectedIds = selection.GetElementIds();
                        IList<ElementId> elems = new List<ElementId>();
                        IList<bool> check = new List<bool>();
                        

                        if (0 == selectedIds.Count)
                        {
                            // If no elements selected.
                            TaskDialog.Show("Revit", "You haven't selected any elements.");
                        }
                        else
                        {
                            String info = "The following assemblies have successfully been created: ";
                            foreach (ElementId id in selectedIds)
                            {
                                AssemblyInstance assemblyInstance = null;
                                Element elem = ui_doc.Document.GetElement(id);
                                foreach (Parameter pa in elem.Parameters)
                                {
                                    
                                    if (pa.Definition.Name == "BIMSF_Container" && pa.AsString() != null && elem.get_Parameter(BuiltInParameter.ASSEMBLY_NAME) == null)
                                    {
                                        ElementId ida = new ElementId(pa.Id.IntegerValue);
                                        elems.Clear();

                                        ParameterValueProvider provider
                                          = new ParameterValueProvider(ida);

                                        FilterStringRuleEvaluator eval = new FilterStringEquals();

                                        FilterRule rule = new FilterStringRule(provider, eval, elem.get_Parameter(pa.Definition).AsString(),false);
                                        ElementParameterFilter filter = new ElementParameterFilter( rule );
                                        FilteredElementCollector fec = new FilteredElementCollector(ui_doc.Document).WhereElementIsNotElementType().WherePasses(filter);
                                        foreach (Element e in fec)
                                        {
                                            elems.Add(e.Id);
                                            //info += "\n\t" + e.Id.ToString();
                                        }


                                        ElementId categoryId = doc.GetElement(elems.First()).Category.Id; // use category of one of the assembly elements
                                        if (AssemblyInstance.IsValidNamingCategory(doc, categoryId, elems))
                                        {
                                            if (tr.HasStarted())
                                            {

                                            }
                                            else
                                            {
                                                tr.Start();
                                            }
                                            ICollection<Element> el = new List<Element>();
                                            assemblyInstance = AssemblyInstance.Create(doc, elems, categoryId);
                                            XYZ pt1 = assemblyInstance.GetTransform().BasisZ;
                                            XYZ pt3 = assemblyInstance.GetTransform().Origin;
                                            XYZ pt4 = assemblyInstance.GetCenter();
                                            Element pt5 = doc.GetElement(elems.First());
                                            el.Clear();
                                            foreach (ElementId item in elems)
                                            {
                                                if (doc.GetElement(item).Category.Name == "Structural Framing")
                                                {
                                                    el.Add(doc.GetElement(item));
                                                }
                                            }
                                            Element il = el.First();
                                            LocationCurve lc = il.Location as LocationCurve;
                                            Curve line = lc.Curve;
                                            XYZ start = line.GetEndPoint(0);
                                            XYZ end = line.GetEndPoint(1);
                                            XYZ v = (end - start);
                                            double angle = v.AngleTo(XYZ.BasisX);
                                            //TaskDialog.Show("Revit", angle.ToString());

                                            Transform trf = Transform.CreateRotationAtPoint(pt1, angle, pt4);
                                            assemblyInstance.SetTransform(trf);
                                            tr.Commit(); // commit the transaction that creates the assembly instance before modifying the instance's name

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

                                                    View3D view3d = AssemblyViewUtils.Create3DOrthographic(doc, assemblyInstance.Id);

                                                    View ElView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationFront);
                                                    TagMode tm = TagMode.TM_ADDBY_CATEGORY;
                                                    bool addLead = false;
                                                    foreach (ElementId eI in elems)
                                                    {

                                                        Element e = doc.GetElement(eI);
                                                        XYZ center = null;
                                                        if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_StructuralColumns || (BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_StructuralFraming)
                                                        {
                                                            TagOrientation to = TagOrientation.Horizontal;
                                                            if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_StructuralColumns)
                                                            {
                                                                to = TagOrientation.Vertical;
                                                                LocationPoint col = e.Location as LocationPoint;
                                                                XYZ colpt = col.Point;
                                                                double colX = colpt.X;
                                                                double colY = colpt.Y;
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
                                                                double zcent = (top + bot) / 2;

                                                                center = new XYZ(colX,colY,zcent);
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
                                                            IndependentTag it = IndependentTag.Create(doc, ElView.Id, refer, addLead, tm, to, center);

                                                        }
                                                        
                                                    }

                                                    View PlView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationTop);
                                                    //ElementTransformUtils.MoveElement(doc, PlView.Id, new XYZ(0, 0, -10));
                                                    ViewSchedule partList = AssemblyViewUtils.CreatePartList(doc, assemblyInstance.Id);

                                                    XYZ pt3d = new XYZ(0.1, 0.45, 0);
                                                    XYZ ptEl = new XYZ(0.5, 0.5, 0);
                                                    XYZ ptPl = new XYZ(0.5, 0.5, 0);
                                                    XYZ ptPa = new XYZ(0.03, 0.1, 0);

                                                    if (set != null)
                                                    {
                                                        try
                                                        {
                                                            view3d.ViewTemplateId = set.View3DTemplate;
                                                        }
                                                        catch(Exception ex)
                                                        {
                                                            TaskDialog.Show("Error",ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    
                                                    
                                                    if (set != null)
                                                    {
                                                        try
                                                        {
                                                            ElView.ViewTemplateId = set.ViewElTemplate;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    
                                                    if (set != null)
                                                    {
                                                        try
                                                        {
                                                            PlView.ViewTemplateId = set.ViewPlTemplate;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }

                                                    
                                                    if (set != null)
                                                    {
                                                        try
                                                        {
                                                            partList.ViewTemplateId = set.ViewPaTemplate;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    if (set != null)
                                                    {
                                                        try
                                                        {
                                                            ElementType notitle = null;
                                                            FilteredElementCollector fen = new FilteredElementCollector(doc).OfClass(typeof(ElementType));
                                                            foreach (Element item in fen)
                                                            {
                                                                if (item.Name == "No Title")
                                                                {
                                                                    notitle = item as ElementType;
                                                                    //TaskDialog.Show("Revit", notitle.Name);
                                                                }
                                                            }

                                                            ViewSheet sheet = AssemblyViewUtils.CreateSheet(doc, assemblyInstance.Id, set.TemplateTemplate);
                                                            Viewport v3d = AddViewToSheet(doc, sheet, notitle, pt3d, view3d.Id);
                                                            BoundingBoxXYZ bb3d = v3d.get_BoundingBox(sheet);
                                                                    
                                                            Viewport vEl = AddViewToSheet(doc, sheet, notitle, ptEl, ElView.Id);
                                                            BoundingBoxXYZ bbEl = vEl.get_BoundingBox(sheet);
                                                                    

                                                            Viewport vPl = AddViewToSheet(doc, sheet, notitle, ptPl, PlView.Id);
                                                            BoundingBoxXYZ bbPl = vPl.get_BoundingBox(sheet);
                                                            
                                                                    

                                                            ScheduleSheetInstance vPa =  ScheduleSheetInstance.Create(doc, sheet.Id, partList.Id, ptPa);
                                                            BoundingBoxXYZ bb = sheet.CropBox;
                                                            BoundingBoxXYZ bbvPa = vPa.get_BoundingBox(sheet);
                                                            double minX = bb.Min.X;
                                                            double maxX = bb.Max.X;
                                                            double xLen = maxX - minX;
                                                            TaskDialog.Show("Revit",
                                                                minX.ToString() + ", " + maxX.ToString() + "\n" +
                                                                bbvPa.Min.Y + ", " + bbvPa.Max.Y +  "\n" + bbPl.Min.X + ", " + bbPl.Min.Y);
                                                            vPa.Point = new XYZ(vPa.Point.X , vPa.Point.Y - bbvPa.Min.Y , vPa.Point.Z);
                                                            double schedX = bbvPa.Max.X + 0.03;
                                                            double centEl = (bbEl.Max.X- bbEl.Min.X)/2;
                                                            double centElY = (bbEl.Max.Y - bbEl.Min.Y) / 2;
                                                            double centPlY = (bbPl.Max.Y - bbPl.Min.Y) / 2;
                                                            vEl.SetBoxCenter(new XYZ(schedX+centEl, (centPlY + 0.05)*2 + centElY, vEl.GetBoxCenter().Z));
                                                            vPl.SetBoxCenter(new XYZ(schedX + centEl, centPlY + 0.05, vPl.GetBoxCenter().Z));
                                                            //vPl.SetBoxCenter();
                                                            //vEl.SetBoxCenter();
                                                            //v3d.SetBoxCenter();

                                                            sheet.SheetNumber = elem.get_Parameter(pa.Definition).AsString();
                                                            sheet.Name = "Framing";
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    doc.Regenerate();
                                                    //ViewSheet sheet = AssemblyViewUtils.CreateSheet(doc,assemblyInstance.Id)
                                                    tr.Commit();
                                                }
                                            }
                                        }
                                        //info += "\n\t" + pa.Definition.Name + ": " + pa.AsString();
                                    }                                   
                                }
                            }

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
