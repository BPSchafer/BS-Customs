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
                            String info = "Ids of selected elements in the document are: ";
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
                                            tr.Commit(); // commit the transaction that creates the assembly instance before modifying the instance's name

                                            if (tr.GetStatus() == TransactionStatus.Committed)
                                            {
                                                tr.Start("Set Assembly Name");
                                                assemblyInstance.AssemblyTypeName = elem.get_Parameter(pa.Definition).AsString();
                                                tr.Commit();
                                            }

                                            if (assemblyInstance.AllowsAssemblyViewCreation()) // create assembly views for this assembly instance
                                            {
                                                if (tr.GetStatus() == TransactionStatus.Committed)
                                                {
                                                    tr.Start("View Creation");

                                                    if (Properties.Settings.Default.View3DTemplate != "")
                                                    {
                                                        try
                                                        {
                                                            FilteredElementCollector ViewCollector3D = new FilteredElementCollector(doc).OfClass(typeof(View3D));
                                                            foreach (View3D item in ViewCollector3D)
                                                            {
                                                                if (item.Name == Properties.Settings.Default.View3DTemplate)
                                                                {
                                                                    View3D view3d = AssemblyViewUtils.Create3DOrthographic(doc, assemblyInstance.Id, item.Id,true);

                                                                }
                                                            }

                                                        }
                                                        catch(Exception ex)
                                                        {
                                                            TaskDialog.Show("Error",ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    
                                                    
                                                    if (Properties.Settings.Default.ViewElTemplate != "")
                                                    {
                                                        try
                                                        {
                                                            FilteredElementCollector ViewCollectorEl = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View));
                                                            foreach (View3D item in ViewCollectorEl)
                                                            {
                                                                if (item.Name == Properties.Settings.Default.ViewElTemplate)
                                                                {
                                                                    View ElView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationFront, item.Id,true);
                                                                }
                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    
                                                    if (Properties.Settings.Default.ViewPlTemplate != "")
                                                    {
                                                        try
                                                        {
                                                            FilteredElementCollector ViewCollectorPl = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View));
                                                            foreach (View3D item in ViewCollectorPl)
                                                            {
                                                                if (item.Name == Properties.Settings.Default.ViewPlTemplate)
                                                                {
                                                                    View PlView = AssemblyViewUtils.CreateDetailSection(doc, assemblyInstance.Id, AssemblyDetailViewOrientation.ElevationTop, item.Id, true);
                                                                }
                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }

                                                    
                                                    if (Properties.Settings.Default.ViewPaTemplate != "")
                                                    {
                                                        try
                                                        {
                                                            FilteredElementCollector ViewCollectorPa = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule));
                                                            foreach (View3D item in ViewCollectorPa)
                                                            {
                                                                if (item.Name == Properties.Settings.Default.ViewPaTemplate)
                                                                {
                                                                    ViewSchedule partList = AssemblyViewUtils.CreatePartList(doc, assemblyInstance.Id, item.Id, true);
                                                                }
                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            TaskDialog.Show("Error", ex.Message);
                                                            break;
                                                        }
                                                    }
                                                    if (Properties.Settings.Default.TemplTemplate != "")
                                                    {
                                                        try
                                                        {
                                                            FilteredElementCollector tempCollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks);
                                                            foreach (Element item in tempCollector)
                                                            {
                                                                if (item.Name == Properties.Settings.Default.TemplTemplate)
                                                                {
                                                                    ViewSheet sheet = AssemblyViewUtils.CreateSheet(doc, assemblyInstance.Id, item.Id);
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
