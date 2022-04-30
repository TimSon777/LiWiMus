function removeChatBy(userName) {
    $(`#chat-${userName}`).remove();
}

async function refreshHandler(connection) {
    $(".btn-send-to-user").click(async function () {
        const connectionId = $(this).val();
        const message = $(`textarea[id=${connectionId}]`).val();
        await connection.invoke("SendMessageToUser", connectionId, message);
        let id = $(this).attr('id').split('-', 2)[1];
        $(`#${id}`).append(`<li class="message"><p>Your: ${message}</p></li>`);
    })
    
    $(".btn-close-chat-by-consultant").click(async function() {
        const userName = $(this).val();
        await connection.invoke("CloseChatByConsultant", userName);
        removeChatBy(userName);
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
                        success: async (html) => {
                            $('#chats').prepend(html)
                            await refreshHandler(connection)
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
    
    connection.on("GetNewUserChat", async function (html) {
        $('#chats').html(html);
        await refreshHandler(connection)
    });
    
    connection.on("DeleteChat", function (userName) {
        removeChatBy(userName);
    });
    
    connection.on("CloseChatByUser", function (userName) {
        removeChatBy(userName);
        alert(userName + " close chat"); 
    });

    await connection.start();
    await connection.invoke("ConnectConsultant");
}

$(document).ready(function () {
    $('#online').click(async () => {
        await chatStartWithCOrOpen()
    })
})