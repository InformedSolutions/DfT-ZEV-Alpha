$(function () {
    var link = document.getElementsByClassName("js-cookie-banner-hide--accept")[0];
    if (link != null) {
        link.addEventListener('click', (event) => {
            var cookieBanner = document.getElementsByClassName("govuk-cookie-banner")[0];
            cookieBanner.hidden = true;
        }, false);
    }
});