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
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace BIMtrovert.BS_Customs
{
    public sealed partial class RunFilter 
    {

        public Result Execute(ElementId id, IList<Parameter> pa, ExternalCommandData data, bool andor, bool proj)
        {
            try
            {
                
                ICollection<ElementId> eid = new List<ElementId>();
                UIDocument uidoc = data.Application.ActiveUIDocument;
                IList<Element> elems = new List<Element>();
                IList<ElementFilter> epf = new List<ElementFilter>();
                Element el = uidoc.Document.GetElement(id);
                epf.Clear();

                foreach (Parameter pas in pa)
                {
                    ParameterValueProvider pvp = new ParameterValueProvider(pas.Id);
                    switch (pas.StorageType)
                    {
                        case StorageType.Double:
                            epf.Add(new ElementParameterFilter(new FilterDoubleRule(pvp, new FilterNumericEquals(), el.get_Parameter(pas.Definition).AsDouble(), 0.01)));
                            break;
                        case StorageType.ElementId:
                            epf.Add(new ElementParameterFilter(new FilterElementIdRule(pvp, new FilterNumericEquals(),el.get_Parameter(pas.Definition).AsElementId())));
                            break;
                        case StorageType.Integer:
                            epf.Add(new ElementParameterFilter(new FilterIntegerRule(pvp, new FilterNumericEquals(), el.get_Parameter(pas.Definition).AsInteger())));
                            break;
                        case StorageType.String:
                            if (el.get_Parameter(pas.Definition).AsValueString() != null)
                            {
                                epf.Add(new ElementParameterFilter(new FilterStringRule(pvp, new FilterStringEquals(), el.get_Parameter(pas.Definition).AsValueString(), false)));
                            }
                            else
                            {
                                epf.Add(new ElementParameterFilter(new FilterStringRule(pvp, new FilterStringEquals(), el.get_Parameter(pas.Definition).AsString(), false)));
                            }
                            break;
                    }

                }
                LogicalAndFilter laf = new LogicalAndFilter(epf);
                LogicalOrFilter lof = new LogicalOrFilter(epf);
                FilteredElementCollector fec;

                fec = proj ? new FilteredElementCollector(uidoc.Document).WhereElementIsNotElementType(): new FilteredElementCollector(uidoc.Document, uidoc.Document.ActiveView.Id);

                elems = andor ? fec.WherePasses(laf).ToElements() : fec.WherePasses(lof).ToElements();


                foreach (Element elem in elems)
                {
                    eid.Add(elem.Id);
                }

                try
                {
                    uidoc.Selection.SetElementIds(eid);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                MessageBox.Show($"{elems.Count.ToString()} element(s) matched your search criteria." );
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
