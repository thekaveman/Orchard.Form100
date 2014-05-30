using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using Orchard.UI.Notify;

namespace CSM.Form100.Handlers
{
    public class ReviewPartHandler : ContentHandler
    {
        private readonly char separator = ',';

        private readonly IReviewService reviewService;
        private readonly INotifier notifier;

        public ReviewPartHandler(IOrchardServices orchardServices, IRepository<ReviewPartRecord> repository, IReviewService reviewService)
        {
            this.notifier = orchardServices.Notifier;
            this.reviewService = reviewService;

            T = NullLocalizer.Instance;

            Filters.Add(StorageFilter.For(repository));

            OnActivated<ReviewPart>((context, part) => {
                loaders(part, () => notifier.Information(T("OnActivated")));
                setters(part, () => notifier.Information(T("OnActivated")));
            });

            OnUpdating<ReviewPart>((context, part) => {
                updaters(part, () => notifier.Information(T("OnUpdating")));
            });

            //OnPublishing<ReviewPart>((context, part) => {
            //    loaders(part, () => notifier.Information(T("OnPublishing")));
            //    setters(part, () => notifier.Information(T("OnPublishing")))
            //});
        }

        public Localizer T { get; set; }

        private void loaders(ReviewPart part, Action notify)
        {
            part.PendingReviewsField.Loader(incoming => {
                notify();

                var pendingReviews = (incoming != null && incoming.Any())
                                   ? incoming
                                   : new Queue<ReviewStepRecord>();
                
                string ids = part.Record.PendingReviewsIds;

                if (!String.IsNullOrEmpty(ids))
                    pendingReviews = new Queue<ReviewStepRecord>(parseReviewStepIds(ids));

                return pendingReviews;
            });

            part.ReviewHistoryField.Loader(incoming => {
                notify();

                var reviewHistory = (incoming != null && incoming.Any())
                                   ? incoming
                                   : new Stack<ReviewStepRecord>();

                string ids = part.Record.ReviewHistoryIds;

                if (!String.IsNullOrEmpty(ids))
                    reviewHistory = new Stack<ReviewStepRecord>(parseReviewStepIds(ids));

                return reviewHistory;
            });
        }

        private void setters(ReviewPart part, Action notify)
        {
            part.PendingReviewsField.Setter(pendingReviews => {
                notify();
                
                part.Record.PendingReviewsIds = serializeReviewSteps(pendingReviews);

                return pendingReviews;
            });

            part.ReviewHistoryField.Setter(reviewHistory => {
                notify();
                
                part.Record.ReviewHistoryIds = serializeReviewSteps(reviewHistory);

                return reviewHistory;
            });
        }

        private void updaters(ReviewPart part, Action notify)
        {
            notify();

            part.Record.PendingReviewsIds = serializeReviewSteps(part.PendingReviews);
            part.Record.ReviewHistoryIds = serializeReviewSteps(part.ReviewHistory);
        }

        private IEnumerable<ReviewStepRecord> parseReviewStepIds(string ids)
        {
            var reviewSteps = ids.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(id => reviewService.GetReviewStep(int.Parse(id)));

            return reviewSteps;
        }

        private string serializeReviewSteps(IEnumerable<ReviewStepRecord> reviewSteps)
        {
            StringBuilder reviewStepIds = new StringBuilder();

            foreach (var step in reviewSteps)
            {
                reviewStepIds.AppendFormat("{0}{1}", step.Id, separator);
            }

            return reviewStepIds.ToString().TrimEnd(separator);
        }
    }
}