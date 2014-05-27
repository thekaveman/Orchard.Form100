using System;
using System.Collections.Generic;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;

namespace CSM.Form100.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<ReviewDecisionRecord> reviewDecisionRepository;
        
        public ReviewService(IRepository<ReviewDecisionRecord> reviewDecisionRepository)
        {
            this.reviewDecisionRepository = reviewDecisionRepository;
        }

        public ReviewPartViewModel GetReviewViewModel(ReviewPart part)
        {
            var viewModel = new ReviewPartViewModel();

            if (part.ApprovalChain != null)
            {
                viewModel.ApprovalChainData = serializeApprovalChain(part.ApprovalChain);
            }

            viewModel.Status = part.Status;

            return viewModel;
        }

        public void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part)
        {
            part.Status = viewModel.Status;
            var deserializedApprovalChain = new Queue<ReviewDecisionRecord>();

            if (!String.IsNullOrEmpty(viewModel.ApprovalChainData))
            {
                var identityPart = part.As<IdentityPart>();
                deserializedApprovalChain = new Queue<ReviewDecisionRecord>(deserializeApprovalChain(viewModel.ApprovalChainData, identityPart.Identifier ?? part.Id.ToString()));
            }

            part.ApprovalChain = deserializedApprovalChain;
        }

        public ReviewDecisionRecord GetReviewDecision(int id)
        {
            var approval = reviewDecisionRepository.Get(id);

            return approval;
        }

        public ReviewDecisionRecord CreateReviewDecision(ReviewDecisionRecord decision)
        {
            reviewDecisionRepository.Create(decision);

            return decision;
        }
        
        public ReviewDecisionRecord UpdateReviewDecision(ReviewDecisionRecord decision)
        {
            reviewDecisionRepository.Update(decision);

            return decision;
        }

        internal Queue<ReviewDecisionRecord> deserializeApprovalChain(string approvalChainData, string reviewPartId)
        {
            var approvalChain = new Queue<ReviewDecisionRecord>();
            var reviewersData = JsonConvert.DeserializeObject<IEnumerable<ReviewDecisionRecord>>(approvalChainData);

            foreach(var reviewerData in reviewersData)
            {
                var reviewDecision = new ReviewDecisionRecord() {
                    Id =  reviewerData.Id,
                    ReviewPartIdentifier = reviewPartId,
                    TargetStatus = reviewerData.TargetStatus,
                    ReviewDate = reviewerData.ReviewDate,
                    ReviewerName = reviewerData.ReviewerName,
                    ReviewerEmail = reviewerData.ReviewerEmail
                };
                
                reviewDecision = reviewDecision.Id > 0 ? UpdateReviewDecision(reviewDecision) : CreateReviewDecision(reviewDecision);
                
                approvalChain.Enqueue(reviewDecision);
            }

            return approvalChain;
        }

        internal string serializeApprovalChain(Queue<ReviewDecisionRecord> approvalChain)
        {
            var json = JsonConvert.SerializeObject(approvalChain.ToArray());
            return json;
        }
    }
}