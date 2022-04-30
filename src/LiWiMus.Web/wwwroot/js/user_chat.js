$(document).ready(async function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();
    
    connection.on("SendMessageToUser", function (text) {
        $('#chat').append(`<p>${text}</p>`)
    });

    $('#btn-send-to-consultant').click(async () => {
        const text = $('#ta-send-to-consultant').val()
        await connection.invoke("SendMessageToConsultant", text)
        $('#chat').append(`<p>I:${text}</p>`)
    })

    $('#btn-close-chat-by-user').click(async () => {
        await connection.invoke("CloseChatByUser")
    })
    
    await connection.start();
    await connection.invoke("ConnectUser")
        .then(html => $('#chat').html(html))
})