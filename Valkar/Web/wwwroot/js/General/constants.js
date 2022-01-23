// This file keeps all constants

export {
    maxNameLength,
    minNameLength,
    addressMaxLength,
    postcodeRegex,
    phoneNumberRegex,
    maxFileSizeInMB,
    fileExtensionsRegex,
    fixedNiNoLength,
    ninoRegex,
    emailRegex,
    maxCompanyNameLength,
    fixedCompanyRegNumLength
}

const maxNameLength = 30;
const minNameLength = 3;
const addressMaxLength = 200;
const postcodeRegex = /^([A-Z][A-HJ-Y]?[0-9][A-Z0-9]? ?[0-9][A-Z]{2}|GIR ?0A{2})$/;
const phoneNumberRegex = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
const maxFileSizeInMB = 20 * 1024;
const fileExtensionsRegex = /(\.doc|\.docx|\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;
const fixedNiNoLength = 9;
const ninoRegex = /[abceghj-prstw-z][abceghj-nprstw-z]\d{6}[abcd]/i;
const emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
const maxCompanyNameLength = 160;
const fixedCompanyRegNumLength = 8;