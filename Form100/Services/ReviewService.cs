using System;
using System.Collections.Generic;
using System.Linq;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Newtonsoft.Json;
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
                deserializedApprovalChain = new Queue<ReviewDecisionRecord>(deserializeApprovalChain(viewModel.ApprovalChainData, part.Id));
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

        internal Queue<ReviewDecisionRecord> deserializeApprovalChain(string approvalChainData, int approvalChainId)
        {
            var approvalChain = new Queue<ReviewDecisionRecord>();
            var reviewerTemplate = new { Id = 0, IsApproved = false, ReviewDate = default(DateTime?), ReviewerName = "", ReviewerEmail = "" };
            var reviewersData = JsonConvert.DeserializeAnonymousType(approvalChainData, new[] { reviewerTemplate });

            foreach(var reviewerData in reviewersData)
            {
                var reviewDecision = new ReviewDecisionRecord() {
                    Id =  reviewerData.Id,
                    ReviewPartId = approvalChainId,
                    IsApproved = reviewerData.IsApproved,
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