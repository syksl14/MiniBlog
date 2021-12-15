$(function () {
    $('#modalConfirmDelete').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
    });
    $("select#selectLevels").select2({ width: '200px' });
    $("button#btnListUsers").click(function () {
        var level = $("select#selectLevels").val();
        if (level === '') {
            level = 0;
        }
        $("#divUsers").load("/User/List", { page: 1, level: level });
    });
    $("button#btnListUsers").click();
});
var users = {
    add: function () {
        $('#modalAddUserContent').load("/User/New", function () {
            $('#modalAddUser').modal({
                keyboard: true
            }, 'show');
            users.bindForm(this);
        });
    },
    edit: function (AuthorID) {
        $('#modalEditUserContent').load("/User/Edit/" + AuthorID, function () {
            $('#modalEditUser').modal({
                keyboard: true
            }, 'show');
            users.bindForm(this);
        });
    },
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

    }
}