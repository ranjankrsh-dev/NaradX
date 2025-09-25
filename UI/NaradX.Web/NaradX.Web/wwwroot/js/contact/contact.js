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

function goToPage(pageNumber) {
    document.getElementById('pageNumber').value = pageNumber;
    document.getElementById('filterForm').submit();
}

function applySearch() {
    document.getElementById('pageNumber').value = 1; // Reset to first page
    document.getElementById('searchTerm').value = document.getElementById('searchInput').value;
    document.getElementById('filterForm').submit();
}

function applyFilter(filterType, filterValue) {
    document.getElementById('pageNumber').value = 1; // Reset to first page
    document.getElementById(filterType + 'Filter').value = filterValue;
    document.getElementById('filterForm').submit();
}

function clearFilters() {
    document.getElementById('searchTerm').value = '';
    document.getElementById('nameFilter').value = '';
    document.getElementById('phoneFilter').value = '';
    document.getElementById('statusFilter').value = '';
    document.getElementById('filterForm').submit();
}

// Utility functions
function updateFormData(fieldName, value) {
    document.querySelector(`[name="${fieldName}"]`).value = value;
}

function getFormData() {
    const form = document.getElementById('filterForm');
    const formData = new FormData(form);
    return new URLSearchParams(formData).toString();
}