using System;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard.Data;

namespace CSM.Form100.Services
{
    public class ActionService : IActionService
    {
        private readonly IRepository<ActionPartRecord> actionRepository;

        public ActionService(IRepository<ActionPartRecord> actionRepository)
        {
            this.actionRepository = actionRepository;
        }

        public ActionPartViewModel GetViewModel(ActionPart source)
        {
            var target = new ActionPartViewModel();

            target.EffectiveDate = source.EffectiveDate.HasValue ? source.EffectiveDate.Value : DateTime.Now;
            target.Category = source.Category;
            target.Type = source.Type;
            target.Detail = source.Detail;

            return target;
        }

        public void Update(ActionPartViewModel source, ActionPart target)
        {
            target.EffectiveDate = source.EffectiveDate;
            target.Category = source.Category;
            target.Type = source.Type;
            target.Detail = source.Detail;
        }
    }
}