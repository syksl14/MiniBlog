$(function () {
    $('#modalConfirmDelete').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
    });
    $('#modalConfirmDelete2').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').unbind("click");
        $(this).find('.btn-ok').click(function () {
            $.ajax({
                url: $(e.relatedTarget).data('href'),
                type: "GET",
                success: function (result) {
                    if (result.success) {
                        $('#modalConfirmDelete2').modal('hide');
                        $('#modalRevisionContent').load("/Revision/Index/" + $(e.relatedTarget).data('id') + "?page=1");
                    } 
                }
            }); 
        });
    });
    if (localStorage.getItem("ArticleID") !== null) {
        setTimeout(function () {
            articles.edit(localStorage.getItem("ArticleID"));
            localStorage.removeItem("ArticleID");
        }, 500);
    }
    if (localStorage.getItem("Action") !== null) {
        setTimeout(function () {
            if (localStorage.getItem("Action") === "New_Article") {
                localStorage.removeItem("Action");
                articles.add();
            }
        }, 500);
    }
});
var articles = {
    add: function () {
        $('#modalAddArticle').modal('show');
    },
    edit: function (ArticleID) {
        $('#modalEditArticleContent').load("/Article/Edit/" + ArticleID, function () {
            $('#modalEditArticle').modal({
                keyboard: true
            }, 'show');
            articles.bindForm(this);
        });
    },
    bindForm: function (dialog) {
        $('form', dialog).submit(function () {
            var data = null;
            data.append($(this).serialize());
            data.append($(this)[0].files);  
            return false;
        });
    }
}
var revisions = {
    view: function (RevisionID) {
        $('#modalViewRevisionContent').load("/Revision/View/" + RevisionID, function () {
            $('#modalViewRevision').modal({
                keyboard: true
            }, 'show');
            articles.bindForm(this);
        });
    },
    list: function (page, ArticleID) {
        $('#modalRevisionContent').load("/Revision/Index/" + ArticleID + "?page=" + page, function () {
            $('#modalRevision').modal({
                keyboard: true
            }, 'show');
            articles.bindForm(this);
        });
    },
    page: function (page, ArticleID) {
        $('#modalRevisionContent').load("/Revision/Index/" + ArticleID + "?page=" + page);
    }
}