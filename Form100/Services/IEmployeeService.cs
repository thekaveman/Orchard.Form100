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

        JobStepRecordViewModel GetJobStepViewModel(JobStepRecord jobStep, string qualifier);

        JobStepRecord CreateJobStep(JobStepRecord jobStep);
        
        JobStepRecord UpdateJobStep(JobStepRecordViewModel viewModel, string employeeIdentifier);

        JobStepRecord UpdateJobStep(JobStepRecord jobStep);
    }
}
