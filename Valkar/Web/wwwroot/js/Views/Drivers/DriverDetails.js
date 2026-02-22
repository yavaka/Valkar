import * as Constants from '../../General/constants.js';
import * as ErrorMessages from '../../General/validation-messages.js';

window.DriverDetails = (function () {
    var defaults = {};

    return {
        onInit: function (obj) {

            initializeFlatpickr();

            // Control the limited company visibility
            $('input[type="radio"]').click(function () {
                // Get the checked radio button
                var inputValue = $("input[type='radio']:checked").val();
                if (inputValue === "Yes") {
                    $('.limited-company').show();
                }
                else if (inputValue === "No") {
                    $('.limited-company').hide();
                }
            });

            onChangeFieldEvents();

            $('#submit-btn').bind('click', function () {

                // if the validations does not pass, the system will not submit the form
                if (!validations()) {
                    return false;
                };
            });
        },
        dlCategoriesClickEvent: function () {
            $('#dl-categories-validation-message').text('');
        }
    };

    //#region Validations

    function validations() {
        //#region Driver
        var isFormValid = true;

        // Title
        if ($('#driver-title').find(":selected").text() == 'None') {
            $('#driver-title-validation-message').text(ErrorMessages.dropDownOptionNotSelectedErrorMessage('Title'));
            isFormValid = false;
        }

        // First names
        var firstNames = $('#driver-first-names').val();
        if (firstNames.length == 0 || firstNames == "") {
            $('#driver-first-names-validation-message').text(ErrorMessages.requiredErrorMessage('First name/s'));
            isFormValid = false;
        } else if (firstNames.length > Constants.maxNameLength || firstNames.length < Constants.minNameLength) {
            $('#driver-first-names-validation-message').text(ErrorMessages.minMaxlengthErrorMessage('First name/s', Constants.minNameLength, Constants.maxNameLength));
            isFormValid = false;
        }

        // Surname
        var surname = $('#driver-surname').val();
        if (surname.length == 0 || surname == "") {
            $('#driver-surname-validation-message').text(ErrorMessages.requiredErrorMessage('Surname'));
            isFormValid = false;
        } else if (surname.length > Constants.maxNameLength || surname.length < Constants.minNameLength) {
            $('#driver-surname-validation-message').text(ErrorMessages.minMaxlengthErrorMessage('Surname', Constants.minNameLength, Constants.maxNameLength));
            isFormValid = false;
        }

        // Address
        var address = $('#driver-address').val();
        if (address.length == 0 || address == "") {
            $('#driver-address-validation-message').text(ErrorMessages.requiredErrorMessage('Address'));
            isFormValid = false;
        } else if (address.length > Constants.addressMaxLength) {
            $('#driver-address-validation-message').text(ErrorMessages.maxLengthErrorMessage('Address', Constants.addressMaxLength));
            isFormValid = false;
        }

        // Postcode
        var postcode = $('#driver-postcode').val();
        if (postcode.length == 0 || postcode == "") {
            $('#driver-postcode-validation-message').text(ErrorMessages.requiredErrorMessage('Postcode'));
            isFormValid = false;
        } else if (!isPostcodeValid(postcode)) {
            $('#driver-postcode-validation-message').text(ErrorMessages.invalidPostcodeErrorMessage());
            isFormValid = false;
        }

        // Date of birth
        validateDateOfBirth();

        // Phone number
        var phoneNumber = $('#driver-phone-number').val();
        if (phoneNumber.length == 0 || phoneNumber == "") {
            $('#driver-phone-number-validation-message').text(ErrorMessages.requiredErrorMessage('Phone number'));
            isFormValid = false;
        } else if (!isPhoneNumberValid(phoneNumber)) {
            $('#driver-phone-number-validation-message').text(ErrorMessages.invalidPhoneNumberErrorMessage());
            isFormValid = false;
        }

        //#endregion

        //#region Documents

        // DL categories
        var selectedDLCategories = $('#dl-categories').find("input[type='checkbox']:checked");
        if (selectedDLCategories.length == 0) {
            $('#dl-categories-validation-message').text(ErrorMessages.requiredErrorMessage('Driving licence categories'));
            isFormValid = false;
        }

        // All uploaded files
        if (!validateUploadedFiles()) {
            isFormValid = false;
        }

        // Nino
        var nino = $('#nino').val();
        if (nino.length == 0 || nino == "") {
            $('#nino-validation-message').text(ErrorMessages.requiredErrorMessage('National insurance number'));
            isFormValid = false;
        } else if (nino.length != Constants.fixedNiNoLength
            || !isNiNoValid(nino)) {
            $('#nino-validation-message').text(ErrorMessages.invalidNiNoLengthErrorMessage());
            isFormValid = false;
        }

        //#endregion

        //#region Emergency contact

        // Title
        if ($('#ec-title').find(":selected").text() == 'None') {
            $('#ec-title-validation-message').text(ErrorMessages.dropDownOptionNotSelectedErrorMessage('Title'));
            isFormValid = false;
        }

        // First names
        firstNames = $('#ec-first-names').val();
        if (firstNames.length == 0 || firstNames == "") {
            $('#ec-first-names-validation-message').text(ErrorMessages.requiredErrorMessage('First name/s'));
            isFormValid = false;
        } else if (firstNames.length > Constants.maxNameLength || firstNames.length < Constants.minNameLength) {
            $('#ec-first-names-validation-message').text(ErrorMessages.minMaxlengthErrorMessage('First name/s', Constants.minNameLength, Constants.maxNameLength));
            isFormValid = false;
        }

        // Surname
        surname = $('#ec-surname').val();
        if (surname.length == 0 || surname == "") {
            $('#ec-surname-validation-message').text(ErrorMessages.requiredErrorMessage('Surname'));
            isFormValid = false;
        } else if (surname.length > Constants.maxNameLength || surname.length < Constants.minNameLength) {
            $('#ec-surname-validation-message').text(ErrorMessages.minMaxlengthErrorMessage('Surname', Constants.minNameLength, Constants.maxNameLength));
            isFormValid = false;
        }

        // Address
        address = $('#ec-address').val();
        if (address.length == 0 || address == "") {
            $('#ec-address-validation-message').text(ErrorMessages.requiredErrorMessage('Address'));
            isFormValid = false;
        } else if (address.length > Constants.addressMaxLength) {
            $('#ec-address-validation-message').text(ErrorMessages.maxLengthErrorMessage('Address', Constants.addressMaxLength));
            isFormValid = false;
        }

        // Postcode
        postcode = $('#ec-postcode').val();
        if (postcode.length == 0 || postcode == "") {
            $('#ec-postcode-validation-message').text(ErrorMessages.requiredErrorMessage('Postcode'));
            isFormValid = false;
        } else if (!isPostcodeValid(postcode)) {
            $('#ec-postcode-validation-message').text(ErrorMessages.invalidPostcodeErrorMessage());
            isFormValid = false;
        }

        //Email
        var email = $('#ec-email').val();
        if (email.length == 0 || email == "") {
            $('#ec-email-validation-message').text(ErrorMessages.requiredErrorMessage('Email'));
            isFormValid = false;
        } else if (!isEmailValid(email)) {
            $('#ec-email-validation-message').text(ErrorMessages.invalidEmailErrorMessage());
            isFormValid = false;
        }

        // Phone number
        phoneNumber = $('#ec-phone-number').val();
        if (phoneNumber.length == 0 || phoneNumber == "") {
            $('#ec-phone-number-validation-message').text(ErrorMessages.requiredErrorMessage('Phone number'));
            isFormValid = false;
        } else if (!isPhoneNumberValid(phoneNumber)) {
            $('#ec-phone-number-validation-message').text(ErrorMessages.invalidPhoneNumberErrorMessage());
            isFormValid = false;
        }

        //Relationship
        var relationship = $('#ec-relationship').val();
        if (relationship.length == 0 || relationship == "") {
            $('#ec-relationship-validation-message').text(ErrorMessages.requiredErrorMessage('Relationship'));
            isFormValid = false;
        } else if (relationship.length > Constants.maxNameLength) {
            $('#ec-relationship-validation-message').text(ErrorMessages.maxLengthErrorMessage('Relationship', Constants.maxNameLength));
            isFormValid = false;
        }

        //#endregion

        //#region Company

        if ($('#radio-btn-yes').is(':checked')) {

            // Company name
            var companyName = $('#company-name').val();
            if ((companyName.length == 0 || companyName == "")) {
                $('#company-name-validation-message').text(ErrorMessages.requiredErrorMessage('Company name'));
                isFormValid = false;
            } else if (companyName.length > Constants.maxCompanyNameLength) {
                $('#company-name-validation-message').text(ErrorMessages.maxLengthErrorMessage('Company name', Constants.maxCompanyNameLength));
                isFormValid = false;
            }

            // Company reg num
            var companyRegNum = $('#company-reg-num').val();
            if (companyRegNum.length == 0 || companyRegNum == "") {
                $('#company-reg-num-validation-message').text(ErrorMessages.requiredErrorMessage('Company registration number'));
                isFormValid = false;
            } else if (companyRegNum.length != Constants.fixedCompanyRegNumLength) {
                $('#company-reg-num-validation-message').text(ErrorMessages.invalidCompanyRegNumberLengthErrorMessage());
                isFormValid = false;
            }
        }

        //#endregion

        // if the form is not valid the system scroll to the top of the page and return invalid form
        if (!isFormValid) {
            scrollTop();
            return isFormValid;
        }
        return isFormValid;
    }

    //#region File validations

    function validateUploadedFiles() {
        var isValid = true;

        // DL front
        var dlFront = $('#dl-front').val();
        if (dlFront == "") {
            $('#dl-front-validation-message').text(ErrorMessages.requiredErrorMessage('Driving licence front'));
            isValid = false;
        } else if (!validateFileSize('dl-front', 'dl-front-validation-message')) {
            isValid = false;
        } else if (!validateFileExtension('dl-front', 'dl-front-validation-message')) {
            isValid = false;
        }

        // DL back
        var dlBack = $('#dl-back').val();
        if (dlBack == "") {
            $('#dl-back-validation-message').text(ErrorMessages.requiredErrorMessage('Driving licence back'));
            isValid = false;
        } else if (!validateFileSize('dl-back', 'dl-back-validation-message')) {
            isValid = false;
        } else if (!validateFileExtension('dl-back', 'dl-back-validation-message')) {
            isValid = false;
        }

        // ID front
        var idFront = $('#id-front').val();
        if (idFront == "") {
            $('#id-front-validation-message').text(ErrorMessages.requiredErrorMessage('Identity document front'));
            isValid = false;
        } else if (!validateFileSize('id-front', 'id-front-validation-message')) {
            isValid = false;
        } else if (!validateFileExtension('id-front', 'id-front-validation-message')) {
            isValid = false;
        }

        // ID back - It can be empty when the driver upload its passport instead of identity card
        var idBack = $('#id-back').val();
        if (idBack != "") {
            if (!validateFileSize('id-back', 'id-back-validation-message')) {
                isValid = false;
            } else if (!validateFileExtension('id-back', 'id-back-validation-message')) {
                isValid = false;
            }
        }

        // Nino letter
        var nino = $('#nino-letter').val();
        if (nino == "") {
            $('#nino-letter-validation-message').text(ErrorMessages.requiredErrorMessage('National insurance number letter'));
            isValid = false;
        } else if (!validateFileSize('nino-letter', 'nino-letter-validation-message')) {
            isValid = false;
        } else if (!validateFileExtension('nino-letter', 'nino-letter-validation-message')) {
            isValid = false;
        }

        return isValid;
    }

    function validateFileSize(uploadedFileFieldId, fieldValidationMessageId) {
        const f = $(`#${uploadedFileFieldId}`)[0];
        const fileSize = f.files[0].size;
        // convert the file size in MB
        const file = Math.round((fileSize / 1024));
        // The size of the file.
        if (file >= Constants.maxFileSizeInMB) {
            $(`#${fieldValidationMessageId}`).text(ErrorMessages.exceedFileSizeErrorMessage());
            return false;
        }
        return true;
    }

    function validateFileExtension(uploadedFileFieldId, fieldValidationMessageId) {
        const file = $(`#${uploadedFileFieldId}`)[0];
        var filePath = file.value;

        if (!Constants.fileExtensionsRegex.exec(filePath)) {
            $(`#${fieldValidationMessageId}`).text(ErrorMessages.fileExtensionErrorMessage());
            file.value = '';
            return false;
        }
        return true;
    }

    //#endregion

    function validateDateOfBirth() {
        var dateOfBirth = $('#datepicker').val();

        if (dateOfBirth.length == 0 || dateOfBirth == "" || dateOfBirth == 'Select Date') {
            $('#driver-date-of-birth-validation-message').text(ErrorMessages.requiredErrorMessage('Date of birth'));
            return false;
        }

        var dateParts = dateOfBirth.split("/");
        // month is 0-based, that's why we need dataParts[1] - 1
        var birthDate = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);

        // get difference from current date;
        var difference = Date.now() - birthDate.getTime();

        var ageDate = new Date(difference);
        var calculatedAge = Math.abs(ageDate.getUTCFullYear() - 1970);

        if (calculatedAge < 18) {
            $('#driver-date-of-birth-validation-message').text(ErrorMessages.underAgeErrorMessage());
            return false;
        }
        return true;
    }

    function isPostcodeValid(postcode) {
        return Constants.postcodeRegex.test(postcode);
    }

    function isPhoneNumberValid(phoneNumber) {
        return Constants.phoneNumberRegex.test(phoneNumber);
    }

    function isNiNoValid(nino) {
        return Constants.ninoRegex.test(nino);
    }

    function isEmailValid(email) {
        return Constants.emailRegex.test(email.toLowerCase());
    }

    //#endregion

    //#region Helpers

    function onChangeFieldEvents() {

        //#region Driver

        // Title
        $('#driver-title').on('change', function () {
            $('#driver-title-validation-message').text('');
        });
        // First names
        $('#driver-first-names').blur(function () {
            $('#driver-first-names-validation-message').text('');
        });
        // Surname
        $('#driver-surname').blur(function () {
            $('#driver-surname-validation-message').text('');
        });
        // Address
        $('#driver-address').blur(function () {
            $('#driver-address-validation-message').text('');
        });
        // Postcode
        $('#driver-postcode').blur(function () {
            $('#driver-postcode-validation-message').text('');
        });
        // Date of birth
        $('#datepicker').blur(function () {
            $('#driver-date-of-birth-validation-message').text('');
        });
        // Phone number
        $('#driver-phone-number').blur(function () {
            $('#driver-phone-number-validation-message').text('');
        });

        //#endregion

        //#region Documents

        // DL categories
        // There is an onclick event dlCategoriesClickEvent()

        // DL front
        $('#dl-front').on('click', function () {
            $('#dl-front-validation-message').text('');
        });

        // DL back
        $('#dl-back').on('click', function () {
            $('#dl-back-validation-message').text('');
        });

        // ID front
        $('#id-front').on('click', function () {
            $('#id-front-validation-message').text('');
        });

        // ID back
        $('#id-back').on('click', function () {
            $('#id-back-validation-message').text('');
        });

        // Nino
        $('#nino').blur(function () {
            $('#nino-validation-message').text('');
        });

        // Nino letter
        $('#nino-letter').on('click', function () {
            $('#nino-letter-validation-message').text('');
        });

        //#endregion

        //#region Emergency contact

        // Title
        $('#ec-title').on('change', function () {
            $('#ec-title-validation-message').text('');
        });
        // First names
        $('#ec-first-names').blur(function () {
            $('#ec-first-names-validation-message').text('');
        });
        // Surname
        $('#ec-surname').blur(function () {
            $('#ec-surname-validation-message').text('');
        });
        // Address
        $('#ec-address').blur(function () {
            $('#ec-address-validation-message').text('');
        });
        // Postcode
        $('#ec-postcode').blur(function () {
            $('#ec-postcode-validation-message').text('');
        });
        // Email
        $('#ec-email').blur(function () {
            $('#ec-email-validation-message').text('');
        });
        // Phone number
        $('#ec-phone-number').blur(function () {
            $('#ec-phone-number-validation-message').text('');
        });
        // Relationship
        $('#ec-relationship').blur(function () {
            $('#ec-relationship-validation-message').text('');
        });

        //#endregion

        //#region Company

        // Company name
        $('#company-name').blur(function () {
            $('#company-name-validation-message').text('');
        });
        // Company reg num
        $('#company-reg-num').blur(function () {
            $('#company-reg-num-validation-message').text('');
        });

        //#endregion
    }

    function scrollTop() {
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }

    function initializeFlatpickr() {
        flatpickr("#datepicker", {
            "defaultDate": new Date(),
            "allowInput": false,
            "maxDate": new Date(),
            "onOpen": function (selectedDates, dateStr, instance) {
                instance.setDate(instance.input.value, false);
            }
        });
    }

    //#endregion

})();