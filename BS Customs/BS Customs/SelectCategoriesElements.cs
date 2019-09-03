// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SelectCategoriesElements.cs
 * https://bimtrovert.com
 * © BIMtrovert, 2019
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
using System.Windows.Forms;
#endregion

namespace BIMtrovert.BS_Customs
{

    public sealed partial class SelectCategoriesElements
    {
        public Result Execute(IList<Category> pa, ExternalCommandData data, bool proj)
        {
            try
            {

                ICollection<ElementId> eid = new List<ElementId>();
                UIDocument uidoc = data.Application.ActiveUIDocument;
                IList<Element> elems = new List<Element>();
                IList<ElementFilter> epf = new List<ElementFilter>();
                epf.Clear();

                foreach (Category pas in pa)
                {
                    //ParameterValueProvider pvp = new ParameterValueProvider(pas.Id);
                    epf.Add(new ElementCategoryFilter(pas.Id));

                }
                LogicalOrFilter lof = new LogicalOrFilter(epf);
                FilteredElementCollector fec;

                fec = proj ? new FilteredElementCollector(uidoc.Document).WhereElementIsNotElementType() : new FilteredElementCollector(uidoc.Document, uidoc.Document.ActiveView.Id);

                elems = fec.WherePasses(lof).ToElements();


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

                MessageBox.Show($"{elems.Count.ToString()} element(s) matched your search criteria.");
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
