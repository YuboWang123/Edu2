/**
 * 
 * @param {} cntrl control name of the html. 
 * @param {} exts  array of the file's extension
 * @param {} prvwShow if the preview is shown.
 * @param {} uploadShow if show the upload progress.
 * @param {} hiddenCtrl control that accept cb value.
 * @param {} prvClass 
 * @param {} cb call back func.
 * @returns {}
 * */

fileUpload = function(cntrl, exts, prvwShow, uploadShow, hiddenCtrl, prvClass, cb) {
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

      
    }).on('fileselect',
        function(event, numFiles, label) {
            console.log('file selected');
            
        }).on('fileloaded',
        function(event, file, previewId, index, reader) {
           
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

 