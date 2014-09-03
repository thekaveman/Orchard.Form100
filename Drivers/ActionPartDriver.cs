﻿using System;
using CSM.Form100.Models;
using CSM.Form100.Services;
using CSM.Form100.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace CSM.Form100.Drivers
{
    /// <summary>
    /// ContentPartDrivers are mini-Controllers that work at the ContentPart level
    /// </summary>
    public class ActionPartDriver : ContentPartDriver<ActionPart>
    {
        private readonly IActionService actionService;

        public ActionPartDriver(IActionService actionService)
        {
            this.actionService = actionService;
        }

        /// <summary>
        /// Needed for HTML input id uniqueness
        /// </summary>
        protected override string Prefix
        {
            get { return "CSM_Form100_ActionPart"; }
        }

        /// <summary>
        /// Respond to GET requests to display data from this part.
        /// e.g. return a bunch of "display" shapes
        /// </summary>
        protected override DriverResult Display(ActionPart part, string displayType, dynamic shapeHelper)
        {
            var viewModel = actionService.GetViewModel(part);

            return ContentShape(
                "Parts_Action",
                () => shapeHelper.Parts_Action(viewModel)
            );
        }

        /// <summary>
        /// Respond to GET requests for an edit view of this part.
        /// e.g. return a bunch of "edit" shapes
        /// </summary>
        protected override DriverResult Editor(ActionPart part, dynamic shapeHelper)
        {
            var viewModel = actionService.GetViewModel(part);

            return ContentShape(
                "Parts_Action_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Action",
                    Model: viewModel,
                    Prefix: Prefix
                )
            );
        }

        /// <summary>
        /// Respond to POST requests for updating this part's data.
        /// </summary>
        protected override DriverResult Editor(ActionPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new ActionPartViewModel();

            if (updater.TryUpdateModel(viewModel, Prefix, null, null))
            {
                actionService.UpdateAction(viewModel, part);
            }

            return Editor(part, shapeHelper);
        }

        /// <summary>
        /// Define how the part's data is exported.
        /// Hint: it uses XML.
        /// </summary>
        protected override void Exporting(ActionPart part, ExportContentContext context)
        {
            var actionNode = context.Element(part.PartDefinition.Name);

            actionNode.SetAttributeValue("EffectiveDate", part.EffectiveDate.HasValue ? part.EffectiveDate.Value.ToString() : String.Empty);
            actionNode.SetAttributeValue("Category", part.Category.ToString());
            actionNode.SetAttributeValue("Type", part.Type);
            actionNode.SetAttributeValue("Detail", part.Detail);
        }

        /// <summary>
        /// Define how the part's data is imported.
        /// Hint: it's the inverse of Exporting.
        /// </summary>
        protected override void Importing(ActionPart part, ImportContentContext context)
        {
            var actionNode = context.Data.Element(part.PartDefinition.Name);

            context.ImportAttribute(actionNode.Name.LocalName, "EffectiveDate", d =>
            {
                DateTime effectiveDate;

                if (DateTime.TryParse(d, out effectiveDate))
                    part.EffectiveDate = effectiveDate;
                else
                    part.EffectiveDate = null;
            });

            context.ImportAttribute(actionNode.Name.LocalName, "Category", c =>
            {
                ActionCategory category;

                if (Enum.TryParse(c, out category))
                    part.Category = category;
                else
                    throw new InvalidOperationException("Couldn't parse Category attribute to ActionCategory enum.");
            });

            context.ImportAttribute(actionNode.Name.LocalName, "Type", t => part.Type = t);

            context.ImportAttribute(actionNode.Name.LocalName, "Detail", d => part.Detail = d);
        }
    }
}