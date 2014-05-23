function reviewer(name, email) {
    return {
        Id: 0,
        ReviewPartId: 0,
        IsApproved: false,
        ReviewDate: null,
        ReviewerName: name,
        ReviewerEmail: email
    };
}

function approvalChainViewModel(initChain) {
    var self = this;

    self.approvalChain = ko.observableArray(initChain);
    self.name = ko.observable("");
    self.email = ko.observable("");

    self.reviewerToAdd = ko.computed(function () {
        if (!(self.name() === "" || self.email() === ""))
            return reviewer(self.name(), self.email().toLowerCase());
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
        }
    };

    self.removeReviewer = function () {
        self.approvalChain.remove(this);
    };

    self.cardinality = function () {
        var t = this;
        var ac = self.approvalChain();
        var i = ac.indexOf(t);
        return i + 1;
    };
}