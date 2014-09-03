using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    /// <summary>
    /// An injectable service for interacting with EmployeePart data.
    /// </summary>
    public interface IEmployeeService : IDependency
    {
        /// <summary>
        /// Convert an EmployeePart to its corresponding view model representation.
        /// </summary>
        /// <param name="part">The EmployeePart to convert.</param>
        /// <returns>An EmployeePartViewModel object representing the given EmployeePart.</returns>
        EmployeePartViewModel GetEmployeeViewModel(EmployeePart part);

        /// <summary>
        /// Update an EmployeePart with data from a view model.
        /// </summary>
        /// <param name="viewModel">The EmployeePartViewModel object containg updated data.</param>
        /// <param name="part">The target EmployeePart to update.</param>
        void UpdateEmployee(EmployeePartViewModel viewModel, EmployeePart part);

        /// <summary>
        /// Get a JobStepRecord by id.
        /// </summary>
        JobStepRecord GetJobStep(int id);

        /// <summary>
        /// Convert an EmployeePart to its corresponding view model representation.
        /// </summary>
        /// <param name="part">The EmployeePart to convert.</param>
        /// <returns>An EmployeePartViewModel object representing the given EmployeePart.</returns>
        JobStepRecordViewModel GetJobStepViewModel(JobStepRecord jobStep, string qualifier);

        /// <summary>
        /// Create a new JobStepRecord in the DB.
        /// </summary>
        /// <param name="step">The JobStepRecord containing the data to be persisted.</param>
        /// <returns>The newly persisted JobStepRecord.</returns>
        JobStepRecord CreateJobStep(JobStepRecord jobStep);
        
        /// <summary>
        /// Update an existing JobStepRecord in the DB.
        /// </summary>
        /// <param name="step">The JobStepRecord containing the updated data.</param>
        /// <returns>The newly updated JobStepRecord.</returns>
        JobStepRecord UpdateJobStep(JobStepRecord jobStep);

        /// <summary>
        /// Update a JobStep with data from a view model.
        /// </summary>
        /// <param name="viewModel">The JobStepViewModel object containg updated data.</param>
        /// <param name="part">The target JobStep to update.</param>
        JobStepRecord UpdateJobStep(JobStepRecordViewModel viewModel, string employeeIdentifier);
    }
}
