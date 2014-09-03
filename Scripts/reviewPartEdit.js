//a simple object generator for a reviewStep
function reviewStep(name, email, targetStates, approvingState, rejectingState, defaultState) {
    return {
        Id: 0,
        ReviewPartIdentifier: "",
        TargetStates: validReviewStates,
        ApprovingState: approvingState,
        RejectingState: rejectingState,
        ReviewerName: name,
        ReviewerEmail: email,
        ReviewDate: null,
        ReviewDecision: defaultState
    };
}

//a knockout view model representing a review chain
function reviewsViewModel(initChain, initStates, defaultState) {
    var self = this;

    self.reviews = ko.observableArray(initChain || []);
    self.availableStates = ko.observableArray(initStates || []);
    self.name = ko.observable("");
    self.email = ko.observable("");
    self.targetStates = ko.observableArray([]);
    self.approvingState = ko.observable(defaultState);
    self.rejectingState = ko.observable(defaultState);

    self.stepToAdd = ko.computed(function () {
        if (!(self.name() === "" || self.email() === "" || self.targetStates() === [] || self.approvingState() === defaultState || self.rejectingState() === defaultState))
            return reviewStep(self.name(), self.email().toLowerCase(), self.targetStates().join(","), self.approvingState(), self.rejectingState());
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
            self.targetStates([]);
            self.approvingState(defaultState);
            self.rejectingState(defaultState);
        }
    };

    self.editReviewStep = function () {
        self.name(this.ReviewerName);
        self.email(this.ReviewerEmail);
        self.targetStates(this.TargetStates.split(","));
        self.approvingState(this.ApprovingState);
        self.rejectingState(this.RejectingState);
        self.reviews.remove(this);
    };

    self.removeReviewStep = function () {
        self.reviews.remove(this);
    };

    self.cardinality = function () {
        return self.reviews().indexOf(this) + 1;
    };
}