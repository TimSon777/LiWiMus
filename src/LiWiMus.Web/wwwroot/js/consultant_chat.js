function refreshHandler(btn, connection) {
    $(btn).click(async function () {
        const connectionId = $(this).val();
        const message = $(`textarea[id=${connectionId}]`).val();
        await connection.invoke("SendMessageToUser", connectionId, message);
        let id = $(this).attr('id').split('-', 2)[1];
        $(`#${id}`).append(`<li class="message"><p>Your: ${message}</p></li>`);
    })
}

async function chatStartWithCOrOpen() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    $('#offline').click(async () => {
        await connection.invoke("DisconnectConsultant")
    })

    $('#refresh-chats').click(() => {
        $.ajax({
            method: 'GET',
            url: 'GetTextingUsersChats',
            success: (userNames) => {
                $('#chats').empty()
                userNames.forEach(userName => {
                    $.ajax({
                        method: 'GET',
                        url: `Chat?userName=${userName}`,
                        success: (html) => {
                            $('#chats').prepend(html)
                            refreshHandler($(`#btn-${userName}`), connection)
                        },
                        error: (error) => {
                            alert(error)
                        }
                    })
                })
            },
            error: (error) => {
                alert(error)
            }
        })
    })

    connection.on("SendMessageToConsultant", function (text, userName) {
        $(`#${userName}`).append(`<li><p>${text}</p></li>`)
    });

    await connection.start();
    await connection.invoke("ConnectConsultant");
}

$(document).ready(function () {
    $('#online').click(async () => {
        await chatStartWithCOrOpen()
    })
})