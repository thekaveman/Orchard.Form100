using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;

namespace CSM.Form100.Services
{
    /// <summary>
    /// An injectable service for interacting with ActionPart data.
    /// </summary>
    public interface IActionService : IDependency
    {
        /// <summary>
        /// Convert an ActionPart to its corresponding view model representation.
        /// </summary>
        /// <param name="part">The ActionPart to convert.</param>
        /// <returns>An ActionPartViewModel object representing the given ActionPart.</returns>
        ActionPartViewModel GetViewModel(ActionPart source);

        /// <summary>
        /// Update an ActionPart with data from a view model.
        /// </summary>
        /// <param name="viewModel">The ActionPartViewModel object containg updated data.</param>
        /// <param name="part">The target ActionPart to update.</param>
        void UpdateAction(ActionPartViewModel viewModel, ActionPart part);
    }
}
