async function chatStartOrOpen() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();


    connection.on("SendMessageToUser", function (text) {
        $('#chat').append(`<p>${text}</p>`)
    });

    $('#btn-send-to-consultant').click(async () => {
        await connection.invoke("SendMessageToConsultant", $('#ta-send-to-consultant').val())
    })

    await connection.start();

    await connection.invoke("ConnectUser")
}
$(document).ready(function () {
    $('#user-chat-start-or-open').click(async () => {
        await chatStartOrOpen()
    })
})