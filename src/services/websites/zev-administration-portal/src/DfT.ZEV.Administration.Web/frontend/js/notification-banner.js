class Notification {
    constructor(notification) {
        this.notification = notification;
        if (this.notification) {
            this.notificationClose = notification.querySelector('.js-close-notification');
        }
    }

    init() {
        if (this.notificationClose) {
            this.notificationClose.addEventListener('click', () => {
                this.notification.parentNode.removeChild(this.notification);
            });
        }
    }
}

$(function () {
    var cookieConfirmNotification = new Notification(
        document.getElementById("cookie-confirm")
    );
    cookieConfirmNotification.init();
});