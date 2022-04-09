function refreshHandler(connection) {
    $('.btn-send-to-user').click(async function () {
        const connectionId = $(this).val()
        const message = $(`textarea[id=${connectionId}]`).val()
        await connection.invoke("SendMessageToUser", connectionId, message)
    })
}

async function chatStartWithCOrOpen() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();
    
    $('#refresh-chats').click(()=>{
        $.ajax({
            method: 'GET',
            url: 'GetTextingUsersChats',
            success: (userNames) => {
                userNames.forEach(userName => {
                    const e = $(`#${userName}`)
                    if (e) {
                        $.ajax({
                            method: 'GET',
                            url: `Chat?userName=${userName}`,
                            success: (html) => {
                                $('#chats').prepend(html)
                                
                                $(`#btn-${userName}`).bind( "click" , async function () {
                                    const connectionId = $(this).val()
                                    const message = $(`textarea[id=${connectionId}]`).val()
                                    await connection.invoke("SendMessageToUser", connectionId, message)
                                });
                            },
                            error: (error) => {
                                alert(error)
                            }
                        })
                    }
                })
            }
        })
    })
    
    connection.on("SendMessageToConsultant", function (text, userName) {
        $(`#${userName}`).append(`<p>${text}</p>`)
    });

    refreshHandler(connection)
    
    await connection.start();

    await connection.invoke("ConnectConsultant");
}
$(document).ready(function () {
    $('#online').click(async () => {
        await chatStartWithCOrOpen()
    })
})