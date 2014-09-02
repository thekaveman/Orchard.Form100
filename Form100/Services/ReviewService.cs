using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly string separator = ",";

        private readonly IRepository<ReviewStepRecord> reviewStepRepository;

        public ReviewService(IRepository<ReviewStepRecord> reviewStepRepository)
        {
            this.reviewStepRepository = reviewStepRepository;
        }
        
        public ReviewPartViewModel GetReviewViewModel(ReviewPart part)
        {
            var viewModel = new ReviewPartViewModel() {
                State = part.State,
                PendingReviewsData = getReviewStepsJSON(part.PendingReviews),
                ReviewHistoryData = getReviewStepsJSON(part.ReviewHistory)
            };

            return viewModel;
        }
        
        public void UpdateReview(ReviewPartViewModel viewModel, ReviewPart part)
        {
            var identityPart = part.As<IdentityPart>();
            string identity = identityPart.Identifier ?? part.Id.ToString();

            part.State = viewModel.State;
            part.Record.PendingReviewsIds = getReviewStepIds(viewModel.PendingReviewsData, identity);
            part.Record.ReviewHistoryIds = getReviewStepIds(viewModel.ReviewHistoryData, identity);
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

        public string SerializeReviewSteps<T>(IEnumerable<ReviewStepRecord> reviewSteps, Func<ReviewStepRecord, T> propertySelector, bool distinct = false)
        {
            if (propertySelector == null) throw new ArgumentNullException("propertySelector");

            if (reviewSteps != null && reviewSteps.Any())
            {
                var projectedValues = reviewSteps.Select(propertySelector);

                if (distinct)
                    projectedValues = projectedValues.Distinct();

                return String.Join(separator, projectedValues);
            }

            return String.Empty;
        }

        public IEnumerable<T> DeserializeReviewSteps<T>(string reviewStepIds, Func<ReviewStepRecord, T> propertySelector)
        {
            if(propertySelector == null) throw new ArgumentNullException("propertySelector");

            var reviewSteps = (reviewStepIds ?? "")
                    .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => GetReviewStep(int.Parse(id)))
            ;

            return reviewSteps.Select(propertySelector);
        }

        internal IEnumerable<ReviewStepRecord> getReviewSteps(string reviewStepJSON, string reviewPartId)
        {
            var reviewSteps = JsonConvert.DeserializeObject<IEnumerable<ReviewStepRecord>>(reviewStepJSON);
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

        internal string getReviewStepsJSON(IEnumerable<ReviewStepRecord> reviewSteps)
        {
            reviewSteps = reviewSteps ?? new List<ReviewStepRecord>();
            return JsonConvert.SerializeObject(reviewSteps, new StringEnumConverter());
        }

        internal string getReviewStepIds(string reviewStepJSON, string reviewPartId)
        {
            var reviewSteps = getReviewSteps(reviewStepJSON ?? "[]", reviewPartId);
            return SerializeReviewSteps(reviewSteps, r => r.Id);
        }
    }
}