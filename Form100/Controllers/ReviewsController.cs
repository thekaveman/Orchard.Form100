using System;
using System.Linq;
using System.Web.Mvc;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement;

namespace CSM.Form100.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly Lazy<IContentManager> contentManager;
        private readonly Lazy<IReviewService> reviewService;

        public ReviewsController(Lazy<IContentManager> contentManager, Lazy<IReviewService> reviewService)
        {
            this.contentManager = contentManager;
            this.reviewService = reviewService;
        }

        public ActionResult Index(int id = -1)
        {
            if (id > 0)
            {
                var review = contentManager.Value.Get<ReviewPart>(id);
            }

            return Redirect("~");
        }

        public ActionResult Update(int id, WorkflowStatus status)
        {
            if (id > 0)
            {
                var contentItem = contentManager.Value.Get(id, VersionOptions.DraftRequired);

                if (contentItem != null && contentItem.Has<ReviewPart>())
                {
                    update(contentItem, status);
                }
            }

            return Redirect("~");
        }
                
        private void update(ContentItem contentItem, WorkflowStatus status)
        {
            var reviewPart = contentItem.As<ReviewPart>();

            if (reviewPart.PendingReviews.Any())
            {
                var nextStep = reviewPart.PendingReviews.RemoveNext();

                if (status == nextStep.ApprovingStatus || status == nextStep.RejectingStatus)
                {
                    reviewPart.Status = status;

                    nextStep.ReviewDate = DateTime.Now;
                    nextStep.ReviewDecision = status;
                    nextStep = reviewService.Value.UpdateReviewStep(nextStep);

                    reviewPart.ReviewHistory.Add(nextStep);

                    contentManager.Value.Publish(contentItem);
                }
            }
        }
    }
}