﻿using System;
using System.Collections.Generic;
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
    public class ReviewPartDriver : ContentPartDriver<ReviewPart>
    {
        private readonly IReviewService reviewService;

        public ReviewPartDriver(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

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
            return ContentShape(
                "Parts_Review",
                () => shapeHelper.Parts_Review(part)
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

            if (updater.TryUpdateModel(viewModel, Prefix, null, new[] { "AvailableStatuses" }))
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

            reviewNode.SetAttributeValue("Status", part.Status);

            if (part.PendingReviews != null && part.PendingReviews.Any())
            {
                var pendingReviewsNode = new XElement("PendingReviews");
                var copy = part.PendingReviews.Copy();
                
                while (copy.Any())
                {
                    var reviewStepNode = createReviewStepNode(copy.Dequeue());
                    pendingReviewsNode.Add(reviewStepNode);
                }

                reviewNode.Add(pendingReviewsNode);
            }

            if (part.ReviewHistory != null && part.ReviewHistory.Any())
            {
                var reviewHistoryNode = new XElement("ReviewHistory");
                var copy = part.ReviewHistory.Copy();

                while (copy.Any())
                {
                    var reviewStepNode = createReviewStepNode(copy.Pop());
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

            context.ImportAttribute(reviewNode.Name.LocalName, "Status", s =>
            {
                WorkflowStatus status;

                if (Enum.TryParse(s, out status))
                    part.Status = status;
                else
                    throw new InvalidOperationException("Couldn't parse Status attribute to WorkflowStatus enum.");
            });

            var pendingReviews = new Queue<ReviewStepRecord>();

            var pendingReviewsNode = reviewNode.Element("PendingReviews");

            if (pendingReviewsNode != null && pendingReviewsNode.Elements("ReviewStep").Any())
            {
                foreach (var reviewStepNode in pendingReviewsNode.Elements("ReviewStep"))
                {
                    var reviewStep = parseReviewStepNode(reviewStepNode);
                    pendingReviews.Enqueue(reviewService.CreateReviewStep(reviewStep));
                }
            }

            part.PendingReviews = pendingReviews;

            var reviewHistory = new Stack<ReviewStepRecord>();

            var reviewHistoryNode = reviewNode.Element("ReviewHistory");

            if (reviewHistoryNode != null && reviewHistoryNode.Elements("ReviewStep").Any())
            {
                foreach (var reviewStepNode in reviewHistoryNode.Elements("ReviewStep"))
                {
                    var reviewStep = parseReviewStepNode(reviewStepNode);
                    reviewHistory.Push(reviewService.CreateReviewStep(reviewStep));
                }
            }

            part.ReviewHistory = reviewHistory;
        }

        private XElement createReviewStepNode(ReviewStepRecord reviewStep)
        {
            var reviewStepNode = new XElement("ReviewStep");

            reviewStepNode.SetAttributeValue("ReviewPartIdentifier", reviewStep.ReviewPartIdentifier);
            reviewStepNode.SetAttributeValue("ApprovingStatus", reviewStep.ApprovingStatus);
            reviewStepNode.SetAttributeValue("RejectingStatus", reviewStep.RejectingStatus);
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
                ReviewerName = reviewStepNode.SafeGetAttribute("ReviewerName"),
                ReviewerEmail = reviewStepNode.SafeGetAttribute("ReviewerEmail")
            };
            
            WorkflowStatus status;

            if (Enum.TryParse(reviewStepNode.SafeGetAttribute("ApprovingStatus"), out status))
                reviewStep.ApprovingStatus = status;
            else
                reviewStep.ApprovingStatus = WorkflowStatus.Undefined;

            if (Enum.TryParse(reviewStepNode.SafeGetAttribute("RejectingStatus"), out status))
                reviewStep.RejectingStatus = status;
            else
                reviewStep.RejectingStatus = WorkflowStatus.Undefined;

            if (Enum.TryParse(reviewStepNode.SafeGetAttribute("ReviewDecision"), out status))
                reviewStep.ReviewDecision = status;
            else
                reviewStep.ReviewDecision = WorkflowStatus.Undefined;

            DateTime reviewDate;

            if (DateTime.TryParse(reviewStepNode.SafeGetAttribute("ReviewDate"), out reviewDate))
                reviewStep.ReviewDate = reviewDate;
            else
                reviewStep.ReviewDate = null;

            return reviewStep;
        }
    }
}