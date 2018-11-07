// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProjectSettingsSchema.cs
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
using Autodesk.Revit.DB.ExtensibleStorage;
using Bushman.RevitDevTools;
using BIMtrovert.BS_Customs.Properties;
#endregion

namespace BIMtrovert.BS_Customs
{

    public static class ProjectSettingsSchema
    {
        readonly static Guid schemaGuid = new Guid("{9DBE0174-AA01-4CDD-BA86-96DE1FDCE041}");

        public static Schema GetSchema()
        {
            Schema schema = Schema.Lookup(schemaGuid);

            if (schema != null) return schema;

            SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGuid);

            schemaBuilder.SetSchemaName("ProjectSettings");

            schemaBuilder.AddSimpleField("View3DTemplate", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewElTemplate", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewPlTemplate", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewPaTemplate", typeof(ElementId));
            schemaBuilder.AddSimpleField("TemplateTemplate", typeof(ElementId));
            schemaBuilder.AddSimpleField("HorizontalDimWa", typeof(ElementId));
            schemaBuilder.AddSimpleField("HeightDimWa", typeof(ElementId));

            schemaBuilder.AddSimpleField("Check3DView", typeof(bool));
            schemaBuilder.AddSimpleField("CheckElev", typeof(bool));
            schemaBuilder.AddSimpleField("CheckPlan", typeof(bool));
            schemaBuilder.AddSimpleField("CheckPart", typeof(bool));
            schemaBuilder.AddSimpleField("CheckTemplate", typeof(bool));


            schemaBuilder.AddSimpleField("View3DTemplateFlr", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewPlanTemplateFlr", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewJoistTemplateFlr", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewTrackTemplateFrl", typeof(ElementId));
            schemaBuilder.AddSimpleField("ViewPaTemplateFlr", typeof(ElementId));
            schemaBuilder.AddSimpleField("TemplateTemplateFrl", typeof(ElementId));
            schemaBuilder.AddSimpleField("HorizontalDimFlr", typeof(ElementId));
            schemaBuilder.AddSimpleField("VerticalDimFlr", typeof(ElementId));

            schemaBuilder.AddSimpleField("Check3DViewFlr", typeof(bool));
            schemaBuilder.AddSimpleField("CheckPlanFlr", typeof(bool));
            schemaBuilder.AddSimpleField("CheckJoistFlr", typeof(bool));
            schemaBuilder.AddSimpleField("CheckTrackFlr", typeof(bool));
            schemaBuilder.AddSimpleField("CheckPartFlr", typeof(bool));
            schemaBuilder.AddSimpleField("CheckTemplateFlr", typeof(bool));


            return schemaBuilder.Finish();
        }
    }
}
