window.Companies = (function () {
    var defaults = {
        deleteConfirmationUrl: ''
    };

    function applyFilter() {
        var searchText = (document.getElementById('companies-filter-search') && document.getElementById('companies-filter-search').value || '').trim().toLowerCase();
        var rows = document.querySelectorAll('.company-row');
        var placeholders = document.querySelectorAll('.company-row-placeholder');
        var visibleCount = 0;
        rows.forEach(function (row) {
            var name = (row.getAttribute('data-company-name') || '').toLowerCase();
            var email = (row.getAttribute('data-company-email') || '').toLowerCase();
            var phone = (row.getAttribute('data-company-phone') || '').toLowerCase();
            var match = !searchText || name.indexOf(searchText) !== -1 || email.indexOf(searchText) !== -1 || phone.indexOf(searchText) !== -1;
            row.style.display = match ? '' : 'none';
            if (match) visibleCount++;
        });
        placeholders.forEach(function (row) {
            row.style.display = searchText ? 'none' : '';
        });
        return visibleCount;
    }

    function clearFilter() {
        var inp = document.getElementById('companies-filter-search');
        if (inp) inp.value = '';
        applyFilter();
    }

    function initFilter() {
        var applyBtn = document.getElementById('companies-filter-apply');
        var clearBtn = document.getElementById('companies-filter-clear');
        var searchInput = document.getElementById('companies-filter-search');
        if (applyBtn) applyBtn.addEventListener('click', function () { applyFilter(); });
        if (clearBtn) clearBtn.addEventListener('click', function () { clearFilter(); });
        if (searchInput) searchInput.addEventListener('input', function () { applyFilter(); });
    }

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
        },
        initFilter: initFilter,
        applyFilter: applyFilter,
        clearFilter: clearFilter
    };
})();