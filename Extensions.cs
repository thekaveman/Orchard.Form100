using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CSM.Form100.Models;
using CSM.Form100.Services;

namespace CSM.Form100
{
    public static class XElementExtensions
    {
        /// <summary>
        /// Helper method to safely get an attribute value.
        /// </summary>
        /// <param name="node">An XElement node to read an attribute value from</param>
        /// <param name="attributeName">The attribute to read</param>
        /// <returns>The attribute's value if it exists, or the empty string</returns>
        public static string SafeGetAttribute(this XElement node, string attributeName)
        {
            try
            {
                return node.Attribute(attributeName).Value;
            }
            catch
            {
                return String.Empty;
            }
        }
    }

    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Wraps the Any() LINQ method in a null-check.
        /// </summary>
        public static bool SafeAny<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        /// <summary>
        /// Wraps the Any() LINQ method in a null-check.
        /// </summary>
        public static bool SafeAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable != null && enumerable.Any(predicate);
        }
    }

    internal static class ReviewExtensions
    {
        /// <summary>
        /// Get the next review step for the given state.
        /// </summary>
        /// <returns>The next ReviewStepRecord where review is required, or null.</returns>
        public static ReviewStepRecord RemoveNextReviewStep(this ReviewPart part, IReviewService reviewService, WorkflowStatus forState)
        {
            ReviewStepRecord nextStep = null;

            if (part.ReviewHistory.SafeAny())
            {
                var lastReview = part.ReviewHistory.PeekNext();

                if (lastReview.CanTarget(part.State) && lastReview.ApprovesOrRejects(forState))
                    //want to leave the history intact, return a new version of the last review
                    nextStep = reviewService.CreateReviewStep(new ReviewStepRecord(lastReview));
            }

            if (nextStep == null && part.PendingReviews.SafeAny())
            {
                var nextPending = part.PendingReviews.PeekNext();

                if (nextPending.CanTarget(part.State) && nextPending.ApprovesOrRejects(forState))
                {
                    nextStep = part.PendingReviews.RemoveNext(); 
                }
            }

            return nextStep;
        }

        public static ReviewStepRecord PeekNextReviewStep(this ReviewPart part, IReviewService reviewService)
        {
            ReviewStepRecord nextStep = null;

            if (part.ReviewHistory.SafeAny())
            {
                var lastReview = part.ReviewHistory.PeekNext();

                if (lastReview.CanTarget(part.State))
                    nextStep = lastReview;
            }

            if (nextStep == null && part.PendingReviews.SafeAny())
            {
                var nextPending = part.PendingReviews.PeekNext();

                if (nextPending.CanTarget(part.State))
                    nextStep = nextPending;
            }

            return nextStep;
        }

        /// <summary>
        /// Get a collection of WorkflowStatuses that the reviewStep is allowed to review.
        /// </summary>
        public static IEnumerable<WorkflowStatus> TargetStates(this ReviewStepRecord reviewStep)
        {
            if (!(reviewStep == null || String.IsNullOrEmpty(reviewStep.TargetStates)))
            {
                var split = reviewStep.TargetStates.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in split)
                {
                    WorkflowStatus state;
                    if (Enum.TryParse(s, out state)) yield return state;
                }
            }
        }

        /// <summary>
        /// Determine if the reviewStep is allowed to review the given WorkflowStatus
        /// </summary>
        public static bool CanTarget(this ReviewStepRecord reviewStep, WorkflowStatus target)
        {
            return reviewStep.TargetStates().Any(s => target == s);
        }

        /// <summary>
        /// Determine if the reviewStep Accepts or Rejects the given WorkflowStatus
        /// </summary>
        public static bool ApprovesOrRejects(this ReviewStepRecord reviewStep, WorkflowStatus state)
        {
            return reviewStep != null && (reviewStep.ApprovingState == state || reviewStep.RejectingState == state);
        }
    }
}