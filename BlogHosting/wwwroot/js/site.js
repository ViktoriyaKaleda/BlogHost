"use strict";

var likeConnection = new signalR.HubConnectionBuilder().withUrl("/setLike").build();

likeConnection.on("setLike", function (likesNumber, postId) {
    const likeSpan = document.getElementById("likesNumber" + postId);
    if (Number(likeSpan.textContent) < Number(likesNumber)) {
        document.getElementById("likeIcon" + postId).classList.add("red");
    }
    else {
        document.getElementById("likeIcon" + postId).classList.remove("red");
    }
    likeSpan.textContent = likesNumber;
});

likeConnection.start().catch(function (err) {
    return console.error(err.toString());
});

var likeButtons = document.getElementsByClassName("likeButton");
for (let i = 0; i < likeButtons.length; i++) {
    likeButtons[i].addEventListener("click", function (event) {
        const postId = this.getAttribute('data-postId');
        likeConnection.invoke("SetLike", postId).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}


//for comments replying

var replyConnection = new signalR.HubConnectionBuilder().withUrl("/showReply").build();

replyConnection.on("showReply", function (commentId, username) {
    const commentText = document.getElementById("commentText");
    commentText.textContent = username + ", ";
    commentText.focus();
    commentText.selectionStart = commentText.textContent.length;
    document.getElementById("parentCommentId").setAttribute("value", commentId);
});

replyConnection.start().catch(function (err) {
    return console.error(err.toString());
});


var replyButtons = document.getElementsByClassName("replyButton");
for (let i = 0; i < replyButtons.length; i++) {
    replyButtons[i].addEventListener("click", function (event) {
        const parentId = this.getAttribute('data-parentId');
        const username = this.getAttribute('data-username');
        replyConnection.invoke("ShowReply", parentId, username).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}