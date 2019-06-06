/********************************/

function supportsStorage() {
    try {
        return 'localStorage' in window && window.sessionStorage === null;
    } catch (e) {
        return false;
    }
}

var fu = null,fuHidden=$('hidden.fu');

if (!supportsStorage()) {
    checkSto = function () {
        if (fuHidden.length===1 && localStorage.fu !== '') {
            fuHidden.val(localStorage.fu);
            return true;
        }
        return false;
    };
    SetVal = function(k, v) {
        this.val = v;
        this.k = k;
    };

} else {
    console.log('local storage not support');
}


tabFunc = function() {
    $('.title-list li').mouseover(function() {
        var liindex = $('.title-list li').index(this);
        $(this).addClass('on').siblings().removeClass('on');
        $('.product-wrap div.product').eq(liindex).fadeIn(150).siblings('div.product').hide();
        var liWidth = $('.title-list li').width();
        $('.title-list p').stop(false, true).animate({ 'left': liindex * liWidth + 'px' }, 300);
    });
};


