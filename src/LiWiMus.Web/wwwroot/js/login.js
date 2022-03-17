function sendEmailResetPassword() {
    $('#btn-send-email-reset-password').click(() => {
        const userName = $('#userName').val()
        if (!userName || userName === "") {
            alert('Заполните имя пользователя') //to do
        } else {
            $.get({
                url: `/Account/ResetPassword?userName=${userName}`,
                success: (response) => {
                    alert(response)  //to do
                },
                error: (response) => {
                    alert(response.responseText)  //to do (Так же переписать сообщения на бэке в /Account/ResetPassword)
                }
            })
        }
    })
}

$(document).ready(() => {
    sendEmailResetPassword()
})