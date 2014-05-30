using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IReviewService : IDependency
    {
        ReviewPartViewModel GetReviewViewModel(ReviewPart part);
        
        void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part);
        
        ReviewStepRecord GetReviewStep(int id);

        ReviewStepRecord CreateReviewStep(ReviewStepRecord decision);

        ReviewStepRecord UpdateReviewStep(ReviewStepRecord decision);
    }
}
