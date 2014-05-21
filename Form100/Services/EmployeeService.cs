﻿using System;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard.ContentManagement;
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

            bool needsPriorJobStepEditor = true;
            string effectiveDate = "Effective Date";
            ActionPart actionPart = part.As<ActionPart>();

            if (actionPart != null && actionPart.EffectiveDate.HasValue)
            {
                effectiveDate = actionPart.EffectiveDate.Value.ToString(FormatProvider.DateFormat);
                needsPriorJobStepEditor = actionPart.Category == ActionCategory.Change;
            }

            viewModel.CurrentJobStep = GetJobStepViewModel(part.CurrentJobStep, String.Format("as of {0}", effectiveDate));
            viewModel.NeedsPriorJobStepEditor = needsPriorJobStepEditor;
            viewModel.PriorJobStep = GetJobStepViewModel(part.PriorJobStep, String.Format("prior to {0}", effectiveDate));

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
            
            part.CurrentJobStep = UpdateJobStep(viewModel.CurrentJobStep);
            
            part.PriorJobStep = UpdateJobStep(viewModel.PriorJobStep);
            
        }

        public JobStepRecord GetJobStep(int id)
        {
            var jobStep = jobStepRepository.Get(id);

            return jobStep;
        }

        public JobStepRecord CreateJobStep(JobStepRecord jobStep)
        {
            jobStepRepository.Create(jobStep);

            return jobStep;
        }
        
        public JobStepRecordViewModel GetJobStepViewModel(JobStepRecord jobStep, string qualifier)
        {
            var viewModel = new JobStepRecordViewModel() { Qualifier = qualifier };

            if (jobStep != null)
            {
                viewModel.Id = jobStep.Id;
                viewModel.Title = jobStep.Title;
                viewModel.DepartmentName = jobStep.DepartmentName;
                viewModel.DivisionName = jobStep.DivisionName;
                viewModel.DivisionNumber = jobStep.DivisionNumber.ToString();
                viewModel.StepNumber = jobStep.StepNumber.ToString();
                viewModel.HoursPerWeek = jobStep.HoursPerWeek.ToString();
                viewModel.HourlyRate = jobStep.HourlyRate.ToString();
            }

            return viewModel;
        }

        public JobStepRecord UpdateJobStep(JobStepRecord jobStep)
        {
            jobStepRepository.Update(jobStep);

            return jobStep;
        }

        public JobStepRecord UpdateJobStep(JobStepRecordViewModel viewModel)
        {
            var jobStep = GetJobStep(viewModel.Id) ?? new JobStepRecord();

            jobStep.Title = viewModel.Title;
            jobStep.DepartmentName = viewModel.DepartmentName;
            jobStep.DivisionName = viewModel.DivisionName;

            int intVal;

            if (int.TryParse(viewModel.DivisionNumber, out intVal))
                jobStep.DivisionNumber = intVal;
            else
                throw new InvalidOperationException("Couldn't parse JobStep DivisionNumber to int from editor template.");

            if (int.TryParse(viewModel.StepNumber, out intVal))
                jobStep.StepNumber = intVal;
            else
                throw new InvalidOperationException("Couldn't parse JobStep StepNumber to int from editor template.");

            if (int.TryParse(viewModel.HoursPerWeek, out intVal))
                jobStep.HoursPerWeek = intVal;
            else
                throw new InvalidOperationException("Couldn't parse JobStep HoursPerWeek to int from editor template.");

            decimal hourlyRate;

            if (decimal.TryParse(viewModel.HourlyRate, out hourlyRate))
                jobStep.HourlyRate = hourlyRate;
            else
                throw new InvalidOperationException("Couldn't parse JobStep HourlyRate to decimal from editor template.");

            if (jobStep.Id > 0)
                return UpdateJobStep(jobStep);
            else
                return CreateJobStep(jobStep);
        }
    }
}