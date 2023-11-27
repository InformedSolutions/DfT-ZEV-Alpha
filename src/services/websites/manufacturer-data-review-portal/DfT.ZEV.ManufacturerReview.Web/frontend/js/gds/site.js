import '../notification-banner';
import './hide-cookie-notification';
import GOVUKFrontend from './govuk-frontend-3.9.1.min.js';
import $ from 'jquery';

global.$ = $;

GOVUKFrontend.initAll();

$(document).ready(function(){
    if($(".govuk-accordion__section--expanded")) {
        // Note this is necessary as session state is managed internally by the GOV.UK library for the accordion component
        $(".govuk-accordion__open-all").click();
        $(".govuk-accordion__open-all").click();
    }
});
