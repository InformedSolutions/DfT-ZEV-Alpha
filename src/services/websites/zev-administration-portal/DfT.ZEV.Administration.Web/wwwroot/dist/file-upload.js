/******/ (() => { // webpackBootstrap
var __webpack_exports__ = {};
/*!***************************!*\
  !*** ./js/file-upload.js ***!
  \***************************/
//let MODE = 'upload' // 'upload' or 'submit';

/**
 * Validates the file input and returns an array of error messages.
 * @param {HTMLInputElement} fileInput - The file input element.
 * @returns {string[]} - Array of error messages.
 */
function validateFileInput(fileInput) {
    const errorMessages = [];
    const file = fileInput.files[0];

    if (!file) {
        errorMessages.push('Please select a file to upload.');
    }

    if (typeof SIZE_LIMIT !== 'undefined' && file && file.size > SIZE_LIMIT) {
        errorMessages.push('The file is too large.');
    }

    if (Array.isArray(VALID_EXTENSIONS) && VALID_EXTENSIONS.length > 0 && file) {
        const fileExtension = file.name.split('.').pop().toLowerCase();
        if (!VALID_EXTENSIONS.includes(fileExtension)) {
            errorMessages.push('The file type is not supported.');
        }
    }

    return errorMessages;
}

/**
 * Draws error messages on the page.
 * @param {string[]} errors - Array of error messages.
 */
function drawErrors(errors) {
    hideErrors();

    const errorsSummary = document.createElement('div');
    errorsSummary.setAttribute('aria-labelledby', 'error-summary-title');
    errorsSummary.setAttribute('role', 'alert');
    errorsSummary.setAttribute('tabindex', '-1');
    errorsSummary.classList.add('govuk-error-summary');
    errorsSummary.innerHTML = `
        <h2 class='govuk-error-summary__title' id='error-summary-title'>There is a problem</h2>
        <div class='govuk-error-summary__body'>
            <ul class='govuk-list govuk-error-summary__list'>
                ${errors.map(error => `<li><a href='#UploadedFiles'>${error}</a></li>`).join('')}
            </ul>
        </div>
    `;
    document.getElementById('main-content').prepend(errorsSummary);

    const formGroup = document.querySelector('.govuk-form-group');
    formGroup.classList.add('govuk-form-group--error');

    const inputErrors = document.createElement('span');
    inputErrors.classList.add('govuk-error-message');
    inputErrors.innerHTML = `
        <span class='govuk-visually-hidden'>Error</span>
        ${errors.map(error => `<div>${error}</div>`).join('')}
    `;
    document.querySelector('.govuk-form-group .govuk-file-upload').insertAdjacentElement('beforebegin', inputErrors);

    const fileUpload = document.querySelector('.govuk-file-upload');
    fileUpload.classList.add('govuk-file-upload--error');
}

/**
 * Hides error messages on the page.
 */
function hideErrors() {
    const errorSummary = document.querySelector('.govuk-error-summary');
    if (errorSummary) {
        errorSummary.remove();
    }

    const formGroup = document.querySelector('.govuk-form-group');
    formGroup.classList.remove('govuk-form-group--error');

    const errorMessage = document.querySelector('.govuk-form-group .govuk-error-message');
    if (errorMessage) {
        errorMessage.remove();
    }

    const fileUpload = document.querySelector('.govuk-file-upload');
    fileUpload.classList.remove('govuk-file-upload--error');
}


document.addEventListener('DOMContentLoaded', function () {
    // Get elements
    const uploadForm = document.querySelector('.async-upload-form');
    const uploadSubmit = document.querySelector('.async-upload-submit');
    const fileInput = document.querySelector('.async-upload-input');

    const submitFileButton = document.querySelector('.submit-file-button');
    const progressBar = document.querySelector('.async-upload-progress');
    const progressText = document.querySelector('.async-upload-progress-text');
    const uploadFileName = document.querySelector('.async-upload-file-name');
    const submtiStageContainer = document.querySelector('.submit-stage-container');
    const cancelUploadButton = document.querySelector('.cancel-file-upload');
    
    const xhr = new XMLHttpRequest();

    cancelUploadButton.addEventListener('click', function (e) {
        e.preventDefault();
        uploadForm.style.display = 'block';
        submtiStageContainer.style.display = 'none';
        progressBar.style.display = 'none';
        uploadSubmit.disabled = false;
        xhr.abort();
    });

    fileInput.addEventListener('change', function () {
        hideErrors();
        uploadFileName.innerHTML = fileInput.files[0].name;
        const errorMessages = validateFileInput(fileInput);
        if (errorMessages.length > 0) {
            drawErrors(errorMessages);
            uploadSubmit.disabled = true;
            return;
        }else {
            uploadSubmit.disabled = false;
        }
    });

    uploadSubmit.addEventListener('click', function (e) {
        e.preventDefault();

        // Disable the submit button
        uploadSubmit.disabled = true;
        uploadForm.style.display = 'none';
        submtiStageContainer.style.display = 'block';

        submitFileButton.disabled = true;
        // Create a new FormData object
        const formData = new FormData(uploadForm);

        // Show the progress bar
        progressBar.style.display = 'block';

        // Create a new XMLHttpRequest

        // Track upload progress
        xhr.upload.addEventListener('progress', function (event) {
            if (event.lengthComputable) {
                const percentComplete = (event.loaded / event.total) * 100;
                const val = Math.min(percentComplete, 90);
                progressBar.value = val;
                progressText.innerHTML = `Uploading ${Math.round(val)}%`;
            }
        });

        // Handle successful upload
        xhr.onload = function () {
            if (xhr.status === 200) {
                document.querySelector('.upload-status-container').style.display = 'none';
                document.querySelector('.uploaded-status').style.display = 'block';


                //This is UX improvement so user can see the progress bar at 100% for a short time
                setTimeout(function () {
                    //window.location.href = '/Data/upload-success';
                    submitFileButton.disabled = false;
                }, 333);
            } else {
                // Enable the submit button if the upload failed
                uploadSubmit.disabled = false;
            }
        };

        // Handle errors
        xhr.onerror = function () {
            console.error('Error during upload');
            // Enable the submit button if an error occurred
            uploadSubmit.disabled = false;
        };

        // Send the request
        xhr.open('POST', '/data/upload-file', true);
        xhr.send(formData);
    });
});

/******/ })()
;
//# sourceMappingURL=file-upload.js.map