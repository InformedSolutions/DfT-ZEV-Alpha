$(function() { 
    var link = document.getElementById("back");
    link.className = 'govuk-back-link';
    link.style.display = 'inline-block';
    link.addEventListener('click', (event) => {
        event.preventDefault();
        window.history.back();
    }, false);
});