using System;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    public class ReviewPartHandler : ContentHandler
    {
        private readonly IReviewService reviewService;

        public ReviewPartHandler(IRepository<ReviewPartRecord> repository, IReviewService reviewService)
        {
            this.reviewService = reviewService;
            
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ReviewPart>((context, part) => {
                setupLoaders(part);
            });
        }
        
        private void setupLoaders(ReviewPart part)
        {
            part.PendingReviewsField.Loader(_ => {
                string ids = part.Record.PendingReviewsIds;

                if (!String.IsNullOrEmpty(ids))
                    return new ReviewChain(CollectionOrder.FIFO, part.Record, reviewService, reviewService.DeserializeReviewStepIds(ids));

                return new ReviewChain(CollectionOrder.FIFO, part.Record, reviewService);
            });

            part.ReviewHistoryField.Loader(_ => {
                string ids = part.Record.ReviewHistoryIds;

                if (!String.IsNullOrEmpty(ids))
                    return new ReviewChain(CollectionOrder.LIFO, part.Record, reviewService, reviewService.DeserializeReviewStepIds(ids));

                return new ReviewChain(CollectionOrder.LIFO, part.Record, reviewService);
            });
        }
    }
}