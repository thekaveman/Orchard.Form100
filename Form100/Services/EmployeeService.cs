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

        public EmployeePartViewModel GetEmployeeViewModel(EmployeePart part)
        {
            var viewModel = new EmployeePartViewModel();

            viewModel.EmployeeId = part.EmployeeId > 0 ? part.EmployeeId.ToString() : String.Empty;
            viewModel.FirstName = part.FirstName;
            viewModel.LastName = part.LastName;
            viewModel.PriorJobStep = part.PriorJobStep;
            viewModel.CurrentJobStep = part.CurrentJobStep;

            return viewModel;
        }

        public void UpdateEmployee(EmployeePartViewModel viewModel, EmployeePart part)
        {
            int employeeId;

            if (int.TryParse(viewModel.EmployeeId, out employeeId))
                part.EmployeeId = employeeId;
            else
                throw new InvalidOperationException("Couldn't parse EmployeeId in editor template.");

            part.FirstName = viewModel.FirstName;
            part.LastName = viewModel.LastName;

            part.PriorJobStep = UpdateJobStep(viewModel.PriorJobStep);
            part.CurrentJobStep = UpdateJobStep(viewModel.CurrentJobStep);
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

        public JobStepRecord UpdateJobStep(JobStepRecord jobStep)
        {
            jobStepRepository.Update(jobStep);

            return jobStep;
        }
    }
}