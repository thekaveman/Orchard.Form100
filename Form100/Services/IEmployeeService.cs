using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IEmployeeService : IDependency
    {
        JobStepRecord GetJobStep(int id);
        
        JobStepRecord CreateJobStep();

        JobStepRecord UpdateJobStep(JobStepRecord source);

        EmployeePartViewModel GetEmployeeViewModel(EmployeePart source);

        void UpdateEmployee(EmployeePartViewModel source, EmployeePart target);
    }
}
