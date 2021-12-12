$(function () {
    $('#modalConfirmDelete').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
    });
});
var users = {
    add: function () {
        $('#modalAddUser').modal('show');
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
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
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