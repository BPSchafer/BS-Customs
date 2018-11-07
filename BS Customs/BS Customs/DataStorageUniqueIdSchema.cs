// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataStorageUniqueIdSchema.cs
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
using Autodesk.Revit.DB.ExtensibleStorage;
#endregion

namespace BIMtrovert.BS_Customs
{

    static class DataStorageUniqueIdSchema
    {
        static readonly Guid schemaGuid = new Guid("{EEEFD606-7262-4782-93F0-2DA87D5AE6E4}");

        public static Schema GetSchema()
        {
            Schema schema = Schema.Lookup(schemaGuid);

            if (schema != null)
                return schema;

            SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGuid);

            schemaBuilder.SetSchemaName("DataStorageUniqueId");

            schemaBuilder.AddSimpleField("Id", typeof(Guid));

            return schemaBuilder.Finish();
        }
    }
}
