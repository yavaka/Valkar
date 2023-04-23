window.Documents = (function () {
    var defaults = {
        deleteConfirmationUrl: ''
    };

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);
        },
        onPreviewClick(docId) {
            window.open(`/Documents/PreviewDocument?docId=${docId}`, "_blank");
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