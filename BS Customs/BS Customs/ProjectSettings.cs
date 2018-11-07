// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProjectSettings.cs
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

    public sealed partial class ProjectSettings
    {
        public ElementId View3DTemplate { get; set; }
        public ElementId ViewElTemplate { get; set; }
        public ElementId ViewPlTemplate { get; set; }
        public ElementId ViewPaTemplate { get; set; }
        public ElementId TemplateTemplate { get; set; }
        public ElementId HorizontalDimWa { get; set; }
        public ElementId HeightDimWa { get; set; }

        public bool Check3DView { get; set; }
        public bool CheckElev { get; set; }
        public bool CheckPlan { get; set; }
        public bool CheckPart { get; set; }
        public bool CheckTemplate { get; set; }


        public ElementId View3DTemplateFlr { get; set; }
        public ElementId ViewPlanTemplateFlr { get; set; }
        public ElementId ViewJoistTemplateFlr { get; set; }
        public ElementId ViewTrackTemplateFrl { get; set; }
        public ElementId ViewPaTemplateFlr { get; set; }
        public ElementId TemplateTemplateFrl { get; set; } 
        public ElementId HorizontalDimFlr { get; set; }
        public ElementId VerticalDimFlr { get; set; }

        public bool Check3DViewFlr { get; set; }
        public bool CheckPlanFlr { get; set; }
        public bool CheckJoistFlr { get; set; }
        public bool CheckTrackFlr { get; set; }
        public bool CheckPartFlr { get; set; }
        public bool CheckTemplateFlr { get; set; }
        
    }
}
