using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    public interface IActionService : IDependency
    {
        ActionPartViewModel GetViewModel(ActionPart source);

        void Update(ActionPartViewModel source, ActionPart target);
    }
}
