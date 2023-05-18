//create connection
var connectionNotification = new signalR.HubConnectionBuilder().withUrl("/hubs/notificationHub").build();

document.getElementById("sendButton").disabled = true;

connectionNotification.on("LoadNotification", function (message, counter){
    document.getElementById("messageList").innerHTML = "";
    var notificationCounter = document.getElementById("notificationCounter");
    notificationCounter.innerHTML = "<span>(" + counter + ")</span>"; 
    for (var i = message.length-1; i >=0 ; i--) {
        var li = document.createElement("li")
        li.textContent = "Notification - " + message[i]
        document.getElementById("messageList").appendChild(li)
    }
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    let message = document.getElementById("notificationInput").value;
    connectionNotification.send("SendMessage", message).then(function (){
        document.getElementById("notificationInput").value = "";
    })
    event.preventDefault();
})


//start connection
function fulfilled() {
    let show = function () {
        connectionNotification.send("LoadMessages")
        document.getElementById("sendButton").disabled = false;
    }
    show();
}

function rejected() {
    //rejected logs
}

//connectionNotification.start().then(function () {
//    connectionNotification.send("LoadMessages")
//    document.getElementById("sendButton").disabled = false;
//});

connectionNotification.start().then(fulfilled, rejected);