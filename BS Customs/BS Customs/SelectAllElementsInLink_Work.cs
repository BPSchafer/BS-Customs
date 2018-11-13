// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SelectAllElementsInLink_Work.cs
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
    public class LinkFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            Category category = element.Category;
            if ((BuiltInCategory)category.Id.IntegerValue == BuiltInCategory.OST_RvtLinks)
            {
                return true;
            }

            return false;
        }

        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }
    }

    public sealed partial class SelectAllElementsInLink
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
            ICollection<ElementId> eid = new List<ElementId>();

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

                        Reference hasPicked = selection.PickObject(ObjectType.LinkedElement, "Select a linked model");
                        if (hasPicked != null)
                        {
                            String list = null;
                            ElementId id = hasPicked.ElementId;
                            var el = (RevitLinkInstance)doc.GetElement(id);
                            //Type linkType = el.GetType();
                            Document d = el.GetLinkDocument();
                            Transform tf1 = el.GetTotalTransform();
                            Element del = d.GetElement(hasPicked.LinkedElementId);
                            //Element linkedElement = d.GetElement(hasPicked.LinkedElementId);
                            //Type linkType = linkedElement.GetType();
                            View view = (View)doc.ActiveView;
                            ElementId viewId = view.Id;

                            FilteredElementCollector fec = new FilteredElementCollector(d).OfCategory((BuiltInCategory)d.GetElement(hasPicked.LinkedElementId).Category.Id.IntegerValue);
                            Element e = fec.FirstElement();
                            foreach (Element ids in fec.ToElements())
                            {
                                if (ids.Name == del.Name)
                                {
                                    
                                    eid.Add(ids.Id);
                                    list += "\n" + ids.Document.Title + " : " + ids.Name;
                                }
                                
                            }

                            ICollection<ElementId> es = ElementTransformUtils.CopyElements(d, eid, doc, tf1, null);
                            ui_doc.Selection.SetElementIds(es);
                            //TaskDialog.Show("Revit", list);



                        }

                        return TransactionStatus.Committed ==
                            tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                /* TODO: Handle the exception here if you need 
                 * or throw the exception again if you need. */
                throw ex;
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
