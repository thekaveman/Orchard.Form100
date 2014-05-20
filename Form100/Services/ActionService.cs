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

        public ActionPartViewModel GetViewModel(ActionPart source)
        {
            var target = new ActionPartViewModel();

            target.EffectiveDate = source.EffectiveDate.ToString(FormatProvider.DateFormat);
            target.Category = source.Category == ActionCategory.Undefined ? String.Empty : source.Category.ToString();
            target.Type = source.Type;
            target.Detail = source.Detail;

            target.AllCategories = Enum.GetNames(typeof(ActionCategory)).Except(new[] { "Undefined" });

            return target;
        }

        public void Update(ActionPartViewModel source, ActionPart target)
        {
            DateTime effectiveDate;

            if (DateTime.TryParse(source.EffectiveDate, out effectiveDate))
                target.EffectiveDate = effectiveDate;
            else
                throw new InvalidOperationException("Couldn't parse EffectiveDate in editor template.");

            ActionCategory category;

            if (Enum.TryParse(source.Category, out category))
                target.Category = category;
            else
                target.Category = ActionCategory.Undefined;

            target.Type = source.Type;
            target.Detail = source.Detail;
        }
    }
}