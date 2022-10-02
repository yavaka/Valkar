window.Companies = (function () {
    var defaults = {
        deleteConfirmationUrl: '',
        companyId: ''
    };

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);

            initDeleteConfirmationModal();
        },
        updateDefaults: function (obj) {
            $.extend(defaults, obj);
        }
    };

    function initDeleteConfirmationModal() {
        $('a[data-bs-toggle="modal"]').click(function () {
            debugger;
            $.get(defaults.deleteConfirmationUrl, { id: defaults.companyId })
                .done(function (data) {
                    debugger;
                    $('#deleteConfirmationDialog').html(data);
                    $('#deleteConfirmationDialog').find('.modal').modal('show');
                });
        });
    }

})();