﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
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
                                string parVal = null;
                                Element elem = ui_doc.Document.GetElement(id);
                                foreach (Parameter pa in elem.Parameters)
                                {
                                    
                                    if (pa.Definition.Name == "BIMSF_Container")
                                    {
                                        parVal = pa.AsValueString();
                                        /* (pa.AsValueString() != null)
                                        {
                                            parVal = pa.AsValueString();
                                        }
                                        if (parVal != null)
                                        {
                                            info += "\n\t" + parVal;

                                        }*/
                                    }
                                    info += "\n\t" + parVal;
                                }
                                
                                    //info += "\n\t" + id.IntegerValue;
                            }

                            TaskDialog.Show("Revit", info);
                        }

                        return TransactionStatus.Committed ==
                            tr.Commit();
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