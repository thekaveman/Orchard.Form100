﻿using System.Collections.Generic;
using System.Globalization;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace CSM.Form100.Settings
{
    public class WorkflowStatusFieldListModeEvents : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition)
        {
            if (definition.FieldDefinition.Name == "WorkflowStatusField")
            {
                var model = definition.Settings.GetModel<WorkflowStatusFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.FieldType != "WorkflowStatusField") yield break;
            
            var model = new WorkflowStatusFieldSettings();
            if (updateModel.TryUpdateModel(model, "WorkflowStatusFieldSettings", null, null))
            {
                builder.WithSetting("WorkflowStatusFieldSettings.Disabled", model.Disabled.ToString(CultureInfo.InvariantCulture));
                builder.WithSetting("WorkflowStatusFieldSettings.Hint", model.Hint);
                builder.WithSetting("WorkflowStatusFieldSettings.Required", model.Required.ToString(CultureInfo.InvariantCulture));
            }

            yield return DefinitionTemplate(model);
        }
    }
}