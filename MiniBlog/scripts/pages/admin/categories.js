$(function () {
    $('#modalConfirmDelete').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
    });
});
var categories = {
    add: function () {
        $('#modalAddCategory').modal('show');
    },
    edit: function (CategoryID) {
        $('#modalEditCategoryContent').load("/Category/Edit/" + CategoryID, function () {
            $('#modalEditCategory').modal({
                keyboard: true
            }, 'show');
            categories.bindForm(this);
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
                        $('#modalEditCategory').modal('hide');
                        //Refresh
                        location.reload();
                    } else {
                        $('#modalEditCategoryContent').html(result);
                        bindForm(dialog);
                    }
                }
            });
            return false;
        });
    }
}