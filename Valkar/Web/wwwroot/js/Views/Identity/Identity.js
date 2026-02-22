window.Identity = (function () {
    var defaults = {
        deleteConfirmationUrl: '',
    };

    function initDeleteUserConfirmationModal() {
        // Modal content is loaded dynamically in deleteConfirmation(); no setup needed.
        // Use this to attach delegated handlers to #deleteUserConfirmationDialog if needed.
    }

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);

            initDeleteUserConfirmationModal();
        },
        deleteConfirmation: function (userId) {
            $.get(defaults.deleteConfirmationUrl, { userId: userId })
                .done(function (data) {
                    $('#deleteUserConfirmationDialog').html(data);
                    $('#deleteUserConfirmationDialog').find('.modal').modal('show');
                });
        }
    };
})();