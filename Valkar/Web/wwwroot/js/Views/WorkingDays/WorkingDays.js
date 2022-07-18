module.WorkingDays = (function () {
    var defaults = {};

    return {
        onInit: function (obj) {
            flatpickr("#break", {
                time_24hr: true
            });
        },
    };

})();