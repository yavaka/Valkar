window.Companies = (function () {
    var defaults = {
        deleteConfirmationUrl: ''
    };

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);
        },
        deleteConfirmation: function (id) {
            $.get(defaults.deleteConfirmationUrl, { id: id })
                .done(function (data) {
                    $('#deleteConfirmationDialog').html(data);
                    $('#deleteConfirmationDialog').find('.modal').modal('show');
                });
        }
    };
})();