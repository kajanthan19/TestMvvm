function AircrafResult(data) {
    var self = this;
    self.make = ko.observable(data.make);
    self.model = ko.observable(data.model);
    self.registration = ko.observable(data.registration);
    self.location = ko.observable(data.location);
    self.aircraftSeen = ko.observable(data.aircraftSeen);
    self.imageUrl = ko.observable(data.imageUrl);
    self.id = ko.observable(data.id);

}

function AircraftViewModel() {
    var self = this;

    // declare aircrafts 
    self.Aircrafts = ko.observableArray([]);
    self.isEdit = ko.observable(false);
    self.isNew = ko.observable(false);
    self.disableSubmitButton = ko.observable(false);
    self.selectedIndex = ko.observable(null);
    self.errorMessage = ko.observable(null);
    self.isLoading = ko.observable(true);
    self.filter = ko.observable('');

    self.Make = ko.observable().extend({ required: true, maxLength: 128 });
    self.Model = ko.observable().extend({ required: true, maxLength: 128 });
    self.Registration = ko.observable().extend({
        required: true,
        pattern: {
            message: 'Registration does not match my pattern',
            params: '^([A-Z]{1,2})\-([A-Z]{1,5}$)+'
        }  
    });
    self.Location = ko.observable().extend({ required: true, maxLength: 255 });
    self.AircraftSeen = ko.observable(new Date()).extend({ required: true });
    self.ImageUrl = ko.observable();
    self.ImageFile = null;
    self.Id = null;

    // Get All Aircrafts
    $.ajax({
        type: "GET",
        url: 'api/aircraft',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var aircrafts = $.map(response, function (item) {
                return new AircrafResult(item)
            });
            self.Aircrafts(aircrafts);
            self.isLoading(false);
            // console.log(self.Aircrafts())
        },
        error: function (err) {
            self.errorMessage("Exception throw during the get all operation");
            self.isLoading(false);
            console.log(err);
        }
    }); 


    self.errors = ko.validation.group(this, { deep: true, observable: false });

    // Show Add Form
    self.onClickNew = function () {
        self.isNew(true);
    };

    // On click Form Submit Add or Edit
    self.onSubmit = function () {
        if (self.errors().length === 0) { 
            self.errorMessage(null);
            self.disableSubmitButton(true);
            var formData = new FormData();
            formData.append("Registration", self.Registration().toString());
            formData.append("Location", self.Location().toString());
            formData.append("Make", self.Make().toString());
            formData.append("Model", self.Model().toString());
            formData.append("AircraftSeen", moment(self.AircraftSeen()).format("YYYY-MM-DD[T]HH:mm:ss.SSS[Z]"));
            formData.append("ImageFile", self.ImageFile);

            if (self.isNew()) { // Add Api call
                $.ajax({
                    type: "POST",
                    url: "api/aircraft",
                    data: formData,
                    cache: false,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (result) {
                        self.Aircrafts.push(new AircrafResult(result));
                        self.onReset();
                        self.disableSubmitButton(false);
                    }, error: function (err) {
                        self.disableSubmitButton(false);
                        self.errorMessage("Exception throw during the add operation");
                        console.log(err);
                    }
                });
            } else { // Edit API call

                formData.append("Id", self.Id);

                $.ajax({
                    type: "PUT",
                    url: "api/aircraft/" + self.Id,
                    data: formData,
                    cache: false,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (result) {
                        // Update Array 
                        self.Aircrafts.replace(self.Aircrafts()[self.selectedIndex()], new AircrafResult(result))
                        self.selectedIndex(null);
                        self.onReset();
                        self.isEdit(false);
                        self.disableSubmitButton(false);
                    }, error: function (err) {
                        self.disableSubmitButton(false);
                        self.errorMessage("Exception throw during the edit operation");
                        console.log(err);
                    }
                });

            }
        } else {
            self.errors.showAllMessages();
        }
    };

    // File upload and display image
    self.fileUpload = function(data, e) {
        self.ImageFile = e.target.files[0];
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onloadend = function (onloadend_e) {
            var result = reader.result;
            self.ImageUrl(result);
        };

        if (file) {
            reader.readAsDataURL(file);
        }
    };

    // Edit Row
    self.onEditAircraft = function (data, index) {
        self.selectedIndex(index);
        self.Make(data.make());
        self.Model(data.model());
        self.Registration(data.registration());
        self.Location(data.location());
        self.AircraftSeen(new Date(data.aircraftSeen()));
        self.ImageUrl(data.imageUrl());
        self.Id = data.id();
        self.isEdit(true);
        self.isNew(false);
    };

    // Delete Aircraft
    self.onDeleteRow = function (data, index) {
        self.selectedIndex(index);
        var confirm_delete = confirm('Are you sure you want to delete this?');
        if (confirm_delete) {
            self.errorMessage(null);
            $.ajax({
                type: "DELETE",
                url: 'api/aircraft/' + data.id(),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    // Remove from the List
                    self.Aircrafts.remove(data) 
                    self.selectedIndex(null);
                },
                error: function (err) {
                    self.selectedIndex(null);
                    self.errorMessage("Exception throw during the delete operation");
                    console.log(err);
                }
            });

        } else {
            self.selectedIndex(null);
        }
    }

    // Reset
    self.onReset = function () {
        self.Make('');
        self.Model('');
        self.Registration('');
        self.Location('');
        self.AircraftSeen('');
        self.ImageUrl('');
        self.ImageFile = null;
        self.errors.showAllMessages(false);
    }

    // Cancel screen
    self.onCancelScreen = function () {
        self.isNew(false);
        self.isEdit(false);
        self.selectedIndex(null);
        self.onReset();
    }

    // Search Filter Function
    self.filteredAircrafts = ko.computed(function () {
        var filter = self.filter().toLowerCase();
        if (!filter) { return self.Aircrafts(); }
        return self.Aircrafts().filter(function (val) {
            return val.registration().toLowerCase().indexOf(filter) > -1 || val.model().toLowerCase().indexOf(filter) > -1 || val.make().toLowerCase().indexOf(filter) > -1;
        });
    });

};

// Date time picker implementation
ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {

        var options = {
            format: 'DD/MM/YYYY HH:mm',
            date: ko.utils.unwrapObservable(valueAccessor()),
            maxDate: new Date(),
            maxTime: new Date(),
            disabledTimeIntervals: [
                [moment(), moment().hour(24).minutes(0).seconds(0)]
            ],
        };

        if (allBindingsAccessor() !== undefined) {
            if (allBindingsAccessor().dateTimePickerOptions !== undefined) {
                options.format = allBindingsAccessor().dateTimePickerOptions.format !== undefined ? allBindingsAccessor().dateTimePickerOptions.format : options.format;
            }
        }

        $(element).datetimepicker(options);

        ko.utils.registerEventHandler(element, "change.datetimepicker", function (event) {
            var value = valueAccessor();
            if (ko.isObservable(value)) {
                value(event.date || event.target.value || null);
            }
        });

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datetimepicker("destroy");
        });
    },
    update: function (element, valueAccessor) {
        var val = ko.utils.unwrapObservable(valueAccessor());
        if ($(element).datetimepicker) {
            $(element).datetimepicker("date", val);
        }
    }
};



// VALIDATION RULES
var knockoutValidationSettings = {
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null,
    errorElementClass: 'input-error',
    errorClass: 'error',
    decorateElementOnModified: true,
    decorateInputElement: true
};

ko.validation.init(knockoutValidationSettings, true);

ko.applyBindings(new AircraftViewModel());