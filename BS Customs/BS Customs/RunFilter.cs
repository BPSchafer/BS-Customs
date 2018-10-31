// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RunFilter.cs
 * https://www.bimtrovert.com
 * © BIMtrovert, 2018
 *
 * This updater is used to create an updater capable of reacting
 * to changes in the Revit model.
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
    public sealed partial class RunFilter 
    {
        public Result Execute(ExternalCommandData data, ref string message, ElementSet elements)
        {
            try
            {

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Wrong", e.Message);
                return Result.Failed;
            }
        }

        public Result Execute(ElementId id, IList<Parameter> pa, ExternalCommandData data, bool andor, bool proj)
        {
            try
            {

                string parameterList = "";
                ICollection<ElementId> eid = new List<ElementId>();
                UIDocument uidoc = data.Application.ActiveUIDocument;
                IList<bool> check = new List<bool>();
                IList<bool> check2 = new List<bool>();
                IList<Element> elems = new List<Element>();
                if (proj)
                {
                    FilteredElementCollector fec = new FilteredElementCollector(uidoc.Document).WhereElementIsNotElementType();
                    foreach (Element e in fec)
                    {
                        if (null != e.Category && e.CanHaveTypeAssigned())
                        {
                            elems.Add(e);
                        }

                    }
                }
                else
                {
                    elems = new FilteredElementCollector(uidoc.Document, uidoc.Document.ActiveView.Id).ToElements();
                }

                Element el = uidoc.Document.GetElement(id);
                if (andor)
                {
                    foreach (Element elem in elems)
                    {
                        check.Clear();
                        check2.Clear();
                        foreach (Parameter pas in pa)
                        {
                            if (null != elem.get_Parameter(pas.Definition))
                            {
                                check.Add(true);
                            }
                            else
                            {
                                check.Add(false);
                            }
                        }
                        if (check.Contains(false))
                        {

                        }
                        else
                        {
                            foreach (Parameter pas in pa)
                            {
                                if (elem.get_Parameter(pas.Definition).AsValueString() != null)
                                {
                                    check2.Add(elem.get_Parameter(pas.Definition).AsValueString() ==
                                               el.get_Parameter(pas.Definition).AsValueString());
                                }
                                else
                                {
                                    check2.Add(elem.get_Parameter(pas.Definition).AsString() ==
                                               el.get_Parameter(pas.Definition).AsString());
                                }
                            }
                            if (check2.Contains(false))
                            {

                            }
                            else
                            {
                                eid.Add(elem.Id);
                                parameterList += elem.Name + ": ";
                                parameterList += elem.Id + "\n";
                            }
                        }
                    }
                }
                else
                {
                    foreach (Element elem in elems)
                    {

                        check.Clear();
                        check2.Clear();
                        foreach (Parameter pas in pa)
                        {
                            if (null != elem.get_Parameter(pas.Definition))
                            {
                                check.Add(true);
                            }
                            else
                            {
                                check.Add(false);
                            }
                        }

                        for (int i = 0; i < check.Count; i++)
                        {
                            if (!check[i]) continue;
                            if (elem.get_Parameter(pa[i].Definition).AsValueString() != null)
                            {
                                check2.Add(elem.get_Parameter(pa[i].Definition).AsValueString() ==
                                           el.get_Parameter(pa[i].Definition).AsValueString());
                            }
                            else
                            {
                                check2.Add(elem.get_Parameter(pa[i].Definition).AsString() ==
                                           el.get_Parameter(pa[i].Definition).AsString());
                            }

                        }
                        if (check2.Contains(true))
                        {
                            eid.Add(elem.Id);
                        }
                    }

                }

                uidoc.Selection.SetElementIds(eid);
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Value", e.Message);
                return Result.Failed;
            }
        }
    }
}
