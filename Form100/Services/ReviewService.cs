using System.Collections.Generic;
using CSM.Form100.Models;
using CSM.Form100.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;

namespace CSM.Form100.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<ReviewStepRecord> reviewStepRepository;

        public ReviewService(IRepository<ReviewStepRecord> reviewStepRepository)
        {
            this.reviewStepRepository = reviewStepRepository;
        }
        
        public ReviewPartViewModel GetReviewViewModel(ReviewPart part)
        {
            var viewModel = new ReviewPartViewModel() {
                Status = part.Status,
                PendingReviewsData = serializeReviewSteps(part.PendingReviews),
                ReviewHistoryData = serializeReviewSteps(part.ReviewHistory)
            };

            return viewModel;
        }
        
        public void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part)
        {
            var identityPart = part.As<IdentityPart>();
            string identity = identityPart.Identifier ?? part.Id.ToString();

            part.Status = viewModel.Status;                        
            part.PendingReviews = deserializePendingReviews(viewModel.PendingReviewsData, identity);
            part.ReviewHistory = deserializeReviewHistory(viewModel.ReviewHistoryData, identity);
        }
        
        public ReviewStepRecord GetReviewStep(int id)
        {
            var reviewStep = reviewStepRepository.Get(id);
            return reviewStep;
        }

        public ReviewStepRecord CreateReviewStep(ReviewStepRecord reviewStep)
        {
            reviewStepRepository.Create(reviewStep);
            return reviewStep;
        }
        
        public ReviewStepRecord UpdateReviewStep(ReviewStepRecord reviewStep)
        {
            reviewStepRepository.Update(reviewStep);
            return reviewStep;
        }

        internal IEnumerable<ReviewStepRecord> deserializeReviewSteps(string reviewStepData, string reviewPartId)
        {
            var reviewSteps = JsonConvert.DeserializeObject<IEnumerable<ReviewStepRecord>>(reviewStepData);
            List<ReviewStepRecord> processedReviewSteps = new List<ReviewStepRecord>();

            foreach (var reviewStep in reviewSteps)
            {
                reviewStep.ReviewPartIdentifier = reviewPartId;

                if (reviewStep.Id > 0)
                    processedReviewSteps.Add(UpdateReviewStep(reviewStep));
                else
                    processedReviewSteps.Add(CreateReviewStep(reviewStep));
            }

            return processedReviewSteps;
        }

        internal Queue<ReviewStepRecord> deserializePendingReviews(string pendingReviewData, string reviewPartId)
        {
            var pendingReviews = deserializeReviewSteps(pendingReviewData ?? "[]", reviewPartId);
            return new Queue<ReviewStepRecord>(pendingReviews);
        }

        internal Stack<ReviewStepRecord> deserializeReviewHistory(string reviewHistoryData, string reviewPartId)
        {
            var reviewHistory = deserializeReviewSteps(reviewHistoryData ?? "[]", reviewPartId);
            return new Stack<ReviewStepRecord>(reviewHistory);
        }

        internal string serializeReviewSteps(IEnumerable<ReviewStepRecord> reviewSteps)
        {
            reviewSteps = reviewSteps ?? new List<ReviewStepRecord>();
            return JsonConvert.SerializeObject(reviewSteps, new StringEnumConverter());
        }
    }
}