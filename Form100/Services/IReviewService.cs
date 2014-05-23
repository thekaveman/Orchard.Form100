using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IReviewService : IDependency
    {
        ReviewPartViewModel GetReviewViewModel(ReviewPart part);

        void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part);

        ReviewDecisionRecord GetReviewDecision(int id);

        ReviewDecisionRecord CreateReviewDecision(ReviewDecisionRecord decision);

        ReviewDecisionRecord UpdateReviewDecision(ReviewDecisionRecord decision);
    }
}
