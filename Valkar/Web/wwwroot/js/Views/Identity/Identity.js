window.Identity = (function () {
    var defaults = {
        deleteConfirmationUrl: '',
        userId: '', // used when deleting a user
    };

    return {
        onInit: function (obj) {
            debugger;
            $.extend(defaults, obj);

            initDeleteUserConfirmationModal();
        },
        updateDefaults: function (obj) {
            $.extend(defaults, obj);
        }
    };

    function initDeleteUserConfirmationModal() {

        $('a[data-bs-toggle="modal"]').click(function () {
            debugger;

            $.get(defaults.deleteConfirmationUrl, { userId: defaults.userId })
                .done(function (data) {
                    debugger;
                    $('#deleteUserConfirmationDialog').html(data);
                    $('#deleteUserConfirmationDialog').find('.modal').modal('show');
                });
        });


    }

    //#region Helpers


    //#endregion

})();