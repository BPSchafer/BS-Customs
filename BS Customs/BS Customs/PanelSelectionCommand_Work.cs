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
        private void AddViewToSheet(Document doc, ViewSheet sheet, ElementType notitle,XYZ pt, View view = null, View3D view3d = null)
        {
            if (view3d != null)
            {
                if (Viewport.CanAddViewToSheet(doc, sheet.Id, view3d.Id))
                {
                    Viewport p3d = Viewport.Create(doc, sheet.Id, view3d.Id, pt);

                    foreach (ElementId eid in p3d.GetValidTypes())
                    {
                        ElementType typ = doc.GetElement(eid) as ElementType;
                        if (typ.Name == "No Title")
                        {
                            notitle = typ;
                            break;
                        }

                    }
                    if (notitle != null)
                    {
                        p3d.ChangeTypeId(notitle.Id);
                    }
                }

            }
            else
            {
                if (Viewport.CanAddViewToSheet(doc, sheet.Id, view.Id))
                {
                    Viewport p3d = Viewport.Create(doc, sheet.Id, view.Id, pt);

                    foreach (ElementId eid in p3d.GetValidTypes())
                    {
                        ElementType typ = doc.GetElement(eid) as ElementType;
                        if (typ.Name == "No Title")
                        {
                            notitle = typ;
                            break;
                        }

                    }
                    if (notitle != null)
                    {
                        p3d.ChangeTypeId(notitle.Id);
                    }
                }

            }
            
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
                                            assemblyInstance = AssemblyInstance.Create(doc, elems, categoryId);
                                            XYZ pt1 = assemblyInstance.GetTransform().BasisZ;
                                            XYZ pt3 = assemblyInstance.GetTransform().Origin;
                                            XYZ pt4 = assemblyInstance.GetCenter();
                                            Element pt5 = doc.GetElement(elems.First());
                                            Options option = new Options();

                                            XYZ pt6 = pt5.get_Geometry(option).GetBoundingBox().Transform.BasisZ;
                                            //XYZ pt2 = new XYZ(pt1.X, pt1.Y, pt1.Z+1);
                                            
                                            //LocationPoint ep = doc.GetElement(elems[0]).Location as LocationPoint;
                                            //double angle = ep.Rotation*(180/Math.PI);
                                            //Line line = Line.CreateBound(pt1, pt2);
                                            //Transform trf = Transform.CreateRotationAtPoint(pt1, 90, pt4);
                                            assemblyInstance.GetTransform().OfVector(pt6);
                                            tr.Commit(); // commit the transaction that creates the assembly instance before modifying the instance's name

                                            if (tr.GetStatus() == TransactionStatus.Committed)
                                            {
                                                tr.Start("Set Assembly Name");
                                                assemblyInstance.AssemblyTypeName = elem.get_Parameter(pa.Definition).AsString();
                                                info += "\n" + assemblyInstance.AssemblyTypeName;
                                                tr.Commit();
                                            }

                                            /*if (assemblyInstance.AllowsAssemblyViewCreation()) // create assembly views for this assembly instance
                                            {
                                                if (tr.GetStatus() == TransactionStatus.Committed)
                                                {
                                                    tr.Start("View Creation");

                                                    View3D view3d = AssemblyViewUtils.Create3DOrthographic(doc, assemblyInstance.Id);
                                                    View ElView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationFront);
                                                    //BoundingBoxXYZ bb = assemblyInstance.get_BoundingBox();
                                                    
                                                    View PlView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.HorizontalDetail);
                                                    ViewSchedule partList = AssemblyViewUtils.CreatePartList(doc, assemblyInstance.Id);

                                                    XYZ pt3d = new XYZ(0.05, 0.4, 0);
                                                    XYZ ptEl = new XYZ(0.5, 0.5, 0);
                                                    XYZ ptPl = new XYZ(0.5, 0.1, 0);
                                                    XYZ ptPa = new XYZ(0.03, 0.25, 0);

                                                    if (set != null)
                                                    {
                                                        try
                                                        {
                                                            FilteredElementCollector ViewCollector3D = new FilteredElementCollector(doc).OfClass(typeof(View3D));
                                                            foreach (View3D item in ViewCollector3D)
                                                            {
                                                                if (item.Id == set.View3DTemplate)
                                                                {
                                                                    view3d.ViewTemplateId = item.Id;

                                                                }
                                                            }

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
                                                            FilteredElementCollector ViewCollectorEl = new FilteredElementCollector(doc).OfClass(typeof(View));
                                                            foreach (View item in ViewCollectorEl)
                                                            {
                                                                if (item.Id == set.ViewElTemplate)
                                                                {
                                                                    ElView.ViewTemplateId = item.Id;
                                                                }
                                                            }

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
                                                            FilteredElementCollector ViewCollectorPl = new FilteredElementCollector(doc).OfClass(typeof(View));
                                                            foreach (View item in ViewCollectorPl)
                                                            {
                                                                if (item.Id == set.ViewPlTemplate)
                                                                {
                                                                    PlView.ViewTemplateId = item.Id;
                                                                }
                                                            }

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
                                                            FilteredElementCollector ViewCollectorPa = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule));
                                                            foreach (ViewSchedule item in ViewCollectorPa)
                                                            {
                                                                if (item.Id == set.ViewPaTemplate)
                                                                {
                                                                    partList.ViewTemplateId = item.Id;
                                                                }
                                                            }

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
                                                            FilteredElementCollector tempCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks);
                                                            foreach (Element item in tempCollector)
                                                            {
                                                                if (item.Id == set.TemplateTemplate)
                                                                {
                                                                    ViewSheet sheet = AssemblyViewUtils.CreateSheet(doc, assemblyInstance.Id, item.Id);
                                                                    AddViewToSheet(doc, sheet, notitle, pt3d, view3d: view3d);
                                                                    
                                                                    AddViewToSheet(doc, sheet, notitle, ptEl, ElView);
                                                                    

                                                                    AddViewToSheet(doc, sheet, notitle, ptPl, PlView);
                                                                    

                                                                    ScheduleSheetInstance vPa =  ScheduleSheetInstance.Create(doc, sheet.Id, partList.Id, ptPa);

                                                                    sheet.SheetNumber = elem.get_Parameter(pa.Definition).AsString();
                                                                    sheet.Name = "Framing";
                                                                }
                                                            }
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
                                            }*/
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
