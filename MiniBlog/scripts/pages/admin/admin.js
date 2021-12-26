$(function () {
    $.extend($.fn.dataTable.defaults, {
        language: { url: "/Scripts/plugins/DataTables/langs/Turkish.json" }
    });
    jQuery.extend(jQuery.validator.messages, {
        required: "Bu alan gereklidir.",
        remote: "Bu alanı düzeltin lütfen.",
        email: "Geçerli bir e-posta adresi girin.",
        url: "Lütfen geçerli bir adres girin.",
        date: "Please enter a valid date.",
        dateISO: "Lütfen geçerli bir tarih giriniz.",
        number: "Lütfen geçerli bir numara girin.",
        digits: "Lütfen sadece rakam giriniz.",
        equalTo: "Lütfen aynı değeri tekrar giriniz.",
        accept: "Lütfen geçerli bir uzantıya sahip bir değer girin.",
        maxlength: jQuery.validator.format("Lütfen en fazla {0} karakter girin."),
        minlength: jQuery.validator.format("Lütfen en az {0} karakter girin."),
        rangelength: jQuery.validator.format("Lütfen {0} ile {1} karakter uzunluğunda bir değer girin."),
        range: jQuery.validator.format("Lütfen {0} ile {1} arasında bir değer girin."),
        max: jQuery.validator.format("Lütfen {0} değerine eşit veya daha küçük bir değer girin."),
        min: jQuery.validator.format("Lütfen {0}'e eşit veya daha büyük bir değer girin.")
    });
    var $body = $("body");
    $(document).on({
        ajaxStart: function () { $body.addClass("loading"); },
        ajaxStop: function () { $body.removeClass("loading"); }
    });
    $('.modal').on('hidden.bs.modal', function () {
        if ($(".modal").hasClass('in')) {
            $('body').addClass('modal-open');
        }
    })
});
var helper = {
    successForm: function (form) {
        form.find("div.server-side-errors").remove();
        form.prepend('<div class="alert alert-success server-side-errors"><button type="button" class="close" data-dismiss="alert">×</button><i class="fa fa-check-circle"></i> Değişiklikler başarıyla kaydedildi.</div>');
        helper.setSuccessButton(form.find('button[type="submit"]'));
    },
    setSuccessButton: function (button) {
        var text = button.text();
        button.attr("disabled", "disabled").addClass("disabled");
        button.html('<i class="fa fa-check"></i> ' + text);
        setTimeout(function () {
            button.html(text).removeAttr("disabled").removeClass("disabled");
        }, 500);
    }
}
var mbAjax = {
    bindForm: function (dialog) {
        var action = $('form', dialog).attr("action");
        $('form', dialog).attr("action", "javascript:");
        $('form', dialog).submit(function () {
            if ($('form', dialog).valid()) {
                var formData = new FormData(this);
                $.ajax({
                    url: action,
                    type: this.method,
                    data: formData,
                    contentType: false,
                    processData: false,
                    cache: false,
                    success: function (result) {
                        if (result.success) {
                            $(dialog).modal('hide');
                            //Refresh
                            location.reload();
                        } else {
                            //server side error response 
                            $('form', dialog).find("div.modal-body > div.server-side-errors").remove();
                            $('form', dialog).find('div.modal-body').prepend('<div class="alert alert-danger server-side-errors"><button type="button" class="close" data-dismiss="alert">×</button> <ul style="padding-left: 15px;"></ul></div>');
                            for (var i = 0; i < result.errors.length; i++) {
                                $('form', dialog).find("div.modal-body > div.server-side-errors > ul").prepend('<li>' + result.errors[i] + '</li>');
                            }
                        }
                    }
                });
                return true;
            } else {
                return false;
            }
        });
        $('form', dialog).validate({
            submitHandler: function (form) {
                form.submit();
            }
        });

    },
    bindFormNonDialog: function (form, success) {
        var action = form.attr("action");
        form.attr("action", "javascript:");
        form.submit(function () {
            if (form.valid()) {
                var formData = new FormData(this);
                $.ajax({
                    url: action,
                    type: this.method,
                    data: formData,
                    contentType: false,
                    processData: false,
                    cache: false,
                    success: function (result) {
                        if (result.success) { 
                            //Refresh
                            success();
                        } else {
                            //server side error response 
                            form.find("div.server-side-errors").remove();
                            form.prepend('<div class="alert alert-danger server-side-errors"><button type="button" class="close" data-dismiss="alert">×</button> <ul style="padding-left: 15px;"></ul></div>');
                            for (var i = 0; i < result.errors.length; i++) {
                                form.find("div.server-side-errors > ul").prepend('<li>' + result.errors[i] + '</li>');
                            }
                        }
                    }
                });
                return true;
            } else {
                return false;
            }
        });
        form.validate({
            submitHandler: function (f) {
                f.submit();
            }
        });
    }
}