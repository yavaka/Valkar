namespace Infrastructure.Common
{
    public static class ModelConstants
    {
        public const int MAX_NAME_LENGTH = 30;
        public const int MIN_NAME_LENGTH = 3;

        public const int MAX_ADDRESS_LENGTH = 200;
        
        public const int FIXED_NINO_LENGTH = 9;
        
        public const int MAX_RELATIONSHIP_LENGTH = 20;
        
        public const int MAX_COMPANY_NAME_LENGTH = 160;
        public const int MIN_COMPANY_NAME_LENGTH = 3;

        public const int FIXED_COMPANY_REGISTRATION_NUMBER = 8;

        public const int MAX_FILE_SIZE = 20 * 1024 * 1024;

        public const string POSTCODE_REGEX = @"^([A-Z][A-HJ-Y]?[0-9][A-Z0-9]? ?[0-9][A-Z]{2}|GIR ?0A{2})$";
        public const string PHONE_NUMBER_REGEX = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";
        public const string FILE_EXTENSIONS_REGEX = @"\.doc|\.docx|\.pdf|\.jpg|\.jpeg|\.png|\.bmp$i";
        public const string NINO_REGEX = @"/[abceghj-prstw-z][abceghj-nprstw-z]\d{6}[abcd]/i";
        public const string EMAIL_REGEX = "^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";

        public const int MIN_DRIVER_AGE = 18;

        public const string SUCCESSFUL_UPDATE_MSG = "Your details was updated successfully.";
    }
}
