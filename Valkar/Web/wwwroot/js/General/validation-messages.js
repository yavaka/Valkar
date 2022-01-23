// This file keeps all error messages

export {
    requiredErrorMessage,
    minMaxlengthErrorMessage,
    maxLengthErrorMessage,
    dropDownOptionNotSelectedErrorMessage,
    invalidPostcodeErrorMessage,
    underAgeErrorMessage,
    invalidPhoneNumberErrorMessage,
    exceedFileSizeErrorMessage,
    fileExtensionErrorMessage,
    invalidNiNoLengthErrorMessage,
    invalidEmailErrorMessage,
    invalidCompanyRegNumberLengthErrorMessage
}

function requiredErrorMessage(fieldName) {
    return `${fieldName} required`;
}

function minMaxlengthErrorMessage(fieldName, minLength, maxLength) {
    return `${fieldName} cannot be less than ${minLength} and more than ${maxLength} symbols`;
}

function maxLengthErrorMessage(fieldName, maxLength) {
    return `${fieldName} cannot be more than ${maxLength} symbols`;
}

function dropDownOptionNotSelectedErrorMessage(fieldName) {
    return `Please select a ${fieldName}`;
}

function invalidPostcodeErrorMessage() {
    return 'Invalid postcode';
}

function underAgeErrorMessage() {
    return 'Age is under 18';
}

function invalidPhoneNumberErrorMessage() {
    return 'Invalid phone number';
}

function exceedFileSizeErrorMessage() {
    return `File size is more than 20MB`;
}

function fileExtensionErrorMessage() {
    return 'Invalid file, allowed file extensions are .jpg, .jpeg, .png, .bmp, .pdf, .doc, .docx';
}

function invalidNiNoLengthErrorMessage() {
    return 'Invalid national insurance number'
}

function invalidEmailErrorMessage() {
    return 'Invalid email'
}

function invalidCompanyRegNumberLengthErrorMessage() {
    return 'Company registration number is fixed 8 symbols long'
}