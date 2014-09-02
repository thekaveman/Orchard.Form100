using System.Collections.Generic;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Orchard;
using System;

namespace CSM.Form100.Services
{
    /// <summary>
    /// An injectable service for interacting with ReviewPart data.
    /// </summary>
    public interface IReviewService : IDependency
    {
        /// <summary>
        /// Convert a ReviewPart to its corresponding view model representation.
        /// </summary>
        /// <param name="part">The ReviewPart to convert.</param>
        /// <returns>A ReviewPartViewModel object representing the given ReviewPart.</returns>
        ReviewPartViewModel GetReviewViewModel(ReviewPart part);
        
        /// <summary>
        /// Update a ReviewPart with data from a view model.
        /// </summary>
        /// <param name="viewModel">The ReviewPartViewModel object containg updated data.</param>
        /// <param name="part">The target ReviewPart to update.</param>
        void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part);
        
        /// <summary>
        /// Get a ReviewStepRecord by id.
        /// </summary>
        ReviewStepRecord GetReviewStep(int id);
                        
        /// <summary>
        /// Create a new ReviewStepRecord in the DB.
        /// </summary>
        /// <param name="step">The ReviewStepRecord containing the data to be persisted.</param>
        /// <returns>The newly persisted ReviewStepRecord.</returns>
        ReviewStepRecord CreateReviewStep(ReviewStepRecord step);

        /// <summary>
        /// Update an existing ReviewStepRecord in the DB.
        /// </summary>
        /// <param name="step">The ReviewStepRecord containing the updated data.</param>
        /// <returns>The newly updated ReviewStepRecord.</returns>
        ReviewStepRecord UpdateReviewStep(ReviewStepRecord step);

        /// <summary>
        /// Serialize a collection of ReviewStepRecords.
        /// </summary>
        /// <param name="reviewSteps">The ReviewStepRecords to serialize</param>
        /// <param name="propertySelector">A function to select a property from each ReviewStepRecord</param>
        /// <param name="distinct">True to filter out duplicate results.</param>
        /// <returns>A comma-separated list of the selected ReviewStepRecord property</returns>
        string SerializeReviewSteps<T>(IEnumerable<ReviewStepRecord> reviewSteps, Func<ReviewStepRecord, T> propertySelector, bool distinct = false);

        /// <summary>
        /// Deserialize the ids of a sequence of ReviewStepRecords.
        /// </summary>
        /// <param name="reviewStepIds">A comma-separated list of ReviewStepRecord ids</param>
        /// /// <param name="propertySelector">A function to select a property from each ReviewStepRecord</param>
        /// <returns>A collection of T, the type projected by the propertySelector.</returns>
        IEnumerable<T> DeserializeReviewSteps<T>(string reviewStepIds, Func<ReviewStepRecord, T> propertySelector);
    }
}
