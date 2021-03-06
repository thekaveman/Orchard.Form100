﻿using System;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;

namespace CSM.Form100.Tokens
{
    /// <summary>
    /// Token provider for ReviewPart data.
    /// </summary>
    public class ReviewPartTokens : ITokenProvider
    {
        private readonly IReviewService reviewService;

        public ReviewPartTokens(IReviewService reviewService)
        {
            this.reviewService = reviewService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context)
        {
            //provide 7 tokens:
            context.For("Review", T("Review"), T("Tokens for the ReviewPart."))
                    //{Review.Status}
                    .Token("State", T("State"), T("The current state of a ReviewPart."))
                    //{Review.Next}
                    .Token("Next", T("Next"), T("The next reviewer in the chain."))
                    //{Review.Next.Id}
                    .Token("Next.Id", T("Next.Id"), T("The id of the next reviewer in the chain."))
                    //{Review.Next.Name}
                    .Token("Next.Name", T("Next.Name"), T("The name of the next reviewer in the chain."))
                    //{Review.Next.Email}
                    .Token("Next.Email", T("Next.Email"), T("The email of the next reviewer in the chain."))
                    //{Review.Next.Approving}
                    .Token("Next.Approving", T("Next.Accepting"), T("The approving state of the next reviewer in the chain."))
                    //{Review.Next.Rejecting}
                    .Token("Next.Rejecting", T("Next.Rejecting"), T("The rejecting state of the next reviewer in the chain."))
                    //{Review.History.Emails}
                    .Token("History.Emails", T("History.Emails"), T("The email addresses of each reviewer in the history."))
                    ;
        }

        public void Evaluate(EvaluateContext context)
        {
            //try to get the current content from context
            IContent content = context.Data.ContainsKey("Content") ? context.Data["Content"] as IContent : null;

            //try to get a ReviewPart from the current content
            ReviewPart part = content == null ? default(ReviewPart) : content.As<ReviewPart>();

            //provide the current ReviewPart, if any, as context data
            context.For<ReviewPart>("Review", part)
                    //evaluator function for {Review.Status}
                    .Token("State", reviewPart =>
                    {
                        if (reviewPart != null)
                            return reviewPart.State.ToString();
                        return WorkflowStatus.Undefined.ToString();
                    })
                    //evaluator function for {Review.Next}
                    .Token("Next", reviewPart => reviewPart.PeekNextReviewStep(reviewService))
                    //evaluator function for {Review.Next.Id}
                    .Token("Next.Id", reviewPart => {
                        var next = reviewPart.PeekNextReviewStep(reviewService);
                        if (next != null)
                            return next.Id.ToString();
                        return String.Empty;
                    })
                    //evaluator function for {Review.Next.Name}
                    .Token("Next.Name", reviewPart => {
                        var next = reviewPart.PeekNextReviewStep(reviewService);
                        if (next != null)
                            return next.ReviewerName;
                        return String.Empty;
                    })
                    //evaluator function for {Review.Next.Email}
                    .Token("Next.Email", reviewPart => {
                        var next = reviewPart.PeekNextReviewStep(reviewService);
                        if (next != null)
                            return next.ReviewerEmail;
                        return String.Empty;
                    })
                    //evaluator function for {Review.Next.Approving}
                    .Token("Next.Approving", reviewPart => {
                        var next = reviewPart.PeekNextReviewStep(reviewService);
                        if (next != null)
                            return next.ApprovingState.ToString();
                        return WorkflowStatus.Undefined.ToString();
                    })
                    //evaluator function for {Review.Next.Rejecting}
                    .Token("Next.Rejecting", reviewPart => {
                        var next = reviewPart.PeekNextReviewStep(reviewService);
                        if (next != null)
                            return next.RejectingState.ToString();
                        return WorkflowStatus.Undefined.ToString();
                    })
                    //evaluator function for {Review.History.Emails}
                    .Token("History.Emails", reviewPart => {
                        if (reviewPart != null && reviewPart.ReviewHistory != null)
                            return reviewService.SerializeReviewSteps(reviewPart.ReviewHistory, r => r.ReviewerEmail, true);
                        return String.Empty;
                    })
                    ;
        }
    }
}