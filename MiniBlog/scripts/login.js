var login = {
    submit: function () {
        var ajaxRequest = $.ajax({
            type: "POST",
            url: "/Admin/Index",
            async: true,
            contentType: false,
            processData: true,
            contentType: "application/json",
            data: JSON.stringify({
                Email: $("input#inputEmail").val(),
                Password: $("input#inputPassword").val()
            })
        });
        ajaxRequest.done(function (responseData, textStatus) {
            console.log(responseData);
        });
    }
}