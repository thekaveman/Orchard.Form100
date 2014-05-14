using Orchard.Data;

namespace CSM.Form100.Services
{
    using Models;

    public class ReviewApprovalService : IReviewApprovalService
    {
        private readonly IRepository<ReviewApprovalRecord> reviewApprovalRepository;

        public ReviewApprovalService(IRepository<ReviewApprovalRecord> reviewApprovalRepository)
        {
            this.reviewApprovalRepository = reviewApprovalRepository;
        }

        public ReviewApprovalRecord Get(int id)
        {
            var approval = reviewApprovalRepository.Get(id);
            return approval;
        }
    }
}