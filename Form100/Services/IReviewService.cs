using Orchard;

namespace CSM.Form100.Services
{
    using Models;

    public interface IReviewService : IDependency
    {
        ReviewDecisionRecord Get(int id);

        ReviewDecisionRecord Create();
    }
}
