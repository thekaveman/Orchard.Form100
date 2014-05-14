using Orchard;

namespace CSM.Form100.Services
{
    using Models;

    public interface IReviewApprovalService : IDependency
    {
        ReviewApprovalRecord Get(int id);
    }
}
