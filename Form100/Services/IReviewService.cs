using CSM.Form100.Models;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IReviewService : IDependency
    {
        ReviewDecisionRecord Get(int id);

        ReviewDecisionRecord Create();
    }
}
