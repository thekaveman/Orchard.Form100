//a simple object generator for a reviewStep
function reviewStep(name, email, approvingStatus, rejectingStatus, defaultStatus) {
    return {
        Id: 0,
        ReviewPartIdentifier: "",
        ApprovingStatus: approvingStatus,
        RejectingStatus: rejectingStatus,
        ReviewerName: name,
        ReviewerEmail: email,
        ReviewDate: null,
        ReviewDecision: defaultStatus
    };
}

//a knockout view model representing a review chain
function reviewsViewModel(initChain, initStatuses, defaultStatus) {
    var self = this;

    self.reviews = ko.observableArray(initChain || []);
    self.availableStatuses = ko.observableArray(initStatuses || []);
    self.name = ko.observable("");
    self.email = ko.observable("");
    self.approvingStatus = ko.observable(defaultStatus);
    self.rejectingStatus = ko.observable(defaultStatus);

    self.stepToAdd = ko.computed(function () {
        if (!(self.name() === "" || self.email() === "" || self.approvingStatus() === defaultStatus || self.rejectingStatus() === defaultStatus))
            return reviewStep(self.name(), self.email().toLowerCase(), self.approvingStatus(), self.rejectingStatus());
        else
            return null;
    });

    self.reviewsJSON = ko.computed(function () {
        return JSON.stringify(self.reviews());
    });

    self.addReviewStep = function () {
        var step = self.stepToAdd();
        if (step) {
            self.reviews.push(step);
            self.name("");
            self.email("");
            self.approvingStatus(defaultStatus);
            self.rejectingStatus(defaultStatus);
        }
    };

    self.editReviewStep = function () {
        self.name(this.ReviewerName);
        self.email(this.ReviewerEmail);
        self.approvingStatus(this.ApprovingStatus);
        self.rejectingStatus(this.RejectingStatus);
        self.reviews.remove(this);
    };

    self.removeReviewStep = function () {
        self.reviews.remove(this);
    };

    self.cardinality = function () {
        return self.reviews().indexOf(this) + 1;
    };
}