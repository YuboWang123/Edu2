$(function () {
    //初始化绑定默认的属性
    $.upLoadDefaults = $.upLoadDefaults || {};
    $.upLoadDefaults.property = {
        multiple: false, //是否多文件
        water: false, //是否加水印
        thumbnail: false, //是否生成缩略图
        sendurl: "/base/getWebUploaderFile", //发送地址
        filetypes: "jpg,jpge,png,gif", //文件类型
        filesize: "2048", //文件大小
        filenum: "1",//文件个数
        btntext: "浏览...", //上传按钮的文字
        swf: "../js/fileUpload/webUpload/uploader.swf", //SWF上传控件相对地址
        pathname: ""//文件追加路径
    };
    //初始化上传控件
    $.fn.InitUploader = function (b) {
        var fun = function (parentObj) {
             var p = $.extend({}, $.upLoadDefaults.property, b || {});
            var btnObj = $('<div class="upload-btn">' + p.btntext + '</div>').appendTo(parentObj);
            //初始化属性
            p.sendurl += "?action=UpLoadFile";
            if (p.water) {
                p.sendurl += "&IsWater=1";
            }
            if (p.thumbnail) {
                p.sendurl += "&IsThumbnail=1";
            }
            if (p.pathname) {
                p.sendurl += "&PathName=" + p.pathname;
            }
            if (!p.multiple) {
                p.sendurl += "&DelFilePath=" + parentObj.siblings(".upload-path").val();
            }

            //初始化WebUploader
            var uploader = WebUploader.create({
                auto: true, //自动上传
                swf: p.swf, //SWF路径
                server: p.sendurl, //上传地址
                pick: {//创建选择文件的按钮
                    id: btnObj,
                    multiple: p.multiple
                },
                accept: {//只能允许选择的文件类型
                    extensions: p.filetypes
                },
                formData: {
                    'DelFilePath': '' //定义参数
                },
                fileVal: 'Filedata', //上传域的名称
                fileSingleSizeLimit: p.filesize * 1024, //文件大小
                fileNumLimit: p.filenum//可上传的文件数量
            });

            //当validate不通过时，会以派送错误事件的形式通知
            uploader.on('error', function (type) {
                switch (type) {
                    case 'Q_EXCEED_NUM_LIMIT':
                        alert("错误：上传文件数量过多！");
                        break;
                    case 'Q_EXCEED_SIZE_LIMIT':
                        alert("错误：文件总大小超出限制！");
                        break;
                    case 'F_EXCEED_SIZE':
                        alert("错误：文件大小超出限制！");
                        break;
                    case 'Q_TYPE_DENIED':
                        alert("错误：禁止上传该类型文件！");
                        break;
                    case 'F_DUPLICATE':
                        alert("错误：请勿重复上传该文件！");
                        break;
                    default:
                        alert('错误代码：' + type);
                        break;
                }
            });

            //当有文件添加进来的时候
            uploader.on('fileQueued', function (file) {
                //如果是单文件上传，把旧的文件地址传过去
                if (!p.multiple) {
                    uploader.options.formData.DelFilePath = parentObj.siblings(".upload-path").val();
                }
                //防止重复创建
                if (parentObj.children(".upload-progress").length == 0) {
                    //创建进度条
                    var fileProgressObj = $('<div class="upload-progress"></div>').appendTo(parentObj);
                    var progressText = $('<span class="txt">正在上传，请稍候...</span>').appendTo(fileProgressObj);
                    var progressBar = $('<span class="bar"><b></b></span>').appendTo(fileProgressObj);
                    var progressCancel = $('<a class="close" title="取消上传">关闭</a>').appendTo(fileProgressObj);
                    //绑定点击事件
                    progressCancel.click(function () {
                        uploader.cancelFile(file);
                        fileProgressObj.remove();
                    });
                }
            });

            //文件上传过程中创建进度条实时显示
            uploader.on('uploadProgress', function (file, percentage) {
                var progressObj = parentObj.children(".upload-progress");
                progressObj.children(".txt").html(file.name);
                progressObj.find(".bar b").width(percentage * 100 + "%");
            });

            //当文件上传出错时触发
            uploader.on('uploadError', function (file, reason) {
                uploader.removeFile(file); //从队列中移除
                alert(file.name + "上传失败，错误代码：" + reason);
            });

            //当文件上传成功时触发
            uploader.on('uploadSuccess', function (file, data) {
                if (data.status == '0') {
                    var progressObj = parentObj.children(".upload-progress");
                    progressObj.children(".txt").html(data.msg);
                }
                if (data.status == '1') {
                    //如果是单文件上传，则赋值相应的表单
                    if (!p.multiple) {                        
                        parentObj.siblings(".upload-name").val(data.name);
                        parentObj.siblings(".upload-path").val(data.path);
                        parentObj.siblings(".upload-size").val(data.size);
                    } else {
                        addImage(parentObj, data.path, data.thumb);

                        //批量上传资源
                        try {
                            datumsManager.setBatchList(data);
                        }
                        catch (e) { }
                    }
                    var progressObj = parentObj.children(".upload-progress");
                    progressObj.children(".txt").html("上传成功：" + file.name);
                }
                uploader.removeFile(file); //从队列中移除
            });

            //不管成功或者失败，文件上传完成时触发
            uploader.on('uploadComplete', function (file) {
                var progressObj = parentObj.children(".upload-progress");
                progressObj.children(".txt").html("上传完成");
                //如果队列为空，则移除进度条
                if (uploader.getStats().queueNum == 0) {
                    progressObj.remove();
                }
            });
        };
        return $(this).each(function () {
            fun($(this));
        });
    }
});

/*图片相册处理事件
=====================================================*/
//添加图片相册
function addImage(targetObj, originalSrc, thumbSrc) {
    //插入到相册UL里面
    var newLi = $('<li>'
    + '<input type="hidden" name="hid_photo_name" value="0|' + originalSrc + '|' + thumbSrc + '" />'
    + '<input type="hidden" name="hid_photo_title" value="" />'
    + '<input type="hidden" name="hid_photo_remark" value="" />'
    + '<input type="hidden" name="hid_photo_sort" value="" />'
    + '<div class="img-box" onclick="setFocusImg(this);">'
    + '<img src="' + thumbSrc + '" bigsrc="' + originalSrc + '" />'
    + '<span class="remark"><i>暂无名称</i></span>'
    + '</div>'
    + '<a href="javascript:;" onclick="setRemark(this);">描述</a>'
    + '<a href="javascript:;" onclick="delImg(this);">删除</a>'
    + '</li>');
    newLi.appendTo(targetObj.siblings(".photo-list").children("ul"));

    //默认第一个为相册封面
    var focusPhotoObj = targetObj.siblings(".focus-photo");
    if (focusPhotoObj.val() == "") {
        focusPhotoObj.val(thumbSrc);
        newLi.children(".img-box").addClass("selected");
    }
}
//设置相册封面
function setFocusImg(obj) {
    var focusPhotoObj = $(obj).parents(".photo-list").siblings(".focus-photo");
    focusPhotoObj.val($(obj).children("img").eq(0).attr("src"));
    $(obj).parent().siblings().children(".img-box").removeClass("selected");
    $(obj).addClass("selected");
}
//图片描述窗内容
function getHtml() {
    var result = "";
    result += '<div class="dl-albums-box">';
    result += '<dl>';
    result += '<dt>图片名称：</dt>';
    result += '<dd><input id="AlbumsTitle" type="text" class="form-control input-ruler" placeholder="图片显示的名称" style="width: 500px;"></dd>';
    result += '</dl>';
    result += '<dl>';
    result += '<dt>设计意图：</dt>';
    result += '<dd><textarea id="ImageRemark" class="editor" style="margin:10px 0;font-size:12px;padding:3px;color:#000;border:1px #d2d2d2 solid;vertical-align:middle;width:100%;height:200px;" placeholder="该图片的设计意图"></textarea></dd>';
    result += '</dl>';
    result += '<dl>';
    result += '<dt>排序编码：</dt>';
    result += '<dd><input id="SortCode" type="text" class="form-control" style="width: 100px;" value="99" /> <span>*数字，越小越向前</span></dd>';
    result += '</dl>';
    result += '</div>';
    return result;
}
//设置图片描述
function setRemark(obj) {
    var parentObj = $(obj); //父对象
    var hidTitleObj = parentObj.prevAll("input[name='hid_photo_title']").eq(0);//名称
    var hidRemarkObj = parentObj.prevAll("input[name='hid_photo_remark']").eq(0);//描述
    var hidSortObj = parentObj.prevAll("input[name='hid_photo_sort']").eq(0);//排序
    var d = parent.dialog({
        title: "图片扩展信息",
        content: getHtml(),
        button: [{
            value: '批量描述',
            callback: function () {
                var titleObj = $('#AlbumsTitle', parent.document);
                var remarkObj = $('#ImageRemark', parent.document);
                var sortObj = $('#SortCode', parent.document);
                parentObj.parent().parent().find("li input[name='hid_photo_title']").val(titleObj.val());
                parentObj.parent().parent().find("li input[name='hid_photo_remark']").val(remarkObj.val());
                parentObj.parent().parent().find("li input[name='hid_photo_sort']").val(sortObj.val());
                parentObj.parent().parent().find("li .img-box .remark i").html(titleObj.val());
            }
        }, {
            value: '单张描述',
            callback: function () {
                var titleObj = $('#AlbumsTitle', parent.document);//名称
                var remarkObj = $('#ImageRemark', parent.document);//描述
                var sortObj = $('#SortCode', parent.document);//排序
                hidTitleObj.val(titleObj.val());
                hidRemarkObj.val(remarkObj.val());
                hidSortObj.val(sortObj.val());
                parentObj.siblings(".img-box").children(".remark").children("i").html(titleObj.val());
            },
            autofocus: true
        }]
    }).showModal();
    //给弹窗赋值
    var popup = d.__popup;
    popup.find("#AlbumsTitle").val(hidTitleObj.val());
    popup.find("#ImageRemark").val(hidRemarkObj.val());
    if (hidSortObj.val() > 0) {
        popup.find("#SortCode").val(hidSortObj.val());
    }
}
//删除图片LI节点
function delImg(obj) {
    var parentObj = $(obj).parent().parent();
    var focusPhotoObj = parentObj.parent().siblings(".focus-photo");
    var smallImg = $(obj).siblings(".img-box").children("img").attr("src");
    $(obj).parent().remove(); //删除的LI节点
    //检查是否为封面
    if (focusPhotoObj.val() == smallImg) {
        focusPhotoObj.val("");
        var firtImgBox = parentObj.find("li .img-box").eq(0); //取第一张做为封面
        firtImgBox.addClass("selected");
        focusPhotoObj.val(firtImgBox.children("img").attr("src")); //重新给封面的隐藏域赋值
    }
}