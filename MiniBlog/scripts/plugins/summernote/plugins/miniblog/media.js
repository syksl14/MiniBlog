var mediaPlugin = {
    init: function () {
        if ($("#btnMediaSelect").length === 0) {
            $(".modal[role='dialog'] .note-group-image-url").append('<button id="btnMediaSelect" type="button" class="btn btn-primary btn-sm" title="Medya\'\dan Seç" style="margin-left: 5px; margin-top: 1px;"><i class="fa fa-folder"></i></button>');
            $(".modal[role='dialog'] .note-group-image-url label").css({
                'width': '100%'
            });
            $(".modal[role='dialog'] .note-group-image-url input").css({
                'width': '90%'
            });
            $("#btnMediaSelect").click(function () {
                var dialog = bootbox.dialog({
                    message: '<p><i class="fa fa-spin fa-spinner"></i> Yükleniyor...</p>',
                    closeButton: false,
                    size: 'large',
                    buttons: {
                        cancel: {
                            label: "İptal",
                            className: 'btn-default',
                            callback: function () {
                                localStorage.removeItem("file-path");
                            }
                        },
                        success: {
                            label: "Dosyayı Seç",
                            className: 'btn-info',
                            callback: function () {
                                if (localStorage.getItem("file-path") !== null) {
                                    dialog.modal('hide');
                                    $(".note-image-url").val(localStorage.getItem("file-path")).keyup(); 
                                    localStorage.removeItem("file-path");
                                } else {
                                    bootbox.alert("Lütfen geçerli bir dosya seçiniz!");
                                }
                            }
                        }
                    }
                });
                dialog.init(function () {
                    setTimeout(function () {
                        dialog.find('.bootbox-body').html('<iframe src="/Admin/Media?browser=true" border="0" style="border: none; width: 100%; height: 550px;"></iframe>');
                    }, 1000);
                });

            });
        }
    }
}