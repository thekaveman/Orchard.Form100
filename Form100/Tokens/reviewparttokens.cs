using System;
using System.Linq;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;

namespace CSM.Form100.Tokens
{
    public class ReviewPartTokens : ITokenProvider
    {
        public ReviewPartTokens()
        {            
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context)
        {
            context.For("Review", T("Review"), T("Tokens for the ReviewPart."))
                    .Token("Status", T("Status"), T("The current status of a ReviewPart."))
                    .Token("Next", T("Next"), T("The next reviewer in the chain."))
                    .Token("Next.Name", T("Next.Name"), T("The name of the next reviewer in the chain."))
                    .Token("Next.Email", T("Next.Email"), T("The email of the next reviewer in the chain."))
                    .Token("Next.Approving", T("Next.Accepting"), T("The approving status of the next reviewer in the chain."))
                    .Token("Next.Rejecting", T("Next.Rejecting"), T("The rejecting status of the next reviewer in the chain."))
                    ;
        }

        public void Evaluate(EvaluateContext context)
        {
            IContent currentContentItem = context.Data["Content"] as IContent;
            ReviewPart currentReviewPart = currentContentItem == null ? default(ReviewPart) : currentContentItem.As<ReviewPart>();
            
            context.For<ReviewPart>("Review", currentReviewPart)
                    .Token("Status", reviewPart =>
                    {
                        if (reviewPart != null)
                            return reviewPart.Status.ToString();

                        return WorkflowStatus.Undefined.ToString();
                    })
                    .Token("Next", nextReviewer)
                    .Token("Next.Name", reviewPart => {
                        var next = nextReviewer(reviewPart);
                        if (next != null)
                            return next.ReviewerName;

                        return String.Empty;
                    })
                    .Token("Next.Email", reviewPart => {
                        var next = nextReviewer(reviewPart);
                        if (next != null)
                            return next.ReviewerEmail;

                        return String.Empty;
                    })
                    .Token("Next.Approving", reviewPart => {
                        var next = nextReviewer(reviewPart);
                        if (next != null)
                            return next.ApprovingStatus.ToString();

                        return WorkflowStatus.Undefined.ToString();
                    })
                    .Token("Next.Rejecting", reviewPart => {
                        var next = nextReviewer(reviewPart);
                        if (next != null)
                            return next.RejectingStatus.ToString();

                        return WorkflowStatus.Undefined.ToString();
                    })
                    ;
        }

        private ReviewStepRecord nextReviewer(ReviewPart reviewPart)
        {
            if (reviewPart != null && reviewPart.PendingReviews != null && reviewPart.PendingReviews.Any())
                return reviewPart.PendingReviews.Peek();

            return null;
        }
    }
}