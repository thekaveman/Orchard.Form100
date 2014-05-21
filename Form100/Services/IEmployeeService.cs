using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IEmployeeService : IDependency
    {
        EmployeePartViewModel GetEmployeeViewModel(EmployeePart part);

        void UpdateEmployee(EmployeePartViewModel viewModel, EmployeePart part);

        JobStepRecord GetJobStep(int id);

        JobStepRecord CreateJobStep();

        JobStepRecord UpdateJobStep(JobStepRecord jobStep);

        JobStepRecordViewModel GetJobStepViewModel(JobStepRecord jobStep, string qualifier);

        JobStepRecord UpdateJobStep(JobStepRecordViewModel viewModel);
    }
}
