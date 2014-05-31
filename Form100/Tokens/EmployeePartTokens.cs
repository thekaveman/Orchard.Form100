using System;
using CSM.Form100.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;

namespace CSM.Form100.Tokens
{
    /// <summary>
    /// Token provider for EmployeePart data.
    /// </summary>
    public class EmployeePartTokens : ITokenProvider
    {
        public EmployeePartTokens()
        {            
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context)
        {
            //provide 1 token:
            context.For("Employee", T("Employee"), T("Tokens for the EmployeePart."))
                    //{Employee.Name}
                    .Token("Name", T("Name"), T("The name of a Form100's employee."));
        }

        public void Evaluate(EvaluateContext context)
        {
            //try to get the current content from context
            IContent content = context.Data.ContainsKey("Content") ? context.Data["Content"] as IContent : null;

            //try to get an EmployeePart from the current content
            EmployeePart part = content == null ? default(EmployeePart) : content.As<EmployeePart>();

            //provide the current EmployeePart, if any, as context data
            context.For<EmployeePart>("Employee", part)
                    //evaluator function for {Employee.Name}
                    .Token("Name", employeePart =>
                    {
                        if (employeePart != null)
                            return String.Format("{0} {1}", employeePart.FirstName, employeePart.LastName);

                        return String.Empty;
                    });
        }
    }
}