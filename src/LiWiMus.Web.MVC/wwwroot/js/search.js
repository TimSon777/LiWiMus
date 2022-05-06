$(document).ready(() => {
    const form = $('#form-search');
    $(form).submit((e) => {
        e.preventDefault();
        const formData = new FormData(e.target);
        const params = new URLSearchParams(formData);
        const url = $(form).attr('action') + '?' + params.toString();
        $.get({
            url: url,
            success: (html) => {
                $('#search-items').append(html);
                const inputPageSearch = $('#input-page-search');
                const currentNumberPage = parseInt(inputPageSearch.val())
                inputPageSearch.val(currentNumberPage + 1)
            }
        });
    });
});