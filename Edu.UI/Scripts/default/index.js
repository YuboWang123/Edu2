console.log('in index func');
SEND = false;

function msgSnd(data) {
    
    if (data.i === 1) {
        $.cookie('send', true, { expires: 1 });
        SEND = true;
        $('.tips').html('反馈提交成功');
    }
}

function check() {
    SEND = $.cookie('send');
    if (SEND === 'true') {
        $('.tips').html('您已经提交过反馈,请24小时之后再提交');
        return false;
    } else {
        return true;
    }
}

indexStart = function() {
    var bn = $('#contact').find('a.btn-primary');
    bn.click(function() {
        if (aut === 'False') {
            alert('没有登录');
            return false;
        }
        if (check()) {
            bn.parents('form').submit();
        }
    });

    $('#latest .row>div').on('click',
        function() {
            var loc = $(this).find('a.thumbnail').attr('href');
            window.location = loc.substr(1);
            return false;
        });


};
indexStart();