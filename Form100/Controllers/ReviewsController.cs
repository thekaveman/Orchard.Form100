using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Workflows.Services;
using Orchard.Workflows.Activities;

namespace CSM.Form100.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly string signal = "ReviewStepDone";

        private readonly Lazy<IContentManager> contentManager;
        private readonly Lazy<IReviewService> reviewService;
        private readonly Lazy<IWorkflowManager> workflowManager;

        public ReviewsController(Lazy<IContentManager> contentManager, Lazy<IReviewService> reviewService, Lazy<IWorkflowManager> workflowManager)
        {
            this.contentManager = contentManager;
            this.reviewService = reviewService;
            this.workflowManager = workflowManager;
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
                var nextReview = reviewPart.PendingReviews.Dequeue();

                if (status == nextReview.ApprovingStatus || status == nextReview.RejectingStatus)
                {
                    nextReview.ReviewDate = DateTime.Now;
                    nextReview.ReviewDecision = status;
                    nextReview = reviewService.Value.UpdateReviewStep(nextReview);

                    reviewPart.Status = status;
                    reviewPart.ReviewHistory.Push(nextReview);

                    contentManager.Value.Publish(contentItem);
                }
            }
        }
    }
}