using System.Collections.Generic;
using System.Linq;
using System;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    using Models;
    using Services;
    using System.Text;

    public class ReviewPartHandler : ContentHandler
    {
        private readonly char separator = ',';

        public ReviewPartHandler(IRepository<ReviewPartRecord> repository, IReviewService reviewService)
        {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ReviewPart>((context, reviewPart) => {

                reviewPart.ApprovalChainField.Loader(_ => {
                    var approvalChain = new Queue<ReviewDecisionRecord>();

                    if (!String.IsNullOrEmpty(reviewPart.Record.ApprovalChainIds))
                    {
                        var reviews = reviewPart.Record.ApprovalChainIds
                                                       .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Select(id => reviewService.Get(int.Parse(id)));

                        approvalChain = new Queue<ReviewDecisionRecord>(reviews);
                    }

                    return approvalChain;
                });

                reviewPart.ApprovalChainField.Setter(approvalChain => {
                    StringBuilder approvalChainIds = new StringBuilder();

                    if (approvalChain.Any())
                    {
                        var copy = new Queue<ReviewDecisionRecord>(approvalChain);

                        while (copy.Any())
                        {
                            approvalChainIds.AppendFormat("{0}{1}", copy.Dequeue().Id, separator);
                        }
                    }

                    reviewPart.Record.ApprovalChainIds = approvalChainIds.ToString().TrimEnd(separator);

                    return approvalChain;
                });
            });
        }
    }
}