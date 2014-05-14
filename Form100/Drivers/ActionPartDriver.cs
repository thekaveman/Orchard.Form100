using Orchard.ContentManagement.Drivers;

namespace CSM.Form100.Drivers
{
    using Models;

    public class ActionPartDriver : ContentPartDriver<ActionPart>
    {
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
                    "Parts_Action_Label",
                    () => shapeHelper.Parts_Action_Label(Label: part.Label)
                ),
                ContentShape(
                    "Parts_Action_Detail",
                    () => shapeHelper.Parts_Action_Detail(Detail: part.Detail)
                )
            );
        }
    }
}