using System;
using System.Linq;
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

        public ActionPartViewModel GetViewModel(ActionPart part)
        {
            var target = new ActionPartViewModel();

            target.EffectiveDate = part.EffectiveDate.Equals(DateTime.MinValue) ? String.Empty : part.EffectiveDate.ToString(FormatProvider.DateFormat);
            target.Category = part.Category;
            target.Type = part.Type;
            target.Detail = part.Detail;

            return target;
        }

        public void Update(ActionPartViewModel viewModel, ActionPart part)
        {
            DateTime effectiveDate;

            if (DateTime.TryParse(viewModel.EffectiveDate, out effectiveDate))
                part.EffectiveDate = effectiveDate;
            else
                throw new InvalidOperationException("Couldn't parse EffectiveDate in editor template.");

            part.Category = viewModel.Category;
            part.Type = viewModel.Type;
            part.Detail = viewModel.Detail;
        }
    }
}