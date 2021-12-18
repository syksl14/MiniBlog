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
            mbAjax.bindForm(this);
        });
    },
    edit: function (AuthorID) {
        $('#modalEditUserContent').load("/User/Edit/" + AuthorID, function () {
            $('#modalEditUser').modal({
                keyboard: true
            }, 'show');
            mbAjax.bindForm(this);
        });
    }
}