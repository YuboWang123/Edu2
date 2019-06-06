var uploader;
//在点击弹出模态框的时候再初始化WebUploader，解决点击上传无反应问题
$("#VideoAdd").on("shown.bs.modal", function () {
    uploader = WebUploader.create({
        swf: '/Areas/Admin/Js/fileUpload/webUpload/uploader.swf',
        server: "/base/getWebUploaderFile",// 后台路径
        pick: '.upload-image', // 选择文件的按钮。可选。内部根据当前运行是创建，可能是input元素，也可能是flash.
        resize: false,// 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        chunked: true, // 是否分片
        duplicate: true,//去重， 根据文件名字、文件大小和最后修改时间来生成hash Key.
        chunkSize: 52428 * 100, // 分片大小， 5M
        /*    fileSingleSizeLimit:100*1024,//文件大小限制*/
        auto: true,
        // 只允许选择图片文件。
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/jpg,image/jpeg,image/png'
        }
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) { 
        $("#ImagePath").val(response.path).next().val(response.path);
    });

    // 当文件上传出错时触发
    uploader.on('uploadError', function (file) {
        $(".errLable").text("上传失败");
        uploader.removeFile(file); //从队列中移除
        alert(file.name + "上传失败，错误代码：" + reason);
    });

    //当validate不通过时触发
    uploader.on('error', function (type) {
        if (type == "F_EXCEED_SIZE") {
            alert("文件大小不能超过xxx KB!");
        }
    });



    /////////
    uploader_video = WebUploader.create({
        swf: '/Areas/Admin/Js/fileUpload/webUpload/uploader.swf',
        server: '/base/getWebUploaderFile',// 后台路径
        pick: '.upload-video', // 选择文件的按钮。可选。内部根据当前运行是创建，可能是input元素，也可能是flash.
        resize: false,// 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        chunked: true, // 是否分片
        duplicate: true,//去重， 根据文件名字、文件大小和最后修改时间来生成hash Key.
        chunkSize: 52428 * 100, // 分片大小， 5M
        /*    fileSingleSizeLimit:100*1024,//文件大小限制*/
        auto: true,
        // 只允许选择图片文件。
        accept: {
            title: 'video',
            extensions: 'mp4,rmvb,wmv,avi',
            //mimeTypes: 'video/x-msvideo'
        }
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader_video.on('uploadSuccess', function (file, response) {
        //var fileUrl = response.data.fileUrl;
        //TODO      
        $("#VideoPath").val(response.path).next().val(response.path);
    });

    // 当文件上传出错时触发
    uploader_video.on('uploadError', function (file) {
        $(".errLable").text("上传失败");
        uploader_video.removeFile(file); //从队列中移除
        //alert(file.name + "上传失败，错误代码：" + reason);
    });

    //当validate不通过时触发
    uploader_video.on('error', function (type) {
        if (type == "F_EXCEED_SIZE") {
            alert("文件大小不能超过xxx KB!");
        }
    });
});

//关闭模态框销毁WebUploader，解决再次打开模态框时按钮越变越大问题
$('#VideoAdd').on('hide.bs.modal', function () {
    $(".errLabel").text("");
    uploader.destroy();
    uploader_video.destroy();
});


