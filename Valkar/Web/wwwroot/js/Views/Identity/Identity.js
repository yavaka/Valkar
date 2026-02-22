window.Identity = (function () {
    var defaults = {
        deleteConfirmationUrl: '',
    };

    function initDeleteUserConfirmationModal() {
        // Modal content is loaded dynamically in deleteConfirmation(); no setup needed.
        // Use this to attach delegated handlers to #deleteUserConfirmationDialog if needed.
    }

    function applyDashboardFilter() {
        var searchText = (document.getElementById('dashboard-filter-search') && document.getElementById('dashboard-filter-search').value || '').trim().toLowerCase();
        var rows = document.querySelectorAll('.driver-row');
        var placeholders = document.querySelectorAll('.driver-row-placeholder');
        var visibleCount = 0;
        rows.forEach(function (row) {
            var name = (row.getAttribute('data-driver-name') || '').toLowerCase();
            var email = (row.getAttribute('data-driver-email') || '').toLowerCase();
            var phone = (row.getAttribute('data-driver-phone') || '').toLowerCase();
            var match = !searchText || name.indexOf(searchText) !== -1 || email.indexOf(searchText) !== -1 || phone.indexOf(searchText) !== -1;
            row.style.display = match ? '' : 'none';
            if (match) visibleCount++;
        });
        placeholders.forEach(function (row) {
            row.style.display = searchText ? 'none' : '';
        });
        return visibleCount;
    }

    function clearDashboardFilter() {
        var inp = document.getElementById('dashboard-filter-search');
        if (inp) inp.value = '';
        applyDashboardFilter();
    }

    function initDashboardFilter() {
        var applyBtn = document.getElementById('dashboard-filter-apply');
        var clearBtn = document.getElementById('dashboard-filter-clear');
        var searchInput = document.getElementById('dashboard-filter-search');
        if (applyBtn) applyBtn.addEventListener('click', function () { applyDashboardFilter(); });
        if (clearBtn) clearBtn.addEventListener('click', function () { clearDashboardFilter(); });
        if (searchInput) searchInput.addEventListener('input', function () { applyDashboardFilter(); });
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
        },
        initDashboardFilter: initDashboardFilter,
        applyDashboardFilter: applyDashboardFilter,
        clearDashboardFilter: clearDashboardFilter
    };
})();