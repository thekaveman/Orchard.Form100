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
                loaders(part, () => notifier.Information(T("OnUpdating")));
                setters(part, () => notifier.Information(T("OnUpdating")));
            });

            OnPublishing<ReviewPart>((context, part) => {
                loaders(part, () => notifier.Information(T("OnPublishing")));
                setters(part, () => notifier.Information(T("OnPublishing")))
            });
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

                StringBuilder pendingReviewIds = new StringBuilder();

                if (pendingReviews.Any())
                {
                    var copy = pendingReviews.Copy();

                    while (copy.Any())
                    {
                        pendingReviewIds.AppendFormat("{0}{1}", copy.Dequeue().Id, separator);
                    }
                }

                part.Record.PendingReviewsIds = pendingReviewIds.ToString().TrimEnd(separator);

                return pendingReviews;
            });

            part.ReviewHistoryField.Setter(reviewHistory => {
                notify();

                StringBuilder reviewHistoryIds = new StringBuilder();

                if (reviewHistory.Any())
                {
                    var copy = reviewHistory.Copy();

                    while (copy.Any())
                    {
                        reviewHistoryIds.AppendFormat("{0}{1}", copy.Pop().Id, separator);
                    }
                }

                part.Record.ReviewHistoryIds = reviewHistoryIds.ToString().TrimEnd(separator);

                return reviewHistory;
            });
        }

        private IEnumerable<ReviewStepRecord> parseReviewStepIds(string ids)
        {
            var reviewSteps = ids.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(id => reviewService.GetReviewStep(int.Parse(id)));

            return reviewSteps;
        }
    }
}