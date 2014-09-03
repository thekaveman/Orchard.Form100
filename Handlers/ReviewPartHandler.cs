using System.Collections.Generic;
using CSM.Form100.Collections;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    /// <summary>
    /// ContentHandlers can hook into events happening on ContentParts
    /// </summary>
    public class ReviewPartHandler : ContentHandler
    {
        private readonly IReviewService reviewService;

        public ReviewPartHandler(IRepository<ReviewPartRecord> repository, IReviewService reviewService)
        {
            this.reviewService = reviewService;

            //adds a storage mechanism for EmployeePartRecords into the content pipeline
            Filters.Add(StorageFilter.For(repository));

            //sets up the ReviewPart's lazy field loaders when the part is activated
            //i.e. after all its field are populated, just before returning to the caller
            OnActivated<ReviewPart>((context, part) => {
                setupLoaders(part);
            });
        }
        
        private void setupLoaders(ReviewPart part)
        {
            part.PendingReviewsField.Loader(_ => {
                //get any existing data from the record
                string ids = part.Record.PendingReviewsIds;

                //deserialize if posible into a collection of ReviewStepRecords
                IEnumerable<ReviewStepRecord> reviewSteps = reviewService.DeserializeReviewSteps(ids, r => r);

                //create the appropriate ReviewChain ordering for pending reviews
                return new ReviewChain(CollectionOrder.FIFO, part.Record, reviewService, reviewSteps);
            });

            part.ReviewHistoryField.Loader(_ => {
                //get any existing data from the record
                string ids = part.Record.ReviewHistoryIds;

                //deserialize if posible into a collection of ReviewStepRecords
                var reviewSteps = reviewService.DeserializeReviewSteps(ids, r => r);

                //create the appropriate ReviewChain ordering for review history
                return new ReviewChain(CollectionOrder.LIFO, part.Record, reviewService, reviewSteps);
            });
        }
    }
}