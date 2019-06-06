var payCheckUrl = '/pay/vip';
function UploadAvat(opts) {
    this.start = function() {
        $(opts.trigger).on('change',
            function() {

            });
    };

    return {
         start:start
    };
}

function bfAct() {
    console.log('bf active');
    $('.validation-summary-errors li').text('');
}
function activeCb(d) {
    
    if (d === 0) {
        layer.msg('卡片不存在!');
        return;
    }
    if (d === 2) {
        layer.alert('卡片激活成功!');
        return;
    }
    if (d === 3) {
        layer.msg('卡号或密码错误!');
        return;
    }
    if (d === 1) {
        layer.msg('尝试大于5次,卡片被冻结!');
        return;
    }

}

function pay() {

    layer.alert('发送支付查询');
    $.get(payCheckUrl, null).done(function(v) {
      setTimeout(function () {
            layer.closeAll('loading');
        }, 1);

        if (v === 1) {
            layer.msg('支付结果是:' + v);
        } else {
            layer.msg('支付失败!');
        }

    });
  
}


function popPay() {
    layer.open({
        title: '支付'
        , content: '立即购买vip'     //show orcode
        , yes: function (ind, la) {
            layer.close(ind);
            layer.load();
            pay();
        }
    });  
}



UploadAvat({
    url: '/base/getFile',
    trigger:'.imgUpload'
}).start();

$('.pay').on('click', pay);

$('.title-list li').click(function () {
    var liindex = $('.title-list li').index(this);
    $(this).addClass('on').siblings().removeClass('on');
    $('.product-wrap div.product').eq(liindex).fadeIn(150).siblings('div.product').hide();
    var liWidth = $('.title-list li').width();
    $('.title-list p').stop(false, true).animate({ 'left': liindex * liWidth + 'px' }, 300);
});




