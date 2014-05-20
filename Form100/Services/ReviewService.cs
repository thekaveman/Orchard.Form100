using System.Collections.Generic;
using System.Linq;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
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

        public ReviewDecisionRecord GetReviewDecision(int id)
        {
            var approval = reviewDecisionRepository.Get(id);

            return approval;
        }

        public ReviewDecisionRecord CreateReviewDecision()
        {
            var reviewDecision = new ReviewDecisionRecord();
            
            reviewDecisionRepository.Create(reviewDecision);

            return reviewDecision;
        }

        public ReviewPartViewModel GetReviewViewModel(ReviewPart part)
        {
            var viewModel = new ReviewPartViewModel();

            viewModel.ApprovalChain = new Queue<ReviewDecisionRecord>();
            
            if (part.ApprovalChain != null && part.ApprovalChain.Any())
            {
                var copy = part.ApprovalChain.Copy();
                
                while (copy.Any())
                {
                    viewModel.ApprovalChain.Enqueue(copy.Dequeue());
                }
            }

            viewModel.Status = part.Status;

            return viewModel;
        }

        public void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part)
        {
            part.Status = viewModel.Status;
            part.ApprovalChain = new Queue<ReviewDecisionRecord>();

            if (viewModel.ApprovalChain.Any())
            {
                var decision = viewModel.ApprovalChain.Dequeue();
                part.ApprovalChain.Enqueue(UpdateReviewDecision(decision));
            }
        }

        public ReviewDecisionRecord UpdateReviewDecision(ReviewDecisionRecord decision)
        {
            reviewDecisionRepository.Update(decision);

            return decision;
        }
    }
}