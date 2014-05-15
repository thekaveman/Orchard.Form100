using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace CSM.Form100.Drivers
{
    public class ReviewPartDriver : ContentPartDriver<ReviewPart>
    {
        private readonly string dateFormat = "yyyy-dd-MM";

        private readonly IReviewService reviewService;

        public ReviewPartDriver(IReviewService reviewApprovalService)
        {
            this.reviewService = reviewApprovalService;
        }

        protected override string Prefix
        {
            get { return "Review"; }
        }

        /// <summary>
        /// Respond to requests to display data from this part
        /// e.g. return a bunch of so called "display" shapes
        /// </summary>
        protected override DriverResult Display(ReviewPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                ContentShape(
                    "Parts_Review_Approvals",
                    () => shapeHelper.Parts_Review_Approvals(Approvals: part.ApprovalChain)
                ),
                ContentShape(
                    "Parts_Review_Status",
                    () => shapeHelper.Parts_Review_Status(Status: part.Status)
                )
            );
        }

        /// <summary>
        /// Define how the part's data is exported.
        /// Hint: it uses XML.
        /// </summary>
        protected override void Exporting(ReviewPart part, ExportContentContext context)
        {
            var reviewNode = context.Element(part.PartDefinition.Name);

            reviewNode.SetAttributeValue("Status", part.Status);

            if (part.ApprovalChain.Any())
            {
                var copy = new Queue<ReviewDecisionRecord>(part.ApprovalChain);
                var approvalChainNode = new XElement("ApprovalChain");

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
                    var reviewDecision = parseReviewDecisionNode(reviewDecisionNode);

                    approvalChain.Enqueue(reviewDecision);
                }
            }

            part.ApprovalChain = approvalChain;
        }

        private XElement createReviewDecisionNode(ReviewDecisionRecord reviewDecision)
        {
            var reviewDecisionNode = new XElement("ReviewDecision");

            reviewDecisionNode.SetAttributeValue("Id", reviewDecision.Id);

            reviewDecisionNode.SetAttributeValue("IsApproved", reviewDecision.IsApproved);

            reviewDecisionNode.SetAttributeValue("ReviewDate", reviewDecision.ReviewDate.HasValue ? reviewDecision.ReviewDate.Value.ToString(dateFormat) : String.Empty);

            reviewDecisionNode.SetAttributeValue("ReviewerName", reviewDecision.ReviewerName);

            return reviewDecisionNode;
        }

        private ReviewDecisionRecord parseReviewDecisionNode(XElement reviewDecisionNode)
        {
            var reviewDecision = reviewService.Create();

            int id;

            if (int.TryParse(reviewDecisionNode.SafeGetAttribute("Id"), out id))
                reviewDecision.Id = id;
            else
                throw new InvalidOperationException("Couldn't parse Id attribute of ReviewDecision node to int.");

            bool approved = false;

            bool.TryParse(reviewDecisionNode.SafeGetAttribute("IsApproved"), out approved);

            reviewDecision.IsApproved = approved;

            DateTime reviewDate;

            if (DateTime.TryParse(reviewDecisionNode.SafeGetAttribute("ReviewDate"), out reviewDate))
                reviewDecision.ReviewDate = reviewDate;
            else
                reviewDecision.ReviewDate = null;

            reviewDecision.ReviewerName = reviewDecisionNode.SafeGetAttribute("ReviewerName");

            return reviewDecision;
        }
    }
}