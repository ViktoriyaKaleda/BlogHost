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


const buttons = document.getElementsByClassName("replyButton");
console.log(buttons.length);
for (let i = 0; i < buttons.length; i++)
{
    buttons[i].addEventListener("click", function (event) {
        const parentId = this.getAttribute('data-parentId');
        const username = this.getAttribute('data-username');
        console.log(parentId);
        connection.invoke("ShowReply", parentId, username).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}
