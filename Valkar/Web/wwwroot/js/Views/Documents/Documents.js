window.Documents = (function () {
    var defaults = {
        deleteConfirmationUrl: ''
    };

    function applyFilter() {
        var signedFilter = (document.getElementById('filter-signed') && document.getElementById('filter-signed').value) || '';
        var searchText = (document.getElementById('filter-search') && document.getElementById('filter-search').value || '').trim().toLowerCase();
        var cards = document.querySelectorAll('.document-card');
        var visibleCount = 0;
        cards.forEach(function (card) {
            var signed = (card.getAttribute('data-doc-signed') || '') === 'true';
            var name = (card.getAttribute('data-doc-name') || '').toLowerCase();
            var employee = (card.getAttribute('data-doc-employee') || '').toLowerCase();
            var matchSigned = !signedFilter || (signedFilter === 'signed' && signed) || (signedFilter === 'unsigned' && !signed);
            var matchSearch = !searchText || name.indexOf(searchText) !== -1 || employee.indexOf(searchText) !== -1;
            var show = matchSigned && matchSearch;
            card.style.display = show ? '' : 'none';
            if (show) visibleCount++;
        });
        return visibleCount;
    }

    function clearFilter() {
        var sel = document.getElementById('filter-signed');
        var inp = document.getElementById('filter-search');
        if (sel) sel.value = '';
        if (inp) inp.value = '';
        applyFilter();
    }

    function initFilter() {
        var applyBtn = document.getElementById('documents-filter-apply');
        var clearBtn = document.getElementById('documents-filter-clear');
        var searchInput = document.getElementById('filter-search');
        if (applyBtn) applyBtn.addEventListener('click', function () { applyFilter(); });
        if (clearBtn) clearBtn.addEventListener('click', function () { clearFilter(); });
        if (searchInput) searchInput.addEventListener('input', function () { applyFilter(); });
    }

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);
        },
        onPreviewClick: function (docId) {
            window.open('/Documents/PreviewDocument?docId=' + docId, '_blank');
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