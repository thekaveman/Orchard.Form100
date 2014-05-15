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

        public ReviewPartHandler(IRepository<ReviewPartRecord> repository, IReviewApprovalService reviewApprovalService)
        {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ReviewPart>((context, reviewPart) => {

                reviewPart.ApprovalChainField.Loader(_ => {
                    var approvalChain = new Queue<ReviewApprovalRecord>();

                    if (!String.IsNullOrEmpty(reviewPart.Record.ApprovalChainIds))
                    {
                        var approvals = reviewPart.Record.ApprovalChainIds
                                                         .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                                         .Select(id => reviewApprovalService.Get(int.Parse(id)));

                        approvalChain = new Queue<ReviewApprovalRecord>();
                    }

                    return approvalChain;
                });

                reviewPart.ApprovalChainField.Setter(approvalChain => {
                    StringBuilder approvalChainIds = new StringBuilder();

                    if (approvalChain.Any())
                    {
                        var copy = new Queue<ReviewApprovalRecord>(approvalChain);

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