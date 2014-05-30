using System;
using CSM.Form100.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;

namespace CSM.Form100.Tokens
{
    public class EmployeePartTokens : ITokenProvider
    {
        public EmployeePartTokens()
        {            
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context)
        {
            context.For("Employee", T("Employee"), T("Tokens for the EmployeePart."))
                    .Token("Name", T("Name"), T("The name of a Form100's employee."));
        }

        public void Evaluate(EvaluateContext context)
        {
            IContent content = context.Data["Content"] as IContent;

            EmployeePart part = content == null ? default(EmployeePart) : content.As<EmployeePart>();

            context.For<EmployeePart>("Employee", part)
                    .Token("Name", employeePart =>
                    {
                        if (employeePart != null)
                            return String.Format("{0} {1}", employeePart.FirstName, employeePart.LastName);

                        return String.Empty;
                    });
        }
    }
}