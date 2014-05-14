using Orchard.ContentManagement.Drivers;

namespace CSM.Form100.Drivers
{
    using Models;
    using Services;

    public class ReviewPartDriver : ContentPartDriver<ReviewPart>
    {
        private readonly IReviewApprovalService reviewApprovalService;

        public ReviewPartDriver(IReviewApprovalService reviewApprovalService)
        {
            this.reviewApprovalService = reviewApprovalService;
        }

        protected override string Prefix
        {
            get { return "Review"; }
        }

        protected override DriverResult Display(ReviewPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape(
                    "Parts_Review_Approvals",
                    () => shapeHelper.Parts_Review_Approvals(Approvals: part.Approvals)
                ),
                ContentShape(
                    "Parts_Review_Status",
                    () => shapeHelper.Parts_Review_Status(Status: part.Status)
                )
            );
        }
    }
}