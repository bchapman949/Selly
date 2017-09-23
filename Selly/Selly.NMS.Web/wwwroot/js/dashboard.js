var hubConnection = $.connection.homeHub;
hubConnection.client.messageReceived = function (originatorUser, message) {
    var notification = new Notification(message);
};

$.connection.hub.logging = true;
$.connection.hub.start().done(function () {
    hubConnection.server.connect("");
    enableNotifications();
});

function enableNotifications() {
    // Let's check if the browser supports notifications
    if (!("Notification" in window)) {
        alert("This browser does not support desktop notification");
    }

    // Otherwise, we need to ask the user for permission
    else if (Notification.permission !== "denied") {
        Notification.requestPermission(function (permission) {
            // If the user accepts, let's create a notification
            document.getElementById("notifDiv").innerHTML = "Notifications enabled";
        });
    }
}