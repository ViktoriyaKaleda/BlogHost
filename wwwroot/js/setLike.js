"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("setLike", function (likesNumber) {
    document.getElementById("likesNumber").textContent = likesNumber;
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("likeButton").addEventListener("click", function (event) {
    const likeButton = document.getElementById("likeButton");
    const postId = likeButton.getAttribute('data-postId');
    console.log(postId);
    connection.invoke("SetLike", postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});