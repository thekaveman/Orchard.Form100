using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace CSM.Form100.Drivers
{
    using Models;
    using Services;

    public class ReviewPartDriver : ContentPartDriver<ReviewPart>
    {
        private readonly string dateFormat = "yyyy-dd-MM";

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
                    () => shapeHelper.Parts_Review_Approvals(Approvals: part.ApprovalChain)
                ),
                ContentShape(
                    "Parts_Review_Status",
                    () => shapeHelper.Parts_Review_Status(Status: part.Status)
                )
            );
        }

        protected override void Exporting(ReviewPart part, ExportContentContext context)
        {
            var reviewNode = context.Element(part.PartDefinition.Name);

            reviewNode.SetAttributeValue("Status", part.Status);

            if (part.ApprovalChain.Any())
            {
                var copy = new Queue<ReviewApprovalRecord>(part.ApprovalChain);
                var approvalChainNode = new XElement("ApprovalChain");

                while (copy.Any())
                {
                    var approvalRecord = copy.Dequeue();
                    var approvalRecordNode = new XElement("ApprovalRecord");
                    
                    approvalRecordNode.SetAttributeValue("Id", approvalRecord.Id);
                    approvalRecordNode.SetAttributeValue("IsApproved", approvalRecord.IsApproved);
                    approvalRecordNode.SetAttributeValue("ApprovalDate", approvalRecord.ApprovalDate.HasValue ? approvalRecord.ApprovalDate.Value.ToString(dateFormat) : String.Empty);
                    approvalRecordNode.SetAttributeValue("ApproverName", approvalRecord.ApproverName);

                    approvalChainNode.Add(approvalRecordNode);
                }

                reviewNode.Add(approvalChainNode);
            }
        }
    }
}