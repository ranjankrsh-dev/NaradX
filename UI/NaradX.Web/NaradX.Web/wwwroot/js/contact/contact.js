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

newEmployeeBtn.addEventListener('click', function () {
    resetEmployeeModal();
    showModal();
});
//newEmployeeBtn.addEventListener('click', showModal);
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

    const serializedData = $.param(formData);

    $.ajax({
        url: $form.attr('action'),
        type: 'POST',
        data: serializedData,
        success: function (result) {
            $('#contactsTableContainer').html('');
            $('#contactsTableContainer').html(result);
            $('[data-toggle="popover"]').popover();
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

async function loadLanguages(currenRow, operationName) {
    var countryId = currenRow.value;

    var languageDropdown = null;
    if (operationName == 'singleCreate') {
        languageDropdown = document.getElementById('singleLanguage');
    }
    else if (operationName == 'bulkUpload') {
        languageDropdown = document.getElementById('bulkLanguage');
    }

    if (!countryId || countryId == "-1") {
        languageDropdown.innerHTML = '';
        return;
    }

    const result = await actionHelper.get(`Contact/GetLanguagesByCountry?countryId=${encodeURIComponent(countryId)}`);

    if (result) {
        languageDropdown.innerHTML = '';

        const placeholder = document.createElement('option');
        placeholder.value = '';
        placeholder.textContent = 'Select any';
        placeholder.disabled = true;
        languageDropdown.appendChild(placeholder);

        result.languages.forEach(item => {
            const opt = document.createElement('option');
            opt.value = item.id;
            opt.textContent = item.name;
            languageDropdown.appendChild(opt);
        });
    } else {
        console.error('Error fetching languages:');
    }
}

function bulkModalOpen() {
    $('#bulkUploadModal').modal('show');
}


 //Bulk Contact Upload Functionality
document.addEventListener('DOMContentLoaded', function () {
    const bulkUploadModal = document.getElementById('bulkUploadModal');
    if (!bulkUploadModal) return;

    // Elements
    const nextBtn = document.getElementById('nextBtn');
    const backBtn = document.getElementById('backBtn');
    const submitBtn = document.getElementById('submitBtn');
    const step1 = document.getElementById('step1');
    const step2 = document.getElementById('step2');
    const step3 = document.getElementById('step3');
    const steps = document.querySelectorAll('.step');
    const fileInfo = document.getElementById('fileInfo');
    const uploadArea = document.getElementById('uploadArea');
    const fileInput = document.getElementById('fileInput');
    const browseFileBtn = document.getElementById('browseFileBtn');
    const removeFileBtn = document.getElementById('removeFileBtn');
    const bulkCountry = document.getElementById('bulkCountry');
    const bulkLanguage = document.getElementById('bulkLanguage');

    let currentStep = 1;
    let selectedFile = null;
    let validationResult = null;

    document.getElementById('openBulkUploadModal').addEventListener('click', function () {
        resetModal();
        bulkModalOpen();
    });

    // File upload handling
    browseFileBtn.addEventListener('click', function () {
        fileInput.click();
    });

    fileInput.addEventListener('change', handleFileSelect);
    removeFileBtn.addEventListener('click', removeFile);

    // Drag and drop functionality
    uploadArea.addEventListener('dragover', function (e) {
        e.preventDefault();
        uploadArea.classList.add('dragover');
    });

    uploadArea.addEventListener('dragleave', function () {
        uploadArea.classList.remove('dragover');
    });

    uploadArea.addEventListener('drop', function (e) {
        e.preventDefault();
        uploadArea.classList.remove('dragover');
        if (e.dataTransfer.files.length) {
            handleFile(e.dataTransfer.files[0]);
        }
    });

    // Step navigation
    nextBtn.addEventListener('click', handleNext);
    backBtn.addEventListener('click', handleBack);
    submitBtn.addEventListener('click', handleSubmit);

    function handleFileSelect(e) {
        if (e.target.files.length) {
            handleFile(e.target.files[0]);
        }
    }

    function handleFile(file) {
        // Validate file type and size
        const validTypes = ['.csv', '.xlsx', '.xls'];
        const maxSize = 10 * 1024 * 1024; // 10MB

        const fileExtension = '.' + file.name.split('.').pop().toLowerCase();

        if (!validTypes.includes(fileExtension)) {
            showAlert('Please select a valid file type (CSV, XLSX, XLS)', 'danger');
            return;
        }

        if (file.size > maxSize) {
            showAlert('File size must be less than 10MB', 'danger');
            return;
        }

        selectedFile = file;
        updateFileInfo(file);
        fileInput.value = '';
    }

    async function updateFileInfo(file) {
        const fileName = document.getElementById('fileName');
        const fileSize = document.getElementById('fileSize');

        fileName.textContent = file.name;
        fileSize.textContent = `(${formatFileSize(file.size)})`;
        fileInfo.classList.remove('d-none');

        validationResult = await validateFileWithAPI();
        if (validationResult.batchId == null && validationResult.totalRows === 0) {
            nextBtn.disabled = true;
            nextBtn.innerHTML = 'Next';
            showAlert(validationResult.message, 'error');
        }
        else {
            nextBtn.disabled = false;
            nextBtn.innerHTML = 'Next';
        }
    }

    function removeFile() {
        selectedFile = null;
        fileInfo.classList.add('d-none');
        fileInput.value = '';
    }

    function formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    function validateStep1() {
        const country = bulkCountry.value;
        const language = bulkLanguage.value;
        const contactSource = document.getElementById('bulkContactSource').value;
        const channelPreference = document.getElementById('bulkChannelPreference').value;

        if (!country || country === '-1') {
            showAlert('Please select a country', 'danger');
            return false;
        }
        if (!language || language === '-1') {
            showAlert('Please select a language', 'danger');
            return false;
        }
        if (!contactSource) {
            showAlert('Please select a contact source', 'danger');
            return false;
        }
        if (!channelPreference) {
            showAlert('Please select a channel preference', 'danger');
            return false;
        }
        if (!selectedFile) {
            showAlert('Please upload a file', 'danger');
            return false;
        }

        return true;
    }

    async function handleNext() {
        if (currentStep === 1) {
            if (!validateStep1()) return;

            // Show loading state
            nextBtn.disabled = true;
            nextBtn.innerHTML = '<i class="fa fa-spinner fa-spin me-2"></i>Validating...';

            try {
                //// Simulate API validation call
                //validationResult = await validateFileWithAPI();

                // Update step 2 with validation results
                updateValidationResults(validationResult);

                // Move to step 2
                showStep(2);

                // Auto-proceed to step 3 if no invalid rows
                if (validationResult.invalidRowsCount === 0) {
                    setTimeout(() => {
                        updateReviewStep(validationResult);
                        showStep(3);
                    }, 1500);
                }

            } catch (error) {
                showAlert('Error validating file: ' + error.message, 'danger');
            } finally {
                if (validationResult.validRowsCount > 0) {
                    nextBtn.disabled = false;
                    nextBtn.innerHTML = 'Next';
                }
                else {
                    nextBtn.disabled = true;
                    nextBtn.innerHTML = 'Next';
                }
            }

        } else if (currentStep === 2) {
            if (validationResult.validRowsCount > 0) {
                updateReviewStep(validationResult);
                showStep(3);
            }
        }
    }

    function handleBack() {
        if (currentStep === 2) {
            showStep(1);
            nextBtn.disabled = false;
            nextBtn.innerHTML = 'Next';
        } else if (currentStep === 3) {
            if (validationResult.invalidRowsCount === 0) {
                showStep(1);
            } else {
                showStep(2);
            }
        }
    }

    async function handleSubmit() {
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<i class="fa fa-spinner fa-spin me-2"></i>Submitting...';

        try {
            // Simulate API submission
            var response = await submitContactsWithAPI(validationResult);
            if (response) {
                if (response.isSuccess) {
                    showAlert(response.message, 'success');

                    // Close modal after success
                    setTimeout(() => {
                        $(bulkUploadModal).modal('hide');
                        loadContacts();
                    }, 2000);
                } else {
                    showAlert(response.message, 'error');
                }
            }
            else {
                showAlert('Operation failed', 'error');
            }
        } catch (error) {
            showAlert('Error submitting contacts: ' + error.message, 'danger');
            submitBtn.disabled = false;
            submitBtn.innerHTML = 'Submit';
        }
    }

    function showStep(step) {
        // Hide all steps
        step1.classList.add('d-none');
        step2.classList.add('d-none');
        step3.classList.add('d-none');

        // Show current step
        if (step === 1) {
            step1.classList.remove('d-none');
            backBtn.classList.add('d-none');
            nextBtn.classList.remove('d-none');
            submitBtn.classList.add('d-none');
        } else if (step === 2) {
            step2.classList.remove('d-none');
            backBtn.classList.remove('d-none');
            nextBtn.classList.remove('d-none');
            submitBtn.classList.add('d-none');
        } else if (step === 3) {
            step3.classList.remove('d-none');
            backBtn.classList.remove('d-none');
            nextBtn.classList.add('d-none');
            submitBtn.classList.remove('d-none');
        }

        // Update step indicator
        updateStepIndicator(step);
        currentStep = step;
    }

    function updateStepIndicator(step) {
        steps.forEach((s, index) => {
            s.classList.remove('active', 'completed');
            if (index + 1 < step) {
                s.classList.add('completed');
            } else if (index + 1 === step) {
                s.classList.add('active');
            }
        });
    }

    function updateValidationResults(result) {
        document.getElementById('totalRows').textContent = result.totalRows;
        document.getElementById('validRows').textContent = result.validRowsCount;
        document.getElementById('invalidRows').textContent = result.invalidRowsCount;

        const invalidRowsTable = document.getElementById('invalidRowsTable');
        invalidRowsTable.innerHTML = '';

        if (result.invalidRows && result.invalidRows.length > 0) {
            result.invalidRows.forEach(row => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${row.rowNumber}</td>
                    <td>${row.firstName || '-'}</td>
                    <td>${row.lastName || '-'}</td>
                    <td>${row.phoneNumber || '-'}</td>
                    <td>
                        ${row.errors.map(err => `<span class="badge bg-danger error-badge">${err}</span>`).join('')}
                    </td>
                `;
                invalidRowsTable.appendChild(tr);
            });
            document.getElementById('invalidRowsSection').classList.remove('d-none');
        } else {
            document.getElementById('invalidRowsSection').classList.add('d-none');
        }
    }

    function updateReviewStep(result) {
        document.getElementById('reviewBatchId').textContent = result.batchId;
        document.getElementById('reviewCountry').textContent = bulkCountry.options[bulkCountry.selectedIndex].text;
        document.getElementById('reviewLanguage').textContent = bulkLanguage.options[bulkLanguage.selectedIndex].text;
        document.getElementById('reviewContactSource').textContent = document.getElementById('bulkContactSource').options[document.getElementById('bulkContactSource').selectedIndex].text;
        document.getElementById('reviewChannelPreference').textContent = document.getElementById('bulkChannelPreference').options[document.getElementById('bulkChannelPreference').selectedIndex].text;
        document.getElementById('reviewFileName').textContent = selectedFile.name;

        document.getElementById('reviewTotalRows').textContent = result.totalRows;
        document.getElementById('reviewValidRows').textContent = result.validRowsCount;
        document.getElementById('reviewSkippedRows').textContent = result.invalidRowsCount;
        document.getElementById('confirmValidRows').textContent = result.validRowsCount;
    }

    function resetModal() {
        currentStep = 1;
        selectedFile = null;
        validationResult = null;

        // Reset form
        bulkCountry.value = '';
        bulkLanguage.innerHTML = '<option selected disabled>Select Language</option>';
        document.getElementById('bulkContactSource').value = '';
        document.getElementById('bulkChannelPreference').value = '';
        removeFile();

        // Reset steps
        showStep(1);
    }

    async function validateFileWithAPI() {
        const countryId = document.getElementById('bulkCountry').value;
        const languageId = document.getElementById('bulkLanguage').value;
        const contactSource = document.getElementById('bulkContactSource').value;
        const channelPreference = document.getElementById('bulkChannelPreference').value;

        const formData = new FormData();
        formData.append('CountryId', countryId);
        formData.append('LanguageId', languageId);
        formData.append('ContactSource', contactSource);
        formData.append('ChannelPreference', channelPreference);
        formData.append('UploadedFile', selectedFile);

        try {
            const response = await fetch('/Contact/GetBulkUploadValidations', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (!response.ok) {
                // Handle validation errors from server
                if (result.errors && Array.isArray(result.errors)) {
                    const errorMessage = result.errors.join(', ');
                    throw new Error(`Server validation failed: ${errorMessage}`);
                }
                throw new Error(result.message || 'Unknown server error');
            }

            return result;

        } catch (error) {
            console.error('API call failed:', error);
            throw error;
        }
    }

    async function submitContactsWithAPI(validationData) {
        return await actionHelper.postJson('/Contact/SaveBulkUploadContacts', validationData);
    }

    function showAlert(message, type) {
        if (typeof toastr !== 'undefined') {
            toastr[type === 'danger' ? 'error' : type](message);
        } else {
            toastr.error(message);
        }
    }

});

async function onEditContact(thisVal) {
    var contactId = thisVal.attributes.rel.value;

    resetEmployeeModal();

    const response = await actionHelper.get(`Contact/GetContactById?contactId=${encodeURIComponent(contactId)}`);
    var contact = response.data;
    if (response.success == true) {
        document.getElementById("Contact_Id").value = contact.id;
        document.getElementById("Contact_FirstName").value = contact.firstName;
        document.getElementById("Contact_MiddleName").value = contact.middleName;
        document.getElementById("Contact_LastName").value = contact.lastName;
        var country = document.getElementById("CountryDropdown");
        country.value = contact.countryId;
        loadLanguages(country, 'singleCreate').then(ddl => {
            document.getElementById("singleLanguage").value = contact.languageId;
        });

        var phoneInput = document.getElementById("Contact_PhoneNumber");
        phoneInput.value = contact.phoneNumber;
        phoneInput.hidden = true;

        var emailInput = document.getElementById("Contact_Email");
        emailInput.value = contact.email;
        emailInput.hidden = true;

        document.getElementById("Contact_Company").value = contact.company;
        document.getElementById("Contact_JobTitle").value = contact.jobTitle;
        document.getElementById("Contact_ContactSource").value = contact.contactSource;
        document.getElementById("Contact_ChannelPreference").value = contact.channelPreference;
        document.getElementById("singleLanguage").value = contact.languageId;

        document.querySelector('#newEmployeeModal .modal-title').innerHTML = 'Update contact';
        document.getElementById("saveContact").innerHTML = 'Update';

        document.getElementById("RequestType").value = "Update";

        showModal();
    }
}

function onDeleteContact(thisVal) {
    var contactId = thisVal.attributes.rel.value;
    if (contactId == null || contactId == '' || contactId == undefined || contactId == 0) {
        return;
    }

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success",
            cancelButton: "btn btn-danger"
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to view this record!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    })
    .then((willDelete) => {
        if (willDelete) {
            alert('yes')
            }
    });
}

function resetEmployeeModal() {
    const modal = document.getElementById("newEmployeeModal");

    // Reset the form fields
    const form = document.getElementById("createForm");
    form.reset();

    // Clear validation messages (ASP.NET Core spans)
    modal.querySelectorAll("span.text-danger").forEach(span => span.textContent = "");

    // Reset modal title & button text to defaults
    modal.querySelector(".modal-title").innerHTML = "Create new contact";
    document.getElementById("saveContact").innerHTML = "Save";

    // Reset dropdowns (Country & Language)
    const country = document.getElementById("CountryDropdown");
    country.value = "-1";
    country.dispatchEvent(new Event("change"));
}

function deleteDataById(id) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to view this record!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        buttons: ["Cancle", "Delete"]
    })
        .then((willDelete) => {
            if (willDelete) {
                $("#loader").addClass("loaderNew");
                $.ajax({
                    url: "DeleteLeadsById",
                    method: "Post",
                    dataType: "json",
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    data: {
                        RowId: id
                    },
                    success: function (response) {
                        $("#loader").removeClass("loaderNew");
                        if (response.IsValid) {
                            swal(response.Message, {
                                icon: "success",
                                closeOnClickOutside: false,
                                closeOnEsc: false,
                                allowOutsideClick: false
                            });
                            $('.swal-button.swal-button--confirm').on('click', function () {
                                window.location.reload();
                            })
                        }
                    },
                    error: function (response) {
                        $("#loader").removeClass("loaderNew");
                        toastr.error("Something went wrong", " Please try again later !");
                    }
                });
            }
        });
}