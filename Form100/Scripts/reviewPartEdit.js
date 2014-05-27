function reviewer(name, email, status) {
    return {
        Id: 0,
        ReviewPartIdentifier: "",
        TargetStatus: status,
        ReviewerName: name,
        ReviewerEmail: email,
        ReviewDate: null
    };
}

function approvalChainViewModel(initChain, initStatuses) {
    var self = this;

    self.approvalChain = ko.observableArray(initChain || []);
    self.availableStatuses = ko.observableArray(initStatuses || []);
    self.name = ko.observable("");
    self.email = ko.observable("");
    self.targetStatus = ko.observable("Undefined");

    self.reviewerToAdd = ko.computed(function () {
        if (!(self.name() === "" || self.email() === "" || self.targetStatus() === "Undefined"))
            return reviewer(self.name(), self.email().toLowerCase(), self.targetStatus());
        else
            return null;
    });

    self.approvalChainJSON = ko.computed(function () {
        return JSON.stringify(self.approvalChain());
    });

    self.addReviewer = function () {
        var reviewer = self.reviewerToAdd();
        if (reviewer) {
            self.approvalChain.push(reviewer);
            self.name("");
            self.email("");
            self.targetStatus("Undefined");
        }
    };

    self.removeReviewer = function () {
        self.approvalChain.remove(this);
    };

    self.cardinality = function () {
        return self.approvalChain().indexOf(this) + 1;
    };
}