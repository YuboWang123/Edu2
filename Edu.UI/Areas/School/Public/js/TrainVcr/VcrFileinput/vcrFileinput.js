 

VcrFileUploader = function (typ, cntrl, cb, extData) {
    
        var callback = null, opts = new Object(),ext;
        switch (typ) {
            case 'vcr':
                ext = ['mp4', 'avi', 'mkv', 'rmvb', 'wma', 'mp3'];
                callback = cb.genVcrDm;
                opts = {
                    language: 'zh',
                    uploadIcon: '',
                    uploadUrl: '/school/schoolbase/getVcr',
                    allowedFileExtensions: ext,
                    showUploadedThumbs: false,
                    showPreview: false,
                    showRemove: false,
                    showUpload: false,
                    showCancel: false,
                    maxFileSize: 102400,
                    uploadExtraData: extData
                };

                break;
            case 'resource':
                ext = ['docx', 'doc', 'ppt', 'pptx', 'pdf'],
                    callback = cb.genFilesTable;
                opts = {
                    language: 'zh',
                    uploadIcon: '',
                    uploadUrl: '/school/schoolbase/vcrResource',
                    allowedFileExtensions: ext,
                    showUploadedThumbs: false,
                    showPreview: false,
                    showRemove: false,
                    showUpload: false,
                    showCancel: false,
                    maxFileSize: 10240,
                    uploadExtraData: extData
                };

                break;
            case 'test':
                ext = ['docx', 'doc'],
                callback = cb.genTstDm;
                opts = {
                    language: 'zh',
                    uploadIcon: '',
                    uploadUrl: '/school/schoolbase/vcrTest',
                    allowedFileExtensions: ext,
                    showUploadedThumbs: false,
                    showPreview: false,
                    showRemove: false,
                    showUpload: false,
                    showCancel: false,
                    maxFileSize: 10240,
                    uploadExtraData: extData
                };
                break;
        }

        $('#' + cntrl).fileinput(opts)
            .on('fileloaded', function (event, file, previewId, index, reader) {
                console.log('file loaded');
            })
            .on('fileselect', function (event, numFiles, label) {
                app.utility.tipsClear();
            })
            .on('filebatchselected', function (event, files) {
                var fn = files[0].name, myExt = fn.substr(fn.lastIndexOf('.') + 1),
                    file_sz = files[0].size / 1024;
                if (file_sz === 0) {
                 
                   layer.msg('文件没有内容 ');
                    return false;
                }
                if (typ === 'vcr') {
                   
                    if (opts.maxFileSize < file_sz) {
                        layer.msg('文件不能超出最大 ' + opts.maxFileSize / 1024 + 'M');
                        return false;
                    }
                }
                if (typ === 'test') {
                    if (opts.maxFileSize < file_sz) {
                        layer.msg('文件不能超出最大 ' + opts.maxFileSize / 1024 + 'M');
                        return false;
                    }
                }
                if (typ === 'resource') {
                    if (opts.maxFileSize < file_sz) {
                        layer.msg('文件不能超出最大 ' + opts.maxFileSize / 1024 + 'M');
                        return false;
                    }
                }

                if (opts.allowedFileExtensions.indexOf(myExt) === -1) {
              
                    layer.msg(fn + "文件类型不对");
                    return;
                }
                $('#' + cntrl).fileinput('upload');
            })
            .on('fileuploaded', function (event, data, previewId, index) {
                var form = data.form, files = data.files, extra = data.extra,
                    response = data.response.initialPreview, reader = data.reader;
               
                if (callback) {
                    if (typ === 'vcr') {
                        callback(response);
                    }
                    if (typ === 'test') {
                        callback();
                    }
                }
             
            })
            .on('fileuploaderror', function (event, data, msg) {
                app.utility.showTipsLayer(msg, ENVTYPE.danger, 5000);
         
                $(this).fileinput('clearStack');
            })
            .on('filebatchuploadcomplete', function (event, files, extra) {
                $(this).fileinput('refresh');
                if (typ === 'resource' && callback) {
                    callback();
                }
               
            })
            .on('filebatchuploadsuccess', function (event, data) {
                var form = data.form, files = data.files, extra = data.extra,
                    response = data.response, reader = data.reader;
              
                if (typ === 'vcr') {
                    $("#" + cntrl).parents('[id=home]').find('.title').hide();
                }
                layer.alert('上传成功');

            });



    };