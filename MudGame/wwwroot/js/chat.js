"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}: ${message}`;
    messagesList.scrollTop = messagesList.scrollHeight;
});

connection.start().then (function () {
    document.getElementById("sendButton").disabled = false;
    var name = document.getElementById("characterName").textContent;
    var characterId = document.getElementById("characterId").value;
    var roomId = document.getElementById("roomId").value;
    connection.invoke("SendMessage", "System", `${name} connected to game.`);
    connection.invoke("SendGameCommand", characterId, "/move", roomId);
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    if (message[0] == "/") {
        var characterId = document.getElementById("characterId").value;
        var roomId = document.getElementById("roomId").value;
        connection.invoke("SendGameCommand", characterId, message, roomId).catch(function (err) {
            return console.error(err.toString());
        })
    } else if (message) {
        var name = document.getElementById("characterName").textContent;
        connection.invoke("SendMessage", name, message).catch(function (err) {
        return console.error(err.toString());
    })
    } else {
        console.warn("Cannot send an empty message.");
    }
    event.preventDefault();
});