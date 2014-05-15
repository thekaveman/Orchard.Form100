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
            throw new System.NotImplementedException();
        }

        public void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part)
        {
            throw new System.NotImplementedException();
        }
    }
}