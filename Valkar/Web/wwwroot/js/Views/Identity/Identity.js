window.Identity = (function () {
    var defaults = {
        deleteConfirmationUrl: '',
    };

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