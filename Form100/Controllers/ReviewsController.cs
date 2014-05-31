using System;
using System.Linq;
using System.Web.Mvc;
using CSM.Form100.Models;
using CSM.Form100.Services;
using Orchard.ContentManagement;

namespace CSM.Form100.Controllers
{
    /// <summary>
    /// Controller for working with ReviewParts from the outside.
    /// </summary>
    public class ReviewsController : Controller
    {
        //lazy-load the services only when they are needed
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
                //use contentManger.Value to get the lazy-loaded contentManager
                //get a new draft version of the contentItem by its id
                var contentItem = contentManager.Value.Get(id, VersionOptions.DraftRequired);

                if (contentItem != null && contentItem.Has<ReviewPart>())
                {
                    //this content item is compatible, trigger an update
                    update(contentItem, status);
                }
            }

            return Redirect("~");
        }
                
        private void update(ContentItem contentItem, WorkflowStatus status)
        {
            //get the part we are interested in
            var reviewPart = contentItem.As<ReviewPart>();

            //ensure there are pending reviews
            if (reviewPart.PendingReviews.Any())
            {
                //get the next review
                var nextStep = reviewPart.PendingReviews.RemoveNext();

                //simple input verification
                if (status == nextStep.ApprovingStatus || status == nextStep.RejectingStatus)
                {
                    reviewPart.Status = status;

                    nextStep.ReviewDate = DateTime.Now;
                    nextStep.ReviewDecision = status;
                    //pass nextStep through the reviewService for updating in DB
                    //then add it to the review history
                    reviewPart.ReviewHistory.Add(reviewService.Value.UpdateReviewStep(nextStep));

                    //finally, trigger a publish on the content item
                    contentManager.Value.Publish(contentItem);
                }
            }
        }
    }
}