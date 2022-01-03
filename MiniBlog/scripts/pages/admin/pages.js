﻿$(function () {
    $('#modalConfirmDelete').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
    });
    $("select#selectPageType").select2({ width: '200px' });
    $("button#btnPageList").click(function () {
        var privacy = $("select#selectPageType").val();
        $("#divPages").load("/Pages/Index", { privacy: privacy }, function () {
            $('#divPages table').dataTable({
                "order": [[1, "asc"]],
                "scrollY": true
            });
        });
    });
    $("button#btnPageList").click();
    if (localStorage.getItem("PageID") !== null) {
        setTimeout(function () {
            pages.edit(localStorage.getItem("PageID"));
            localStorage.removeItem("PageID");
        }, 500);
    }
});
var pages = {
    add: function () {
        $('#modalAddPageContent').load("/Pages/New", function () {
            $('#modalAddPage').modal({
                keyboard: true
            }, 'show');
            mbAjax.bindForm(this, function () {
                helper.successForm($("#modalAddPageContent form"));
                locaton.reload();
            });
        });
    },
    edit: function (AuthorID) {
        $('#modalEditPageContent').load("/Pages/Edit/" + AuthorID, function () {
            $('#modalEditPage').modal({
                keyboard: true
            }, 'show');
            mbAjax.bindForm(this, function () {
                helper.successForm($("#modalEditPageContent form"));
                locaton.reload();
            });
        });
    },
}