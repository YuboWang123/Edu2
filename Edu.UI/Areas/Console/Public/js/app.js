/*app start of console*/
var app = new Object() || app;
app.utility = {
    valiForm: function () {      //if use validata.ajax.js  
        $.validator.unobtrusive.parse($("[id^=form]"));
        $.extend($.validator.defaults, { ignore: ":hidden" });
    },
    arrayTableRow: function (array) { //del the row with the id of the arraylist.
        if (array.length > 0) {
            for (var i = 0; i < array.length; i++) {
                $("input:checkbox").each(function() {
                    if ($(this).val() === array[i]) {
                        $(this).parents("tr").hide(2000);
                    }
                });
            }
        }
    },
    doAjaxFn: function (url, data, cb, postOrGet) {
        if (postOrGet == 'post') {
            $.post(url, data).done(function(v) {
                if (cb) {
                    cb(v);
                }
            });
        } else if (postOrGet === 'get') {
            $.get(url, data).done(function(v) {
                if (cb) {
                    cb(v);
                }
            });
        }
    },
    doAjax: function (_url, _data, cb) {
        $.ajax({
            url: _url,
            data: _data,
            asyn: true,
            success: function(v) {
                $('#main').html(v);
                if (cb) {
                    cb();
                }
            }
        });
    },
    nodeToString: function (node) { //html elements to string.
        var tmpNode = document.createElement("div");
        tmpNode.appendChild(node.cloneNode(true));
        var str = tmpNode.innerHTML;
        tmpNode = node = null;
        return str;
    }
};
 

app.ds = (function () {
    var _init = function () {
        /*左侧导航栏显示隐藏功能*/
        $(".subNav").click(function() {
            /*显示*/
            if ($(this).find("span:first-child").attr('class') === "title-icon glyphicon glyphicon-chevron-down") {
                $(this).find("span:first-child").removeClass("glyphicon-chevron-down");
                $(this).find("span:first-child").addClass("glyphicon-chevron-up");
                $(this).removeClass("sublist-down");
                $(this).addClass("sublist-up");
            }
            /*隐藏*/
            else {
                $(this).find("span:first-child").removeClass("glyphicon-chevron-up");
                $(this).find("span:first-child").addClass("glyphicon-chevron-down");
                $(this).removeClass("sublist-up");
                $(this).addClass("sublist-down");
            }
            // 修改数字控制速度， slideUp(500)控制卷起速度
            $(this).next(".navContent").slideToggle(300).siblings(".navContent").slideUp(300);
        });
        /*左侧导航栏缩进功能*/
        $(".left-main .sidebar-fold").click(function() {

            if ($(this).parent().attr('class') === "left-main left-full") {
                $(this).parent().removeClass("left-full");
                $(this).parent().addClass("left-off");

                $(this).parent().parent().find(".right-product").removeClass("right-full");
                $(this).parent().parent().find(".right-product").addClass("right-off");

            } else {
                $(this).parent().removeClass("left-off");
                $(this).parent().addClass("left-full");

                $(this).parent().parent().find(".right-product").removeClass("right-off");
                $(this).parent().parent().find(".right-product").addClass("right-full");

            }
        });

        /*左侧鼠标移入提示功能*/
        $(".sBox ul li").mouseenter(function() {
            if ($(this).find("span:last-child").css("display") === "none") {
                $(this).find("div").show();
            }
        }).mouseleave(function() { $(this).find("div").hide(); });
        /*click*/
        $('.sBox ul li').on('click',
            function() {

                $('.sBox').find('li').removeClass('active');
                $(this).addClass('active');

                var ctr = $(this).find('a').attr('property');
                app.utility.doAjax('../' + ctr,
                    null,
                    function() {
                        //page funcs inside
                        var areaHead = $('.page-header'), title = $('.sBox li.active').find('.sub-title').text();
                        areaHead.find('h4').text(title); //title change.

                    });

            });


    },
        partial = function (v) {
       
            $('#main').html(v);
        }
       ;

    return {
        start: _init
    };
})();

app.ds.start();

