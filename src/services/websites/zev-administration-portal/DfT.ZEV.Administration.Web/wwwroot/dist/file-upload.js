/******/ (() => { // webpackBootstrap
var __webpack_exports__ = {};
/*!***************************!*\
  !*** ./js/file-upload.js ***!
  \***************************/
document.addEventListener('DOMContentLoaded', function () {
    // Get elements
    var uploadForm = document.querySelector('.async-upload-form');
    var uploadSubmit = document.querySelector('.async-upload-submit');
    var progressBar = document.querySelector('.async-upload-progress');
    var fileInput = document.querySelector('.async-upload-input');

    function validateFileInput(fileInput) {
        var errorMessages = [];

        // Get the file
        var file = fileInput.files[0];
       
        // Validate the file
        if (!file) {
            errorMessages.push('Please select a file to upload.');
        }

        // Check the file size
        if (typeof SIZE_LIMIT !== 'undefined' && file && file.size > SIZE_LIMIT) {
            errorMessages.push('The file is too large.');
        }

        // Check the file type
        if (Array.isArray(VALID_EXTENSIONS) && VALID_EXTENSIONS.length > 0 && file) {
            var fileExtension = file.name.split('.').at(-1).toLowerCase();
            if (VALID_EXTENSIONS.indexOf(fileExtension) === -1) {
                errorMessages.push('The file type is not supported.');
            }
        }

        return errorMessages;
    }

    function drawErrors(errors) {
        hideErrors();
        var errorsSummary = document.createElement('div');
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

        document.querySelector('.govuk-form-group').classList.add('govuk-form-group--error');

        var inputErrors = document.createElement('span');
        inputErrors.classList.add('govuk-error-message');
        inputErrors.innerHTML = `
            <span class='govuk-visually-hidden'>Error</span>
            ${errors.map(error => `<div>${error}</div>`).join('')}
        `;
        document.querySelector('.govuk-form-group .govuk-file-upload').insertAdjacentElement('beforebegin', inputErrors);

        document.querySelector('.govuk-file-upload').classList.add('govuk-file-upload--error');

        uploadSubmit.disabled = true;
    }

    function hideErrors() {
        var errorSummary = document.querySelector('.govuk-error-summary');
        if (errorSummary) {
            errorSummary.remove();
        }

        var formGroup = document.querySelector('.govuk-form-group');
        if (formGroup) {
            formGroup.classList.remove('govuk-form-group--error');
        }

        var errorMessage = document.querySelector('.govuk-form-group .govuk-error-message');
        if (errorMessage) {
            errorMessage.remove();
        }

        var fileUpload = document.querySelector('.govuk-file-upload');
        if (fileUpload) {
            fileUpload.classList.remove('govuk-file-upload--error');
        }

        uploadSubmit.disabled = false;
    }

    fileInput.addEventListener('change', function () {
        hideErrors();
        var errorMessages = validateFileInput(fileInput);
        if(errorMessages.length > 0){
            drawErrors(errorMessages);
            return;
        }
    });

    uploadSubmit.addEventListener('click', function (e) {
        e.preventDefault();

        // Disable the submit button
        uploadSubmit.disabled = true;
       

        // Create a new FormData object
        var formData = new FormData(uploadForm);

        // Show the progress bar
        progressBar.style.display = 'block';

        // Create a new XMLHttpRequest
        var xhr = new XMLHttpRequest();

        // Track upload progress
        xhr.upload.addEventListener('progress', function (event) {
            if (event.lengthComputable) {
                var percentComplete = (event.loaded / event.total) * 100;
                percentComplete = Math.min(percentComplete, 90);
                progressBar.value = percentComplete;
            }
        });

        // Handle successful upload
        xhr.onload = function () {
            if (xhr.status === 200) {
                progressBar.value = 100;

                setTimeout(function () {
                    window.location.href = '/Data/upload-success';
                }, 200);
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