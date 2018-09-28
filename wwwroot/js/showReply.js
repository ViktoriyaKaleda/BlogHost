"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/showReply").build();

connection.on("showReply", function (commentId, username) {
    const commentText = document.getElementById("commentText");
    commentText.textContent = username + ", ";
    commentText.focus();
    commentText.selectionStart = commentText.textContent.length;
    document.getElementById("parentCommentId").setAttribute("value", commentId);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("replyButton").addEventListener("click", function (event) {
    const replyButton = document.getElementById("replyButton");
    const parentId = replyButton.getAttribute('data-parentId');
    const username = replyButton.getAttribute('data-username');
    console.log(parentId);
    connection.invoke("ShowReply", parentId, username).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});