using Orchard;

namespace CSM.Form100.Services
{
    using Models;

    public interface IJobStepService : IDependency
    {
        JobStepRecord Get(int id);

        JobStepRecord Create();
    }
}