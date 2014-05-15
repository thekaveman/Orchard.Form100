using Orchard.ContentManagement.Drivers;

namespace CSM.Form100.Drivers
{
    using Models;

    public class EmployeePartDriver : ContentPartDriver<EmployeePart>
    {
        protected override string Prefix
        {
            get { return "Employee"; }
        }

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
                    "Parts_Employee_PriorJobClass",
                    () => shapeHelper.Parts_Employee_PriorJobClass(JobClass: part.PriorJobClass)
                ),
                ContentShape(
                    "Parts_Employee_CurrentJobClass",
                    () => shapeHelper.Parts_Employee_CurrentJobClass(JobClass: part.CurrentJobClass)
                )                
            );
        }
    }
}