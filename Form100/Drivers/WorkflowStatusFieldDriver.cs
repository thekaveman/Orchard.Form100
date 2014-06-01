using System;
using CSM.Form100.Fields;
using CSM.Form100.Settings;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace CSM.Form100.Drivers
{
    public class WorkflowStatusFieldDriver : ContentFieldDriver<WorkflowStatusField>
    {
        private const string TemplateName = "Fields/WorkflowStatus.Edit";

        public WorkflowStatusFieldDriver(IOrchardServices services)
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(WorkflowStatusField field, ContentPart part)
        {
            return field.Name;
        }

        protected override DriverResult Display(ContentPart part, WorkflowStatusField field, string displayType, dynamic shapeHelper)
        {
            return ContentShape(
                "Fields_WorkflowStatus",
                GetDifferentiator(field, part),
                () => shapeHelper.Fields_WorkflowStatus()
            );
        }

        protected override DriverResult Editor(ContentPart part, WorkflowStatusField field, dynamic shapeHelper) {
            return ContentShape(
                "Fields_WorkflowStatus_Edit",
                GetDifferentiator(field, part),
                () => shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: field,
                    Prefix: GetPrefix(field, part)
                )
            );
        }

        protected override DriverResult Editor(ContentPart part, WorkflowStatusField field, IUpdateModel updater, dynamic shapeHelper) {
            if (updater.TryUpdateModel(field, GetPrefix(field, part), null, null))
            {
                var settings = field.PartFieldDefinition.Settings.GetModel<WorkflowStatusFieldSettings>();

                if (settings.Required && field.Value == default(WorkflowStatus))
                {
                    updater.AddModelError(field.Name, T("The field {0} is mandatory", T(field.DisplayName)));
                }
            }

            return Editor(part, field, shapeHelper);
        }

        protected override void Importing(ContentPart part, WorkflowStatusField field, ImportContentContext context)
        {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Value", 
                v => {
                    WorkflowStatus status;
                    if (Enum.TryParse(v, out status))
                        field.Value = status;
                }
            );
        }

        protected override void Exporting(ContentPart part, WorkflowStatusField field, ExportContentContext context)
        {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Value", field.Value);
        }

        protected override void Describe(DescribeMembersContext context)
        {
            context
                .Member(null, typeof(string), T("Value"), T("The selected value of the field."))
                .Enumerate<WorkflowStatusField>(() => field => new[] { field.Value });
        }
    }
}
