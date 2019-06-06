
/*ctrl:dom ;exts:['jpg','mp4'...]; initialPreviewStr: html;prvwShow:boolean;prvClass:略图css*/
fileUpload = function(cntrl, exts, prvwShow, uploadShow, hiddenCtrl, prvClass, cb) {
    console.log('in the file uploader');
    $(cntrl).fileinput({
        language: 'zh',
        uploadUrl: "/base/getFile",
        allowedFileExtensions: exts,
        mainClass: 'csMain',
        previewClass: prvClass,
        showUploadedThumbs: false,
        showPreview: prvwShow,
        showRemove: false,
        showUpload: uploadShow,
        showCancel: false,
        maxFileSize: 32428800

        //initialPreview: initialPreviewStr       
    }).on('fileselect',
        function(event, numFiles, label) {
            //$('.modal-footer').find('span').empty();
        }).on('fileloaded',
        function(event, file, previewId, index, reader) {
            // alert(file.maxFileSize);
            if (file.maxFileSize) { //if file length is byd the limits 
                return false;
            } else {
                $(this).fileinput('upload');
            }

        }).on('fileuploaded',
        function(event, data, previewId, index) {
            var form = data.form,
                files = data.files,
                extra = data.extra,
                response = data.response.initialPreview,
                reader = data.reader;
            $(hiddenCtrl).val(response); //结果赋给相应的dom,only one
            if (cb) {
                cb(response); //need function for callback.
            }


        }).on('filebatchuploadsuccess',
        function(event, data, previewId, index) {
            var form = data.form,
                files = data.files,
                extra = data.extra,
                response = data.response,
                reader = data.reader;
            console.log('File batch upload success');
        }).on('fileerror',
        function(event, data, msg) {

        }).on('fileuploaderror',
        function(event, data, msg) {
            //$('.in .modal-footer').find('span').text('').text(msg);
        });

};

/*ctrl:dom ;extArray:['jpg','mp4'...]; prvwShow:boolean;prvClass:略图css;extraStr: extraData*/
