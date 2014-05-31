using System;
using System.Collections.Generic;
using System.Linq;
using CSM.Form100.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace CSM.Form100.Activities
{
    /// <summary>
    /// A Workflow DecisionActivity that represents the WorkflowStatus states
    /// </summary>
    public class StatusDecisionActivity : Task
    {
        private readonly IOrchardServices orchardServices;
        private readonly IWorkContextAccessor workContextAccessor;

        public StatusDecisionActivity(IOrchardServices orchardServices, IWorkContextAccessor workContextAccessor)
        {
            this.orchardServices = orchardServices;
            this.workContextAccessor = workContextAccessor;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name { get { return "StatusDecision"; } }

        public override LocalizedString Category { get { return T("CSM"); } }

        public override LocalizedString Description { get { return T("Evaluates a ContentItem's current WorkflowStatus."); } }
        
        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return getOutcomes(activityContext).Select(outcome => T(outcome));
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            //some error prevention
            if (workflowContext != null && workflowContext.Content != null && workflowContext.Content.ContentItem != null)
            {
                //assume this content item has a ReviewPart
                var currentItem = workflowContext.Content.ContentItem.As<ReviewPart>();

                if (currentItem != null)
                    //if so, return a LocalizedString of the ReviewPart's status
                    yield return T(currentItem.Status.ToString());
            }
            
            //default to an empty LocalizedString
            yield return T(String.Empty);
        }

        private IEnumerable<string> getOutcomes(ActivityContext context)
        {
            //the outcomes are just the values of the WorkflowStatus enum as strings
            return Enum.GetNames(typeof(WorkflowStatus));
        }
    }
}