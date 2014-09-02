﻿using System;
using System.Linq;
using System.Xml.Linq;
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
    public class ReviewPartDriver : ContentPartDriver<ReviewPart>
    {
        private readonly IReviewService reviewService;

        public ReviewPartDriver(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        /// <summary>
        /// Needed for HTML input id uniqueness
        /// </summary>
        protected override string Prefix
        {
            get { return "CSM_Form100_ReviewPart"; }
        }

        /// <summary>
        /// Respond to requests to display data from this part
        /// e.g. return a bunch of so called "display" shapes
        /// </summary>
        protected override DriverResult Display(ReviewPart part, string displayType, dynamic shapeHelper)
        {
            var viewModel = reviewService.GetReviewViewModel(part);

            return ContentShape(
                "Parts_Review",
                () => shapeHelper.Parts_Review(viewModel)
            );
        }

        /// <summary>
        /// Respond to GET requests for an edit view of this part.
        /// e.g. return a bunch of "edit" shapes
        /// </summary>
        protected override DriverResult Editor(ReviewPart part, dynamic shapeHelper)
        {
            var viewModel = reviewService.GetReviewViewModel(part);

            return ContentShape(
                "Parts_Review_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Review",
                    Model: viewModel,
                    Prefix: Prefix
                )
            );
        }

        /// <summary>
        /// Respond to POST requests for updating this part's data.
        /// </summary>
        protected override DriverResult Editor(ReviewPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new ReviewPartViewModel();

            if (updater.TryUpdateModel(viewModel, Prefix, null, new[] { "AvailableStates" }))
            {
                reviewService.UpdateReview(viewModel, part);
            }

            return Editor(part, shapeHelper);
        }

        /// <summary>
        /// Define how the part's data is exported.
        /// Hint: it uses XML.
        /// </summary>
        protected override void Exporting(ReviewPart part, ExportContentContext context)
        {
            var reviewNode = context.Element(part.PartDefinition.Name);

            reviewNode.SetAttributeValue("State", part.State);

            if (part.PendingReviews.SafeAny())
            {
                var pendingReviewsNode = new XElement("PendingReviews");
                var clone = part.PendingReviews.WeakClone();
                
                while (clone.Any())
                {
                    var reviewStepNode = createReviewStepNode(clone.RemoveNext());
                    pendingReviewsNode.Add(reviewStepNode);
                }

                reviewNode.Add(pendingReviewsNode);
            }

            if (part.ReviewHistory.SafeAny())
            {
                var reviewHistoryNode = new XElement("ReviewHistory");
                var clone = part.ReviewHistory.WeakClone();

                while (clone.Any())
                {
                    var reviewStepNode = createReviewStepNode(clone.RemoveNext());
                    reviewHistoryNode.Add(reviewStepNode);
                }

                reviewNode.Add(reviewHistoryNode);
            }
        }

        /// <summary>
        /// Define how the part's data is imported.
        /// Hint: it's the inverse of Exporting.
        /// </summary>
        protected override void Importing(ReviewPart part, ImportContentContext context)
        {
            var reviewNode = context.Data.Element(part.PartDefinition.Name);

            context.ImportAttribute(reviewNode.Name.LocalName, "State", s =>
            {
                WorkflowStatus state;

                if (Enum.TryParse(s, out state))
                    part.State = state;
                else
                    throw new InvalidOperationException("Couldn't parse State attribute to WorkflowStatus enum.");
            });

            var pendingReviewsNode = reviewNode.Element("PendingReviews");
            if (pendingReviewsNode != null && pendingReviewsNode.Elements("ReviewStep").Any())
            {
                foreach (var reviewStepNode in pendingReviewsNode.Elements("ReviewStep"))
                {
                    var reviewStep = parseReviewStepNode(reviewStepNode);
                    part.PendingReviews.Add(reviewService.CreateReviewStep(reviewStep));
                }
            }
            
            var reviewHistoryNode = reviewNode.Element("ReviewHistory");
            if (reviewHistoryNode != null && reviewHistoryNode.Elements("ReviewStep").Any())
            {
                foreach (var reviewStepNode in reviewHistoryNode.Elements("ReviewStep"))
                {
                    var reviewStep = parseReviewStepNode(reviewStepNode);
                    part.ReviewHistory.Add(reviewService.CreateReviewStep(reviewStep));
                }
            }
        }

        private XElement createReviewStepNode(ReviewStepRecord reviewStep)
        {
            var reviewStepNode = new XElement("ReviewStep");

            reviewStepNode.SetAttributeValue("ReviewPartIdentifier", reviewStep.ReviewPartIdentifier);
            reviewStepNode.SetAttributeValue("TargetStates", reviewStep.TargetStates);
            reviewStepNode.SetAttributeValue("ApprovingState", reviewStep.ApprovingState);
            reviewStepNode.SetAttributeValue("RejectingState", reviewStep.RejectingState);
            reviewStepNode.SetAttributeValue("NotificationDate", reviewStep.NotificationDate.HasValue ? reviewStep.NotificationDate.Value.ToString() : String.Empty);
            reviewStepNode.SetAttributeValue("ReviewDate", reviewStep.ReviewDate.HasValue ? reviewStep.ReviewDate.Value.ToString() : String.Empty);
            reviewStepNode.SetAttributeValue("ReviewDecision", reviewStep.ReviewDecision);
            reviewStepNode.SetAttributeValue("ReviewerName", reviewStep.ReviewerName);
            reviewStepNode.SetAttributeValue("ReviewerEmail", reviewStep.ReviewerEmail);

            return reviewStepNode;
        }

        private ReviewStepRecord parseReviewStepNode(XElement reviewStepNode)
        {
            ReviewStepRecord reviewStep = new ReviewStepRecord() {
                ReviewPartIdentifier = reviewStepNode.SafeGetAttribute("ReviewPartIdentifier"),
                TargetStates = reviewStepNode.SafeGetAttribute("TargetStates"),
                ReviewerName = reviewStepNode.SafeGetAttribute("ReviewerName"),
                ReviewerEmail = reviewStepNode.SafeGetAttribute("ReviewerEmail")
            };
            
            WorkflowStatus status;

            if (Enum.TryParse(reviewStepNode.SafeGetAttribute("ApprovingState"), out status))
                reviewStep.ApprovingState = status;
            else
                reviewStep.ApprovingState = WorkflowStatus.Undefined;

            if (Enum.TryParse(reviewStepNode.SafeGetAttribute("RejectingState"), out status))
                reviewStep.RejectingState = status;
            else
                reviewStep.RejectingState = WorkflowStatus.Undefined;

            if (Enum.TryParse(reviewStepNode.SafeGetAttribute("ReviewDecision"), out status))
                reviewStep.ReviewDecision = status;
            else
                reviewStep.ReviewDecision = WorkflowStatus.Undefined;

            DateTime date;

            if (DateTime.TryParse(reviewStepNode.SafeGetAttribute("NotificationDate"), out date))
                reviewStep.NotificationDate = date;
            else
                reviewStep.NotificationDate = null;

            if (DateTime.TryParse(reviewStepNode.SafeGetAttribute("ReviewDate"), out date))
                reviewStep.ReviewDate = date;
            else
                reviewStep.ReviewDate = null;

            return reviewStep;
        }
    }
}