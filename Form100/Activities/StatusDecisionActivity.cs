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

        public override LocalizedString Description { get { return T("Evaluates an IWorkFlowParticipant's current status."); } }

        //public override string Form { get { return "StatusDecisionActivityForm"; } }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return getOutcomes(activityContext).Select(outcome => T(outcome));
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            if (workflowContext != null && workflowContext.Content != null && workflowContext.Content.ContentItem != null)
            {
                var currentItem = workflowContext.Content.ContentItem.As<ReviewPart>();

                if (currentItem != null)
                    yield return T(currentItem.Status.ToString());
            }
            
            yield return T(String.Empty);
        }

        private IEnumerable<string> getOutcomes(ActivityContext context)
        {
            return Enum.GetNames(typeof(WorkflowStatus));
        }
    }
}