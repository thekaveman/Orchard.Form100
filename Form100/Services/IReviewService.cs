using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IReviewService : IDependency
    {
        ReviewDecisionRecord GetReviewDecision(int id);

        ReviewDecisionRecord CreateReviewDecision();

        ReviewPartViewModel GetReviewViewModel(ReviewPart part);

        void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part);
    }
}
