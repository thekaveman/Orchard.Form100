using System;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard.Data;

namespace CSM.Form100.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<JobStepRecord> jobStepRepository;

        public EmployeeService(IRepository<JobStepRecord> jobStepRepository)
        {
            this.jobStepRepository = jobStepRepository;
        }

        public EmployeePartViewModel GetEmployeeViewModel(EmployeePart source)
        {
            var target = new EmployeePartViewModel();

            target.EmployeeId = source.EmployeeId > 0 ? source.EmployeeId.ToString() : String.Empty;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.PriorJobStep = source.PriorJobStep;
            target.CurrentJobStep = source.CurrentJobStep;

            return target;
        }

        public void UpdateEmployee(EmployeePartViewModel source, EmployeePart target)
        {
            int employeeId;

            if (int.TryParse(source.EmployeeId, out employeeId))
                target.EmployeeId = employeeId;
            else
                throw new InvalidOperationException("Couldn't parse EmployeeId in editor template.");

            target.FirstName = source.FirstName;
            target.LastName = source.LastName;

            target.PriorJobStep = UpdateJobStep(source.PriorJobStep);
            target.CurrentJobStep = UpdateJobStep(source.CurrentJobStep);
        }

        public JobStepRecord GetJobStep(int id)
        {
            var jobStep = jobStepRepository.Get(id);

            return jobStep;
        }

        public JobStepRecord CreateJobStep()
        {
            var jobStep = new JobStepRecord();

            jobStepRepository.Create(jobStep);

            return jobStep;
        }

        public JobStepRecord UpdateJobStep(JobStepRecord source)
        {
            jobStepRepository.Update(source);

            return source;
        }
    }
}