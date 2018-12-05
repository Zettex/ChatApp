$(function () {

    let btnToggle = $(".toggle-btn");
    if (btnToggle.length) {
        btnToggle.click(function () {
            $("#sidebar").toggleClass("active");
            $(".toggle-btn").toggleClass("active");
            $(".layout").fadeToggle(200);
        });
    }

    $(".layout").click(function (e) {
        $("#sidebar").toggleClass("active");
        $(".toggle-btn").toggleClass("active");
        $(".layout").fadeToggle(200);
    });

    if ($("#messages > ul").length) {
        $("#messages > ul").scrollTop($("#messages > ul")[0].scrollHeight);
    }

    // ChatHub class proxy
    let chat = $.connection.chatHub;
    let audio = new Audio('/Content/ChatContent/Sounds/msg_get.mp3');
    audio.volume = 0.4;

    chat.client.addMessage = function (msg) {
        $.ajax({
            url: "/GetMessageView",
            method: "POST",
            data: { message: msg },
            success: function (view) {
                $("div.room-info").remove();

                $("#messages > ul").append(view);
                setTimeout(function () {
                    $("#messages > ul li").last().removeClass("new");
                }, 1);
                $("#messages > ul").scrollTop($("#messages > ul")[0].scrollHeight);
                $(".del-msg", $("#messages > ul li").last()).click(deleteMessage);

                audio.play();
            }
        });
    };

    chat.client.deleteMessage = function (msgId) {
        $("#msg_" + msgId).parent().remove();
    };

    chat.client.addRoom = function (room) {
        $("#room-list").append(room);
        setTimeout(function () {
            let lastLi = $("#room-list li").last();
            $(lastLi).slideDown(200);
            $(lastLi).removeClass("new");
        }, 1);

        $("#room-list").scrollTop($("#room-list")[0].scrollHeight);
    };

    //let lastLisTyping = [];
    //let intId = 0;
    //// If the user press enter key, stop typing process
    //let stop = false;
    //chat.client.typing = function (result, id, stop) {

    //    let isExist = false;
    //    lastLisTyping.forEach(function (el) {
    //        if (el.id === id) {
    //            isExist = true;
    //            el.delay++;

    //            if (stop) {
    //                stop = false;
    //                $(el.li).remove();
    //                //clearInterval(intId);
    //                lastLisTyping.splice(lastLisTyping.indexOf(el), 1);
    //            }
    //        }
    //    });

    //    if (isExist)
    //        return;

    //    $("#messages > ul").append(result);

    //    lastLisTyping.push({
    //        id: id,
    //        li: $("#" + id),
    //        delay: 3,
    //        count: 0
    //    });

    //    setTimeout(function () {
    //        $("#" + id).removeClass("new");
    //    }, 1);

    //    let intId = setInterval(function () {
    //        lastLisTyping.forEach(function (el) {
    //            if (el.id === id) {
    //                el.count++;
    //                if (el.count >= el.delay || stop) {
    //                    stop = false;
    //                    $(el.li).remove();
    //                    clearInterval(intId); // maybe move this up?
    //                    lastLisTyping.splice(lastLisTyping.indexOf(el), 1);
    //                }
    //            }
    //        });
    //    }, 1000);

    //    $("#messages > ul").scrollTop($("#messages > ul")[0].scrollHeight);
    //};

    // Open connection
    $.connection.hub.start().done(function () {
        // Join room
        let roomId = $("#RoomId").val();
        if (roomId != null) {
            chat.server.joinRoom(roomId);
        }

        // Leave if change room
        $(".room > a").click(function () {
            //TODO: AJAX change room
            if (roomId != null) {
                chat.server.leaveRoom(roomId);
            }
        });

        // Send to server to save msg in db
        $("#send").click(function (e) {
            let msg = $("#msg-input");
            if (msg.val().trim() !== '') {
                $(msg).val(msg.val().replace(/[ \t]+/g, ' ').trim());
                $("#msg-form").submit();
                $(msg).val('');
            }
        });

        // Send msg if press enter
        $("#msg-input").keypress(function (e) {
            if (e.keyCode == 13 && !e.shiftKey) {
                if ($(this).val().trim() !== '') {
                    $(this).val($(this).val().replace(/[ \t]+/g, ' ').trim());
                    $("#msg-form").submit();
                    $(this).val('');
                    //typing(true);
                    return false;
                }
            }

            //typing();
        });

        //function typing(stop = false) {

        //    chat.invoke('getConnectionId')
        //        .then(function (connectionId) {
        //            $.ajax({
        //                url: "/Typing",
        //                method: "POST",
        //                data: {
        //                    roomId: $("#RoomId").val(),
        //                    id: connectionId,
        //                    stopTyping: stop
        //                }
        //            });
        //        });

        //}

        // Add room 
        $("#add-room").click(function (e) {
            e.preventDefault();
            let room = $("#room-input");
            if (room.val().trim() !== '') {
                $(room).val(room.val().replace(/[ \t]+/g, ' ').trim());
                $("#room-form").submit();
                $(room).val('');
            }
        });

    });

    // Manage page
    $(".table td a.ren").click(function (e) {
        $("#Id").val($(this).attr("id"));
        $("#Name").val($(this).parent().siblings().html().trim());
        $("#room-rename-form").fadeIn(400);
    });

    $("#cancel").click(function (e) {
        $("#room-rename-form").fadeOut(400);
    });

    $(".table td a.del").click(function (e) {
        if (!confirm("Do you really want to delete this room?")) {
            e.preventDefault();
        }
    });

    function deleteMessage() {
        if (confirm("Do you really want to delete this message?")) {
            $.ajax({
                url: "/DeleteMessage",
                method: "POST",
                data: {
                    messageId: $(this).attr("id").split("_")[1],
                    roomId: $("#RoomId").val()
                }
            });
        }
    }

    $("span.del-msg").click(deleteMessage);
});