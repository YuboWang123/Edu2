
/**
 * add or update a lesson
 */


function delImage(p) {
    $.get('/school/trainlesson/DelLessonImage', { path: p }).done(function(v) {
        if (v.result === 1) {
            showTip(4);
            return true;
        }
    });

}

var tempImage = {},
    showTip = function(t) {
        var msg = '';
        switch (t) {
        case 0:
            msg = '<span>保存失败</span>';
            break;
        case 1:
            msg = '<span>保存成功</span>';
            break;
        case 2:
            msg = '<span>课程已存在</span>';
            break;
        case 3:
            msg = '<span>删除失败</span>';
            break;
        case 4:
            msg = '<span>删除成功</span>';
            break;
        default:
        }

        app.utility.showTips(msg);
    },

    uploadCb = function(_url) { //upload image callback fnc.

        tempImage.url = $('.pic img').attr('src');
        tempImage.tempUrl = _url[0];

        if ($('.pic .img').find('img').length !== 0) {
            if (confirm('是否替换原图片') && _url !== '') {
                $('.ImagePath').val(tempImage.tempUrl);
                delImage(tempImage.url);
                $('.pic img').attr('src', tempImage.tempUrl).fadeIn('slow');

                $('.pic :contains("删除")').on('click',
                    function() {

                        var pth = $('.pic').find('img').attr('src');
                        if (pth !== undefined) {
                            delImage(tempImage.tempUrl);
                            return false;
                        }
                    });
            }

        } else {
            var elm = '<img src=" ' + tempImage.tempUrl + ' " />';
            $('.pic .img').empty().append(elm);
            $('.ImagePath').val(tempImage.tempUrl);
        }


    },
    saveCb = function(d) {
       
        var rs = JSON.parse(d.responseText);
        console.log(rs);
        if (rs.OperResult === 0) {
            layer.msg('保存成功!');
        }
        if (rs.OperResult === 3) {
            layer.msg('课程不存在或已被删除');
        }

        var backList = function() {
            $('.sBox li.active').trigger('click');
        };
        setTimeout(backList, 1000);

    },
    lessonEdit = {
        add: '/school/trainlesson/add', //post
        update: '/school/trainlesson/edit', //post     
        start: function() {
            var ext = ['jpg', 'png', 'gif'];
            fileUpload('#UploadFile', ext, false, false, null, 'csMain', uploadCb);

            //validating.
            app.utility.valiForm();

            $('input[type=text],textarea').on('focus',
                function() {
                    var v = $(this).val();
                    if (v === $(this).attr('property')) {
                        $(this).val('');
                    }
                }).on('blur',
                function() {
                    var s = $(this).val();
                    if (s === '') {
                        $(this).val($(this).attr('property'));
                    }

                });

            $('.pic :contains("删除")').on('click',
                function() {
                    var pth = $('.pic').find('img').attr('src');
                    if (pth !== undefined) {
                        delImage(pth);
                        $('.pic img').fadeOut().remove();
                        return false;
                    }
                });


            //get genre names.
            $('a.bck').unbind('click').on('click',
                function(e) {
                    $('.sBox li.active').trigger('click');
                });


        }
    };
lessonEdit.start();