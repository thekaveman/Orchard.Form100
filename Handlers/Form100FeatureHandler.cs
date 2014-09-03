using System;
using System.Linq;
using Orchard.Environment;
using Orchard.Environment.Extensions.Models;
using Orchard.Recipes.Services;

namespace CSM.Form100.Handlers
{
    /// <summary>
    /// A class to handle lifecycle events on the base feature of the CSM.Form100 module.
    /// </summary>
    /// <remarks>
    /// <para>A module may contain more than one feature, and a FeatureEventHandler could be set up for each.</para>
    /// <para>All modules contain at least one feature, the root/self-titled feature.</para>
    /// </remarks>
    public class Form100FeatureEventHandler : IFeatureEventHandler
    {
        IRecipeHarvester recipeHarvester;
        IRecipeManager recipeManager;

        private readonly string recipeName = "CSM.Form100";

        public Form100FeatureEventHandler(IRecipeHarvester recipeHarvester, IRecipeManager recipeManager)
        {
            this.recipeHarvester = recipeHarvester;
            this.recipeManager = recipeManager;
        }

        /// <summary>
        /// Handler for when the feature is enabled.
        /// </summary>
        public void Enabled(Feature feature)
        {
            //we only handle one specific feature here
            if (feature.Descriptor.Name != recipeName)
                return;

            //collect the recipes for this feature
            var recipes = recipeHarvester.HarvestRecipes(recipeName);

            if (recipes.Any())
            {
                //there should be just one
                var recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
                //if so, execute it
                if (recipe != null)
                    recipeManager.Execute(recipe);
            }
        }

        #region no implementation required

        public void Disabled(Feature feature)
        {

        }
        public void Disabling(Feature feature)
        {

        }
        public void Enabling(Feature feature)
        {

        }
        public void Installed(Feature feature)
        {

        }
        public void Installing(Feature feature)
        {

        }
        public void Uninstalled(Feature feature)
        {

        }
        public void Uninstalling(Feature feature)
        {

        }

        #endregion
    }
}