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
        var dialog = $("#modalAddUser");
        $('#modalAddUser').modal('show');
        $('form', dialog).submit(function () {
            var formData = new FormData(this);
            $.ajax({
                url: this.action,
                type: this.method,
                data: formData,
                contentType: false,
                processData: false,
                cache: false,
                success: function (result) {
                    if (result.success) {
                        $('#modalAddUser').modal('hide');
                        //Refresh
                        location.reload();
                    } else {
                        $('#modalAddUser .modal-body').html(result);
                    }
                }
            });
            return false;
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
        $('form', dialog).submit(function () {
            var formData = new FormData(this);
            $.ajax({
                url: this.action,
                type: this.method,
                data: formData,
                contentType: false,
                processData: false,
                cache: false,
                success: function (result) {
                    if (result.success) {
                        $('#modalEditUser').modal('hide');
                        //Refresh
                        location.reload();
                    } else {
                        $('#modalEditUserContent').html(result);
                        bindForm(dialog);
                    }
                }
            });
            return false;
        });
    }
}