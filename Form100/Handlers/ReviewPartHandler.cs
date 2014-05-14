using System.Collections.Generic;
using System.Linq;

using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CSM.Form100.Handlers
{
    using Models;
    using Services;

    public class ReviewPartHandler : ContentHandler
    {
        public ReviewPartHandler(IRepository<ReviewPartRecord> repository, IReviewApprovalService reviewApprovalService)
        {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ReviewPart>((context, reviewPart) => {

                reviewPart.ApprovalsField.Loader(_ => {
                    return reviewPart.Record.ApprovalIds.Any()
                         ? reviewPart.Record.ApprovalIds.Select(id => reviewApprovalService.Get(id)).ToList()
                         : new List<ReviewApprovalRecord>();
                });

                reviewPart.ApprovalsField.Setter(approvals => {
                    reviewPart.Record.ApprovalIds = approvals != null && approvals.Any()
                        ? approvals.Select(a => a.Id).ToList()
                        : new List<int>();
                    return approvals;
                });
            });
        }
    }
}