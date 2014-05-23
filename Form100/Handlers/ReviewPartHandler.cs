using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    public class ReviewPartHandler : ContentHandler
    {
        private readonly char separator = ',';

        private readonly IReviewService reviewService;

        public ReviewPartHandler(IRepository<ReviewPartRecord> repository, IReviewService reviewService)
        {
            this.reviewService = reviewService;

            Filters.Add(StorageFilter.For(repository));

            OnInitialized<ReviewPart>((context, part) => initializeApprovalChainField(part));
            OnLoading<ReviewPart>((context, part) => initializeApprovalChainField(part));
            OnUpdating<ReviewPart>((context, part) => initializeApprovalChainField(part));
        }

        private void initializeApprovalChainField(ReviewPart part)
        {
            part.ApprovalChainField.Loader(_ => {
                var approvalChain = new Queue<ReviewDecisionRecord>();

                if (!String.IsNullOrEmpty(part.Record.ApprovalChainIds))
                {
                    var reviews = part.Record.ApprovalChainIds
                                             .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(id => reviewService.GetReviewDecision(int.Parse(id)));

                    approvalChain = new Queue<ReviewDecisionRecord>(reviews);
                }

                return approvalChain;
            });

            part.ApprovalChainField.Setter(approvalChain => {
                StringBuilder approvalChainIds = new StringBuilder();

                if (approvalChain.Any())
                {
                    var copy = approvalChain.Copy();

                    while (copy.Any())
                    {
                        approvalChainIds.AppendFormat("{0}{1}", copy.Dequeue().Id, separator);
                    }
                }

                part.Record.ApprovalChainIds = approvalChainIds.ToString().TrimEnd(separator);

                return approvalChain;
            });
        }
    }
}