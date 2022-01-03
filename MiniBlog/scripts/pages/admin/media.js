$(function () {
    Dropzone.autoDiscover = false;
    media.fileUploadCreate();
    media.load();
});

var media = {
    dropZone: null,
    folderParentId: 0,
    folderId: 0,
    fileId: 0,
    fileUploadCreate: function () {
        $("#btnFileUpload").click(function () {
            $(".dropzone.dz-clickable").trigger('click');
        });
        media.dropZone = Dropzone.forElement("#fileupload", {
            maxFilesize: 1024,
        });
        media.dropZone.on("error", function (file, message) {
            if (message.errors != null) {
                $(file.previewElement).find('.dz-error-message').text(message.errors[0]);
            }
        });
        media.dropZone.on("success", function (file, message) {
            setTimeout(function () {
                media.dropZone.removeFile(file);
            }, 700);
        });
        media.dropZone.on("complete", function (file, message) {
            if (media.dropZone.getUploadingFiles().length === 0 && media.dropZone.getQueuedFiles().length === 0) {
                setTimeout(function () {
                    media.load(0, 0);
                }, 850);
            }
        });
        media.dropZone.on("addedfile", file => {
            var removeButton = Dropzone.createElement("<button class='btn btn-xs btn-danger'>Sil</button>");
            var _this = this;
            removeButton.addEventListener("click", function (e) {
                e.preventDefault();
                e.stopPropagation();
                _this.removeFile(file);
            });
            file.previewElement.appendChild(removeButton);

        });
        media.dropZone.on('sending', function (data, xhr, formData) {
            formData.append('FolderID', media.folderId);
            formData.append('ParentFolderID', media.folderParentId);
        });
        $('#modalConfirmDelete').on('show.bs.modal', function (e) {
            $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
        });
        $(".dropdown-preferences-menu li").click(function () {
            var _element = $(this);
            if (_element.attr("data-item") === "items-delete") {
                var _selectedItem = $(".file-item.bg-info");
                if (_selectedItem.length > 0) {
                    bootbox.confirm("Seçili olan öğeler silinecektir devam edilsin mi?", function (result) {
                        for (var i = 0; i < _selectedItem.length; i++) {
                            var item = _selectedItem[i];
                            var itemType = $(item).attr("class")
                            if (itemType.indexOf("folder-item") !== -1) {
                                itemType = "Folder";
                            } else {
                                itemType = "File";
                            }
                            var id = $(item).attr("data-id");
                            mbAjax.callAjax('GET', { action: '/Media/' + itemType + '/Delete/' + id, data: null }, function () {
                                $(item).remove();
                            }); 
                        }
                    });
                }
            } else if (_element.attr("data-item") === "new-folder") {
                media.newFolder();
            }
        });
    },
    load: function () {
        $("#divMedia > .panel-body > form > .file-manager-container").load("/Media/Index", function () {
            if (media.folderParentId > 0 || media.folderId > 0) {
                setTimeout(function () {
                    $("div.file-item[data-id='" + media.folderId + "']").find("a.file-item-name").trigger('click');
                }, 300);
            }
            $("div.file-item label.file-item-checkbox input").unbind("change").change(function () {
                var _element = $(this);
                if (this.checked) {
                    _element.parent().parent().addClass("bg-info");
                } else {
                    _element.parent().parent().removeClass("bg-info");
                }
                if ($("div.file-item.bg-info").length > 0) {
                    $("ul.dropdown-preferences-menu > li.disabled").removeClass("disabled").addClass("non-disabled");
                    $("button.dropdown-preferences").html("<i class='fa fa-gear'></i> <strong>(" + $("div.file-item.bg-info").length + ") öğe seçili</strong>");
                } else {
                    $("ul.dropdown-preferences-menu > li.non-disabled").addClass("disabled");
                    $("button.dropdown-preferences").html("<i class='fa fa-gear'></i>");
                }
            });
        });
    },
    open: function (item) {
        var parentId = $(item).parent().attr("data-parent-id");
        var folderId = $(item).parent().attr("data-id");
        $("div.file-item").hide(); //all file-item hide
        $("div.file-item").each(function (index) { //filter file-item
            if ($(this).attr("data-parent-id") === folderId) {
                $(this).show();
            }
        });
        $("div.up-folder").hide();
        $("div.up-folder[data-id='" + parentId + "']").show();
        if ($("div.up-folder[data-id='" + parentId + "']").length == 0) {
            $("#divMedia > .panel-body > form > .file-manager-container").prepend('<div class="file-item up-folder" data-parent-id="' + folderId + '" data-id="' + parentId + '">'
                + '<div class= "file-item-icon file-item-level-up fa fa-level-up fa-2x text-secondary"></div> '
                + '<a href="javascript:" class="file-item-name">'
                + ' ..'
                + ' </a>'
                + '</div>');
            media.addEvent('up_folder');
        }
        $("#divMedia > .panel-heading").html("<span><i class='fa fa-folder'></i> " + $(item).text() + "</span>");
        media.folderId = folderId;
        media.folderParentId = parentId;
    },
    addEvent: function (event_type) {
        if (event_type === "up_folder") {
            $("div.up-folder").unbind("click").click(function () {
                var parentId = $(this).attr("data-parent-id");
                var folderId = $(this).attr("data-id");
                $("div.file-item").hide(); //all file-item hide
                $("div.file-item").each(function (index) { //filter file-item
                    if ($(this).attr("data-parent-id") === folderId) {
                        $(this).show();
                    }
                });
            });
        }
    },
    newFolder: function () {
        $('#modalAddFolderContent').load("/Media/NewFolder/" + media.folderId, function () {
            $('#modalAddFolder').modal({
                keyboard: true
            }, 'show');
            mbAjax.bindForm(this, function () {
                helper.successForm($("#modalAddFolder form"));
                media.load();
            });
        });
    },
    editFolder: function (FolderID) {
        $('#modalEditFolderContent').load("/Media/FolderEdit/" + FolderID, function () {
            $('#modalEditFolder').modal({
                keyboard: true
            }, 'show');
            mbAjax.bindForm(this, function () {
                helper.successForm($("#modalEditFolder form"));
                media.load();
            });
        });
    },
}