using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace CSM.Form100.Drivers
{
    using Models;
    using System;
    
    public class ActionPartDriver : ContentPartDriver<ActionPart>
    {
        private readonly string dateFormat = "yyyy-dd-MM";

        protected override string Prefix
        {
            get { return "Action"; }
        }

        protected override DriverResult Display(ActionPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape(
                    "Parts_Action_EffectiveDate",
                    () => shapeHelper.Parts_Action_EffectiveDate(EffectiveDate: part.EffectiveDate)
                ),
                ContentShape(
                    "Parts_Action_Category",
                    () => shapeHelper.Parts_Action_Category(Category: part.Category)
                ),
                ContentShape(
                    "Parts_Action_Type",
                    () => shapeHelper.Parts_Action_Type(Type: part.Type)
                ),
                ContentShape(
                    "Parts_Action_Detail",
                    () => shapeHelper.Parts_Action_Detail(Detail: part.Detail)
                )
            );
        }

        /// <summary>
        /// Define how ActionPart data is exported.
        /// </summary>
        protected override void Exporting(ActionPart part, ExportContentContext context)
        {
            var actionNode = context.Element(part.PartDefinition.Name);

            actionNode.SetAttributeValue("EffectiveDate", part.EffectiveDate.HasValue ? part.EffectiveDate.Value.ToString(dateFormat) : String.Empty);
            actionNode.SetAttributeValue("Category", part.Category == ActionCategory.Undefined ? String.Empty : part.Category.ToString());
            actionNode.SetAttributeValue("Type", part.Type);
            actionNode.SetAttributeValue("Detail", part.Detail);
        }

        ///// <summary>
        ///// Define how StoryPart data is exported.
        ///// </summary>
        //protected override void Exporting(StoryPart part, ExportContentContext context)
        //{
        //    var storyNode = context.Element(part.PartDefinition.Name);

        //    storyNode.SetAttributeValue("Location", part.Location.TrimSafe());
        //    storyNode.SetAttributeValue("Summary", part.Summary.TrimSafe());
        //    storyNode.SetAttributeValue("LegalAnalysis", part.LegalAnalysis.TrimSafe());
        //    storyNode.SetAttributeValue("Votes", part.Votes);

        //    // export the Contact as a child element of the Story node

        //    var contactNode = new XElement("Contact");

        //    contactNode.SetAttributeValue("Name", part.Contact.Name);
        //    contactNode.SetAttributeValue("Email", part.Contact.Email);
        //    contactNode.SetAttributeValue("JobTitle", part.Contact.JobTitle);
        //    contactNode.SetAttributeValue("Phone", part.Contact.Phone);

        //    context.Element(part.PartDefinition.Name).Add(contactNode);
        //}

        ///// <summary>
        ///// Define how StoryPart data is imported.
        ///// </summary>
        //protected override void Importing(StoryPart part, ImportContentContext context)
        //{
        //    context.ImportAttribute(part.PartDefinition.Name, "Location", x => part.Location = x);
        //    context.ImportAttribute(part.PartDefinition.Name, "Summary", x => part.Summary = x);
        //    context.ImportAttribute(part.PartDefinition.Name, "LegalAnalysis", x => part.LegalAnalysis = x);

        //    context.ImportAttribute(part.PartDefinition.Name, "Votes", x => {
        //        int votes;
        //        if (int.TryParse(x, out votes))
        //            part.Votes = votes;
        //        else
        //            part.Votes = 0;
        //    });

        //    // the object that will eventually be assigned to this part
        //    ContactRecord contactRecord = null;

        //    string name, email, jobTitle, phone;
        //    name = email = jobTitle = phone = String.Empty;

        //    // determine if this part was exported with a Contact element
        //    var contactNode = context.Data.Element(part.PartDefinition.Name).Element("Contact");
        //    if (contactNode != null)
        //    {
        //        // assign values for the known attributes
        //        name = contactNode.Attribute("Name").SafeValue();
        //        email = contactNode.Attribute("Email").SafeValue();
        //        jobTitle = contactNode.Attribute("JobTitle").SafeValue();
        //        phone = contactNode.Attribute("Phone").SafeValue();

        //        // determine if this Contact element represents an existing contact record
        //        if (!(String.IsNullOrEmpty(name) || String.IsNullOrEmpty(email)))
        //        {
        //            contactRecord = contactService.Get(name, email);
        //        }
        //    }

        //    // the export did not contain enough data to query existing contacts
        //    if (contactRecord == null)
        //    {
        //        contactRecord = contactService.Create();
        //    }

        //    contactRecord.Name = name;
        //    contactRecord.Email = email;
        //    contactRecord.JobTitle = jobTitle;
        //    contactRecord.Phone = phone;

        //    // make sure to update the contact record in DB
        //    contactService.Update(contactRecord);

        //    part.Contact = contactRecord;
        //}
    }
}