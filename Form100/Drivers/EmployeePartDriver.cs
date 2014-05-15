using System.Xml.Linq;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace CSM.Form100.Drivers
{
    using Models;

    public class EmployeePartDriver : ContentPartDriver<EmployeePart>
    {
        private readonly string dateFormat = "yyyy-dd-MM";

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
            return Combined(
                ContentShape(
                    "Parts_Employee_EmployeeId",
                    () => shapeHelper.Parts_Employee_EmployeeId(EmployeeId: part.EmployeeId)
                ),
                ContentShape(
                    "Parts_Employee_Name",
                    () => shapeHelper.Parts_Employee_Name(FirstName: part.FirstName, LastName: part.LastName)
                ),
                ContentShape(
                    "Parts_Employee_PriorJobStep",
                    () => shapeHelper.Parts_Employee_PriorJobStep(JobStep: part.PriorJobStep)
                ),
                ContentShape(
                    "Parts_Employee_CurrentJobStep",
                    () => shapeHelper.Parts_Employee_CurrentJobStep(JobStep: part.CurrentJobStep)
                )                
            );
        }

        /// <summary>
        /// Define how EmployeePart data is exported.
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

        private static XElement createJobStepNode(string name, JobStepRecord jobStep)
        {
            var jobStepNode = new XElement(name);
            
            jobStepNode.SetAttributeValue("Id", jobStep.Id);
            jobStepNode.SetAttributeValue("Title", jobStep.Title);
            jobStepNode.SetAttributeValue("DepartmentName", jobStep.DepartmentName);
            jobStepNode.SetAttributeValue("DivisionName", jobStep.DivisionName);
            jobStepNode.SetAttributeValue("DivisionNumber", jobStep.DivisionNumber);
            jobStepNode.SetAttributeValue("StepNumber", jobStep.StepNumber);
            jobStepNode.SetAttributeValue("Id", jobStep.Id);
            jobStepNode.SetAttributeValue("HoursPerWeek", jobStep.HoursPerWeek);

            return jobStepNode;
        }
    }
}