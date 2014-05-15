using Orchard.Data;

namespace CSM.Form100.Services
{
    using Models;

    public class ReviewService : IReviewService
    {
        private readonly IRepository<ReviewDecisionRecord> reviewApprovalRepository;

        public ReviewService(IRepository<ReviewDecisionRecord> reviewApprovalRepository)
        {
            this.reviewApprovalRepository = reviewApprovalRepository;
        }

        public ReviewDecisionRecord Get(int id)
        {
            var approval = reviewApprovalRepository.Get(id);

            return approval;
        }

        public ReviewDecisionRecord Create()
        {
            var record = new ReviewDecisionRecord();
            
            reviewApprovalRepository.Create(record);

            return record;
        }
    }
}