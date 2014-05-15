using CSM.Form100.Models;
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

        public ReviewDecisionRecord Get(int id)
        {
            var approval = reviewDecisionRepository.Get(id);

            return approval;
        }

        public ReviewDecisionRecord Create()
        {
            var reviewDecision = new ReviewDecisionRecord();
            
            reviewDecisionRepository.Create(reviewDecision);

            return reviewDecision;
        }
    }
}