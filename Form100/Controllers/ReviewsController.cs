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
        private readonly IContentManager contentManager;
        private readonly IReviewService reviewService;

        public ReviewsController(IContentManager contentManager, IReviewService reviewService)
        {
            this.contentManager = contentManager;
            this.reviewService = reviewService;
        }

        public ActionResult Index(int id = -1)
        {
            if (id > 0)
            {
                var review = contentManager.Get<ReviewPart>(id);
            }

            return Redirect("~");
        }

        //[HttpPost]
        public ActionResult SetNotifiedDate(int id)
        {
            var reviewStep = reviewService.GetReviewStep(id);

            if (reviewStep != null)
            {
                reviewStep.NotificationDate = DateTime.Now;
                reviewService.UpdateReviewStep(reviewStep);

                return Redirect("~");
            }

            return HttpNotFound(String.Format("ReviewStep with id {0} was not found.", id));
        }

        public ActionResult CompleteReviewStep(int id, WorkflowStatus state)
        {
            //use contentManger.Value to get the lazy-loaded contentManager
            //get a new draft version of the contentItem by its id
            var contentItem = contentManager.Get(id, VersionOptions.DraftRequired);

            if (contentItem != null && contentItem.Has<ReviewPart>())
            {
                //this content item is compatible, trigger an update
                complete(contentItem, state);
            }

            return Redirect("~");
        }
                
        private void complete(ContentItem contentItem, WorkflowStatus state)
        {
            //get the part we are interested in
            var reviewPart = contentItem.As<ReviewPart>();

            //get the next review step that can act on this state
            var nextStep = reviewPart.RemoveNextReviewStep(reviewService, state);

            if (nextStep != null)
            {
                reviewPart.State = state;

                nextStep.ReviewDate = DateTime.Now;
                nextStep.ReviewDecision = state;
                //pass nextStep through the reviewService for updating in DB
                //then add it to the review history
                reviewPart.ReviewHistory.Add(reviewService.UpdateReviewStep(nextStep));

                //finally, trigger a publish on the content item
                contentManager.Publish(contentItem);
            }
        }
    }
}