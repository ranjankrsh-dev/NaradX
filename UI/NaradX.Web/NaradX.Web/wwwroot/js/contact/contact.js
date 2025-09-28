// Toggle filter box
document.getElementById('filterBtn').addEventListener('click', function () {
    document.getElementById('filterBox').classList.toggle('show');
});

// Select all checkbox
//document.getElementById('selectAll').addEventListener('change', function () {
//    const checkboxes = document.querySelectorAll('.employee-table tbody input[type="checkbox"]');
//    checkboxes.forEach(checkbox => {
//        checkbox.checked = this.checked;
//    });
//});

// Modal functionality
const modal = document.getElementById('newEmployeeModal');
const backdrop = document.getElementById('modalBackdrop');
const newEmployeeBtn = document.getElementById('newEmployeeBtn');
const closeModalBtn = document.getElementById('closeModal');
const cancelModalBtn = document.getElementById('cancelModal');
const expandModalBtn = document.getElementById('expandModal');

function showModal() {
    modal.classList.add('show');
    backdrop.style.display = 'block';
    setTimeout(() => {
        backdrop.classList.add('show');
    }, 10);
    document.body.style.overflow = 'hidden';
}

function hideModal() {
    modal.classList.remove('show');
    backdrop.classList.remove('show');
    setTimeout(() => {
        backdrop.style.display = 'none';
    }, 300);
    document.body.style.overflow = 'auto';
    // Reset fullscreen mode when closing
    modal.classList.remove('modal-fullscreen');
    expandModalBtn.classList.remove('icon-size-actual');
    expandModalBtn.classList.add('icon-size-fullscreen');
}

function toggleFullscreen() {
    modal.classList.toggle('modal-fullscreen');
    if (modal.classList.contains('modal-fullscreen')) {
        expandModalBtn.classList.remove('icon-size-fullscreen');
        expandModalBtn.classList.add('icon-size-actual');
    } else {
        expandModalBtn.classList.remove('icon-size-actual');
        expandModalBtn.classList.add('icon-size-fullscreen');
    }
}

newEmployeeBtn.addEventListener('click', showModal);
closeModalBtn.addEventListener('click', hideModal);
cancelModalBtn.addEventListener('click', hideModal);
expandModalBtn.addEventListener('click', toggleFullscreen);

// Close modal when clicking on backdrop
backdrop.addEventListener('click', hideModal);

// Close modal when pressing Escape key
document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape' && modal.classList.contains('show')) {
        hideModal();
    }
});




function loadContacts() {
    const $form = $('#commonFilterForm');

    const formData = $form.serializeArray();

    //formData.push({ name: 'PageNumber', value: pageNumber });

    //const nameFilter = document.querySelector('input[id="filterName"]');
    //const phoneFilter = document.querySelector('input[id="filterPhone"]');
    //const statusFilter = document.querySelector('select[id="filterStatus"]');

    //if (nameFilter && nameFilter.value) {
    //    formData.push({ name: 'Filters.Name', value: nameFilter.value });
    //}
    //if (phoneFilter && phoneFilter.value) {
    //    formData.push({ name: 'Filters.Phone', value: phoneFilter.value });
    //}
    //if (statusFilter && statusFilter.value) {
    //    formData.push({ name: 'Filters.Status', value: statusFilter.value });
    //}

    const serializedData = $.param(formData);

    $.ajax({
        url: $form.attr('action'),
        type: 'POST',
        data: serializedData,
        success: function (result) {
            $('#contactsTableContainer').html('');
            $('#contactsTableContainer').html(result);
        },
        error: function () {
            alert('Error loading contacts.');
        }
    });
}

function goToPage(pageNumber) {
    document.getElementById('pageNumber').value = pageNumber;
    loadContacts();
}

function applySearch() {
    document.getElementById('pageNumber').value = 1;
    document.getElementById('searchTerm').value = document.getElementById('searchInput').value;
    loadContacts();
}

function applyFilter() {
    document.getElementById('pageNumber').value = 1;
    document.getElementById('nameFilter').value = document.getElementById('filterName').value;
    document.getElementById('phoneFilter').value = document.getElementById('filterPhone').value;
    document.getElementById('statusFilter').value = document.getElementById('filterStatus').value;
    loadContacts();
}

function clearFilters() {
    document.getElementById('pageNumber').value = 1;
    document.getElementById('searchTerm').value = '';
    document.getElementById('nameFilter').value = '';
    document.getElementById('phoneFilter').value = '';
    document.getElementById('statusFilter').value = '';

    document.getElementById('searchInput').value = '';
    document.getElementById('filterName').value = '';
    document.getElementById('filterPhone').value = '';
    const statusDropdown = document.getElementById('filterStatus');
    statusDropdown.selectedIndex = 0;

    loadContacts();
}

function loadLanguages(currenRow) {
    var countryId = currenRow.value;
    const languageDropdown = document.getElementById('LanguageDropdown');
    if (!countryId || countryId == "-1") {
        languageDropdown.innerHTML = '';
        return;
    }

    fetch(`Contact/GetLanguagesByCountry?countryId=${encodeURIComponent(countryId)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Clear existing options
            languageDropdown.innerHTML = '';

            // Optionally add a default placeholder
            const placeholder = document.createElement('option');
            placeholder.value = '';
            placeholder.textContent = 'Select any';
            languageDropdown.appendChild(placeholder);

            // Add new options
            data.languages.forEach(item => {
                const opt = document.createElement('option');
                opt.value = item.id;
                opt.textContent = item.name;
                languageDropdown.appendChild(opt);
            });
        })
        .catch(error => {
            console.error('Error fetching languages:', error);
        });
}