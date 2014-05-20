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
    public class EmployeePartDriver : ContentPartDriver<EmployeePart>
    {
        private readonly IEmployeeService employeeService;
        
        public EmployeePartDriver(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        protected override string Prefix
        {
            get { return "Employee"; }
        }

        /// <summary>
        /// Respond to requests to display data from this part
        /// e.g. return a bunch of so called "display" shapes
        /// </summary>
        protected override DriverResult Display(EmployeePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape(
                "Parts_Employee",
                () => shapeHelper.Parts_Employee(part)
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
            var viewModel = new EmployeePartViewModel();

            if (updater.TryUpdateModel(viewModel, Prefix, null, null))
            {
                employeeService.UpdateEmployee(viewModel, part);
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
            
            var priorJobStepNode = employeeNode.Element("PriorJobStep");
            if (priorJobStepNode != null)
            {
                part.PriorJobStep = parseJobStepNode(priorJobStepNode);
            }

            var currentJobStepNode = employeeNode.Element("CurrentJobStep");
            if (currentJobStepNode != null)
            {
                part.CurrentJobStep = parseJobStepNode(currentJobStepNode);
            }
        }

        private XElement createJobStepNode(string name, JobStepRecord jobStep)
        {
            var jobStepNode = new XElement(name);
            
            jobStepNode.SetAttributeValue("Id", jobStep.Id);
            jobStepNode.SetAttributeValue("Title", jobStep.Title);
            jobStepNode.SetAttributeValue("DepartmentName", jobStep.DepartmentName);
            jobStepNode.SetAttributeValue("DivisionName", jobStep.DivisionName);
            jobStepNode.SetAttributeValue("DivisionNumber", jobStep.DivisionNumber);
            jobStepNode.SetAttributeValue("StepNumber", jobStep.StepNumber);
            jobStepNode.SetAttributeValue("HoursPerWeek", jobStep.HoursPerWeek);
            jobStepNode.SetAttributeValue("HourlyPay", jobStep.HourlyPay);
            
            return jobStepNode;
        }

        private JobStepRecord parseJobStepNode(XElement jobStepNode)
        {
            var jobStep = employeeService.CreateJobStep();

            jobStep.Title = jobStepNode.SafeGetAttribute("Title");
            jobStep.DepartmentName = jobStepNode.SafeGetAttribute("DepartmentName");
            jobStep.DivisionName = jobStepNode.SafeGetAttribute("DivisionName");

            int intVar;

            string stringVar = jobStepNode.SafeGetAttribute("Id");

            if (int.TryParse(stringVar, out intVar))
                jobStep.Id = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node Id attribute to int.", jobStepNode.Name.LocalName));

            stringVar = jobStepNode.SafeGetAttribute("DivisionNumber");
            
            if (int.TryParse(stringVar, out intVar))
                jobStep.DivisionNumber = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node DivisionNumber attribute to int.", jobStepNode.Name.LocalName));

            stringVar = jobStepNode.SafeGetAttribute("StepNumber");

            if (int.TryParse(stringVar, out intVar))
                jobStep.StepNumber = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node StepNumber attribute to int.", jobStepNode.Name.LocalName));

            stringVar = jobStepNode.SafeGetAttribute("HoursPerWeek");

            if(int.TryParse(stringVar, out intVar))
                jobStep.HoursPerWeek = intVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node HoursPerWeek attribute to int.", jobStepNode.Name.LocalName));

            decimal decimalVar;

            stringVar = jobStepNode.SafeGetAttribute("HourlyPay");

            if (decimal.TryParse(stringVar, out decimalVar))
                jobStep.HourlyPay = decimalVar;
            else
                throw new InvalidOperationException(String.Format("Couldn't parse {0} node HourlyPay attribute to decimal.", jobStepNode.Name.LocalName));

            return jobStep;
        }
    }
}