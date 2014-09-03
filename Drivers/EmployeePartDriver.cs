using System;
using System.Xml.Linq;
using CSM.Form100.Models;
using CSM.Form100.Services;
using CSM.Form100.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace CSM.Form100.Drivers
{
    /// <summary>
    /// ContentPartDrivers are mini-Controllers that work at the ContentPart level
    /// </summary>
    public class EmployeePartDriver : ContentPartDriver<EmployeePart>
    {
        private readonly IEmployeeService employeeService;
        
        public EmployeePartDriver(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        /// <summary>
        /// Needed for HTML input id uniqueness
        /// </summary>
        protected override string Prefix
        {
            get { return "CSM_Form100_EmployeePart"; }
        }

        /// <summary>
        /// Respond to requests to display data from this part
        /// e.g. return a bunch of so called "display" shapes
        /// </summary>
        protected override DriverResult Display(EmployeePart part, string displayType, dynamic shapeHelper)
        {
            var viewModel = employeeService.GetEmployeeViewModel(part);

            return ContentShape(
                "Parts_Employee",
                () => shapeHelper.Parts_Employee(viewModel)
            );  
        }

        /// <summary>
        /// Respond to GET requests for an edit view of this part.
        /// e.g. return a bunch of "edit" shapes
        /// </summary>
        protected override DriverResult Editor(EmployeePart part, dynamic shapeHelper)
        {
            var viewModel = employeeService.GetEmployeeViewModel(part);

            return ContentShape(
                "Parts_Employee_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Employee",
                    Model: viewModel,
                    Prefix: Prefix
                )
            );
        }

        /// <summary>
        /// Respond to POST requests for updating this part's data.
        /// </summary>
        protected override DriverResult Editor(EmployeePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var employeeViewModel = new EmployeePartViewModel();
            
            if (updater.TryUpdateModel(employeeViewModel, Prefix, null, null))
            {
                employeeService.UpdateEmployee(employeeViewModel, part);
            }

            return Editor(part, shapeHelper);
        }

        /// <summary>
        /// Define how the part's data is exported.
        /// Hint: it uses XML.
        /// </summary>
        protected override void Exporting(EmployeePart part, ExportContentContext context)
        {
            var employeeNode = context.Element(part.PartDefinition.Name);

            employeeNode.SetAttributeValue("EmployeeId", part.EmployeeId);
            employeeNode.SetAttributeValue("FirstName", part.FirstName);
            employeeNode.SetAttributeValue("LastName", part.LastName);

            if (part.PriorJobStep != null)
            {
                var jobStepNode = createJobStepNode("PriorJobStep", part.PriorJobStep);
                employeeNode.Add(jobStepNode);
            }

            if (part.CurrentJobStep != null)
            {
                var jobStepNode = createJobStepNode("CurrentJobStep", part.CurrentJobStep);
                employeeNode.Add(jobStepNode);
            }
        }

        /// <summary>
        /// Define how the part's data is imported.
        /// Hint: it's the inverse of Exporting.
        /// </summary>
        protected override void Importing(EmployeePart part, ImportContentContext context)
        {
            var employeeNode = context.Data.Element(part.PartDefinition.Name);

            context.ImportAttribute(employeeNode.Name.LocalName, "EmployeeId", e =>
            {
                int employeeId;

                if (int.TryParse(e, out employeeId))
                    part.EmployeeId = employeeId;
                else
                    throw new InvalidOperationException("Couldn't parse EmployeeId attribute to int.");
            });

            context.ImportAttribute(employeeNode.Name.LocalName, "FirstName", n => part.FirstName = n);
            context.ImportAttribute(employeeNode.Name.LocalName, "LastName", n => part.LastName = n);

            var currentJobStepNode = employeeNode.Element("CurrentJobStep");
            if (currentJobStepNode != null)
            {
                var jobStep = parseJobStepNode(currentJobStepNode);

                if (jobStep.Id > 0)
                    part.CurrentJobStep = employeeService.UpdateJobStep(jobStep);
                else
                    part.CurrentJobStep = employeeService.CreateJobStep(jobStep);
            }

            var priorJobStepNode = employeeNode.Element("PriorJobStep");
            if (priorJobStepNode != null)
            {
                var jobStep = parseJobStepNode(priorJobStepNode);

                if (jobStep.Id > 0)
                    part.PriorJobStep = employeeService.UpdateJobStep(jobStep);
                else
                    part.PriorJobStep = employeeService.CreateJobStep(jobStep);
            }
        }

        private XElement createJobStepNode(string name, JobStepRecord jobStep)
        {
            var jobStepNode = new XElement(name);

            jobStepNode.SetAttributeValue("EmployeePartIdentifier", jobStep.EmployeePartIdentifier);
            jobStepNode.SetAttributeValue("Title", jobStep.Title);
            jobStepNode.SetAttributeValue("DepartmentName", jobStep.DepartmentName);
            jobStepNode.SetAttributeValue("DivisionName", jobStep.DivisionName);
            jobStepNode.SetAttributeValue("DivisionNumber", jobStep.DivisionNumber);
            jobStepNode.SetAttributeValue("StepNumber", jobStep.StepNumber);
            jobStepNode.SetAttributeValue("HoursPerWeek", jobStep.HoursPerWeek);
            jobStepNode.SetAttributeValue("HourlyRate", jobStep.HourlyRate);
            
            return jobStepNode;
        }

        private JobStepRecord parseJobStepNode(XElement jobStepNode)
        {
            JobStepRecord jobStep = new JobStepRecord()
            {
                EmployeePartIdentifier = jobStepNode.SafeGetAttribute("EmployeePartIdentifier"),
                Title = jobStepNode.SafeGetAttribute("Title"),
                DepartmentName = jobStepNode.SafeGetAttribute("DepartmentName"),
                DivisionName = jobStepNode.SafeGetAttribute("DivisionName")
            };
            
            int intVar;

            if (int.TryParse(jobStepNode.SafeGetAttribute("DivisionNumber"), out intVar))
                jobStep.DivisionNumber = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node DivisionNumber attribute to int.", jobStepNode.Name.LocalName));

            if (int.TryParse(jobStepNode.SafeGetAttribute("StepNumber"), out intVar))
                jobStep.StepNumber = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node StepNumber attribute to int.", jobStepNode.Name.LocalName));

            if(int.TryParse(jobStepNode.SafeGetAttribute("HoursPerWeek"), out intVar))
                jobStep.HoursPerWeek = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node HoursPerWeek attribute to int.", jobStepNode.Name.LocalName));

            decimal decimalVar;

            if (decimal.TryParse(jobStepNode.SafeGetAttribute("HourlyRate"), out decimalVar))
                jobStep.HourlyRate = decimalVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node HourlyPay attribute to decimal.", jobStepNode.Name.LocalName));

            return jobStep;
        }
    }
}