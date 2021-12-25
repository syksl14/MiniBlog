$(function () {

    $("#contactForm input,#contactForm textarea").jqBootstrapValidation({
        preventSubmit: true,
        submitError: function ($form, event, errors) {
        },
        submitSuccess: function ($form, event) {
            $("#contactForm button[type='submit']").html("Lütfen bekleyin...").attr("disabled", "disabled");
            event.preventDefault();
            var name = $("input#name").val();
            var email = $("input#email").val();
            var message = $("textarea#message").val();
            var key = $("input#response_key").val();
            var firstName = name;
            if (firstName.indexOf(' ') >= 0) {
                firstName = name.split(' ').slice(0, -1).join(' ');
            }
            $.ajax({
                url: "/Home/ContactSend",
                type: "POST",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    name: name,
                    email: email,
                    message: message,
                    key: key
                }),
                async: true,
                cache: false,
                success: function (responseData) {
                    if (responseData.Result === "Error") {
                        $('#success').html("<div class='alert alert-danger'>");
                        $('#success > .alert-danger').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                            .append("</button>");
                        $('#success > .alert-danger').append("<strong>Üzgünüm " + firstName + ", Posta sunucum yanıt vermiyor gibi görünüyor. Lütfen daha sonra tekrar deneyiniz!");
                        $('#success > .alert-danger').append('</div>');
                        $('#contactForm').trigger("reset");
                    } else if (responseData.Result === "reCaptcha_VerifyError") {
                        $('#success').html("<div class='alert alert-danger'>");
                        $('#success > .alert-danger').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                            .append("</button>");
                        $('#success > .alert-danger').append("<strong>" + responseData.Error);
                        $('#success > .alert-danger').append('</div>');
                    } else {
                        $('#success').html("<div class='alert alert-success'>");
                        $('#success > .alert-success').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                            .append("</button>");
                        $('#success > .alert-success')
                            .append("<strong>Mesajınız gönderilmiştir. </strong>");
                        $('#success > .alert-success')
                            .append('</div>'); 
                        $('#contactForm').trigger("reset");
                    }
                    $("#contactForm button[type='submit']").html("Gönder").removeAttr("disabled");
                    if ($("#g-recaptcha").size() > 0) {
                        grecaptcha.reset();
                    }
                },
                error: function () {
                    $('#success').html("<div class='alert alert-danger'>");
                    $('#success > .alert-danger').html("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;")
                        .append("</button>");
                    $('#success > .alert-danger').append("<strong>Üzgünüm " + firstName + ", Posta sunucum yanıt vermiyor gibi görünüyor. Lütfen daha sonra tekrar deneyiniz!");
                    $('#success > .alert-danger').append('</div>');
                    $('#contactForm').trigger("reset");
                },
            });
        },
        filter: function () {
            return $(this).is(":visible");
        },
    });

    $("a[data-toggle=\"tab\"]").click(function (e) {
        e.preventDefault();
        $(this).tab("show");
    });
});

$('#name').focus(function () {
    $('#success').html('');
});
