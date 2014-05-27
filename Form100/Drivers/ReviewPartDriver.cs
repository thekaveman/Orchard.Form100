using System;
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

            if (part.ApprovalChain != null && part.ApprovalChain.Any())
            {
                var approvalChainNode = new XElement("ApprovalChain");
                var copy = part.ApprovalChain.Copy();
                
                while (copy.Any())
                {
                    var decisionRecord = copy.Dequeue();
                    var decisionRecordNode = createReviewDecisionNode(decisionRecord);
                    approvalChainNode.Add(decisionRecordNode);
                }

                reviewNode.Add(approvalChainNode);
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

            var approvalChain = new Queue<ReviewDecisionRecord>();

            var approvalChainNode = reviewNode.Element("ApprovalChain");

            if (approvalChainNode != null && approvalChainNode.Elements("ReviewDecision").Any())
            {
                foreach (var reviewDecisionNode in approvalChainNode.Elements("ReviewDecision"))
                {
                    var decision = parseReviewDecisionNode(reviewDecisionNode);
                    approvalChain.Enqueue(reviewService.CreateReviewDecision(decision));
                }
            }

            part.ApprovalChain = approvalChain;
        }

        private XElement createReviewDecisionNode(ReviewDecisionRecord reviewDecision)
        {
            var reviewDecisionNode = new XElement("ReviewDecision");

            reviewDecisionNode.SetAttributeValue("ReviewPartIdentifier", reviewDecision.ReviewPartIdentifier);
            reviewDecisionNode.SetAttributeValue("TargetStatus", reviewDecision.TargetStatus);
            reviewDecisionNode.SetAttributeValue("ReviewDate", reviewDecision.ReviewDate.HasValue ? reviewDecision.ReviewDate.Value.ToString(FormatProvider.DateFormat) : String.Empty);
            reviewDecisionNode.SetAttributeValue("ReviewerName", reviewDecision.ReviewerName);
            reviewDecisionNode.SetAttributeValue("ReviewerEmail", reviewDecision.ReviewerEmail);

            return reviewDecisionNode;
        }

        private ReviewDecisionRecord parseReviewDecisionNode(XElement reviewDecisionNode)
        {
            ReviewDecisionRecord reviewDecision = new ReviewDecisionRecord();
            
            reviewDecision.ReviewPartIdentifier = reviewDecisionNode.SafeGetAttribute("ReviewPartIdentifier");

            WorkflowStatus targetStatus;

            if (Enum.TryParse(reviewDecisionNode.SafeGetAttribute("TargetStatus"), out targetStatus))
                reviewDecision.TargetStatus = targetStatus;
            else
                reviewDecision.TargetStatus = WorkflowStatus.Undefined;

            DateTime reviewDate;

            if (DateTime.TryParse(reviewDecisionNode.SafeGetAttribute("ReviewDate"), out reviewDate))
                reviewDecision.ReviewDate = reviewDate;
            else
                reviewDecision.ReviewDate = null;

            reviewDecision.ReviewerName = reviewDecisionNode.SafeGetAttribute("ReviewerName");

            reviewDecision.ReviewerEmail = reviewDecisionNode.SafeGetAttribute("ReviewerEmail");

            return reviewDecision;
        }
    }
}