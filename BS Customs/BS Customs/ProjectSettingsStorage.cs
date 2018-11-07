// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProjectSettingsStorage.cs
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

    public sealed partial class ProjectSettingsStorage
    {
        readonly Guid settingDsId = new Guid("{A71F620F-BD0D-46DD-AECD-AFDEF0DFFD74}");

        public ProjectSettings ReadSettings(Document doc)
        {
            var settingsEntity = GetSettingsEntity(doc);

            if (settingsEntity == null || !settingsEntity.IsValid())
            {
                return null;
            }

            ProjectSettings settings = new ProjectSettings();

            settings.View3DTemplate = settingsEntity.Get<ElementId>("View3DTemplate");
            settings.ViewElTemplate = settingsEntity.Get<ElementId>("ViewElTemplate");
            settings.ViewPlTemplate = settingsEntity.Get<ElementId>("ViewPlTemplate");
            settings.ViewPaTemplate = settingsEntity.Get<ElementId>("ViewPaTemplate");
            settings.TemplateTemplate = settingsEntity.Get<ElementId>("TemplateTemplate");
            settings.HorizontalDimWa = settingsEntity.Get<ElementId>("HorizontalDimWa");
            settings.HeightDimWa = settingsEntity.Get<ElementId>("HeightDimWa");

            settings.Check3DView = settingsEntity.Get<bool>("Check3DView");
            settings.CheckElev = settingsEntity.Get<bool>("CheckElev");
            settings.CheckPlan = settingsEntity.Get<bool>("CheckPlan");
            settings.CheckPart = settingsEntity.Get<bool>("CheckPart");
            settings.CheckTemplate = settingsEntity.Get<bool>("CheckTemplate");


            settings.View3DTemplateFlr = settingsEntity.Get<ElementId>("View3DTemplateFlr");
            settings.ViewPlanTemplateFlr = settingsEntity.Get<ElementId>("ViewPlanTemplateFlr");
            settings.ViewJoistTemplateFlr = settingsEntity.Get<ElementId>("ViewJoistTemplateFlr");
            settings.ViewTrackTemplateFrl = settingsEntity.Get<ElementId>("ViewTrackTemplateFrl");
            settings.ViewPaTemplateFlr = settingsEntity.Get<ElementId>("ViewPaTemplateFlr");
            settings.TemplateTemplateFrl = settingsEntity.Get<ElementId>("TemplateTemplateFrl");
            settings.HorizontalDimFlr = settingsEntity.Get<ElementId>("HorizontalDimFlr");
            settings.VerticalDimFlr = settingsEntity.Get<ElementId>("VerticalDimFlr");

            settings.Check3DViewFlr = settingsEntity.Get<bool>("Check3DViewFlr");
            settings.CheckPlanFlr = settingsEntity.Get<bool>("CheckPlanFlr");
            settings.CheckJoistFlr = settingsEntity.Get<bool>("CheckJoistFlr");
            settings.CheckTrackFlr = settingsEntity.Get<bool>("CheckTrackFlr");
            settings.CheckPartFlr = settingsEntity.Get<bool>("CheckPartFlr");
            settings.CheckTemplateFlr = settingsEntity.Get<bool>("CheckTemplateFlr");

            return settings;
        }

        public void WriteSettings(Document doc, ProjectSettings settings)
        {
            var settingsDs = GetSettingsDataStorage(doc);

            if (settingsDs == null)
            {
                settingsDs = DataStorage.Create(doc);
            }

            Entity settingsEntity = new Entity(ProjectSettingsSchema.GetSchema());

            settingsEntity.Set("View3DTemplate", settings.View3DTemplate);
            settingsEntity.Set("ViewElTemplate", settings.ViewElTemplate);
            settingsEntity.Set("ViewPlTemplate", settings.ViewPlTemplate);
            settingsEntity.Set("ViewPaTemplate", settings.ViewPaTemplate);
            settingsEntity.Set("TemplateTemplate", settings.TemplateTemplate);
            settingsEntity.Set("HorizontalDimWa", settings.HorizontalDimWa);
            settingsEntity.Set("HeightDimWa", settings.HeightDimWa);

            settingsEntity.Set("Check3DView", settings.Check3DView);
            settingsEntity.Set("CheckElev", settings.CheckElev);
            settingsEntity.Set("CheckPlan", settings.CheckPlan);
            settingsEntity.Set("CheckPart", settings.CheckPart);
            settingsEntity.Set("CheckTemplate", settings.CheckTemplate);


            settingsEntity.Set("View3DTemplateFlr", settings.View3DTemplateFlr);
            settingsEntity.Set("ViewPlanTemplateFlr", settings.ViewPlanTemplateFlr);
            settingsEntity.Set("ViewJoistTemplateFlr", settings.ViewJoistTemplateFlr);
            settingsEntity.Set("ViewTrackTemplateFrl", settings.ViewTrackTemplateFrl);
            settingsEntity.Set("ViewPaTemplateFlr", settings.ViewPaTemplateFlr);
            settingsEntity.Set("TemplateTemplateFrl", settings.TemplateTemplateFrl);
            settingsEntity.Set("HorizontalDimFlr", settings.HorizontalDimFlr);
            settingsEntity.Set("VerticalDimFlr", settings.VerticalDimFlr);

            settingsEntity.Set("Check3DViewFlr", settings.Check3DViewFlr);
            settingsEntity.Set("CheckPlanFlr", settings.CheckPlanFlr);
            settingsEntity.Set("CheckJoistFlr", settings.CheckJoistFlr);
            settingsEntity.Set("CheckTrackFlr", settings.CheckTrackFlr);
            settingsEntity.Set("CheckPartFlr", settings.CheckPartFlr);
            settingsEntity.Set("CheckTemplateFlr", settings.CheckTemplateFlr);


            Entity idEntity = new Entity(DataStorageUniqueIdSchema.GetSchema());

            idEntity.Set("Id", settingDsId);

            settingsDs.SetEntity(idEntity);
            settingsDs.SetEntity(settingsEntity);
        }

        private DataStorage GetSettingsDataStorage(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            var dataStorages = collector.OfClass(typeof(DataStorage));

            foreach (DataStorage dataStorage in dataStorages)
            {
                Entity settingIdEntity = dataStorage.GetEntity(DataStorageUniqueIdSchema.GetSchema());

                if (!settingIdEntity.IsValid()) continue;

                var id = settingIdEntity.Get<Guid>("Id");

                if (!id.Equals(settingDsId)) continue;

                return dataStorage;
            }

            return null;
        }

        private Entity GetSettingsEntity(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            var dataStorages = collector.OfClass(typeof(DataStorage));

            foreach (DataStorage dataStorage in dataStorages)
            {
                Entity settingEntity = dataStorage.GetEntity(ProjectSettingsSchema.GetSchema());

                if (!settingEntity.IsValid()) continue;

                return settingEntity;
            }

            return null;
        }
    }
}
