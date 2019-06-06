/*app start of console*/



var app = new Object() || app,
    topNav = $('#topmenu li.li-border:gt(0)'),
    ENVTYPE = { default: 'default', info: 'info', success: 'success', warning: 'warning', danger: 'warning' };

   app.utility = {
    getUrlParam: function (name) {
        //构造一个含有目标参数的正则表达式对象
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        //匹配目标参数
        var r = window.location.search.substr(1).match(reg);
        //返回参数值
        if (r !== null) return unescape(r[2]);
        return null;
    },
    checkAll: function () {
        var t = $('table thead').find('[type=checkbox]').is(':checked');
        if (t) {
            $("table tbody input[type=checkbox]").prop("checked", true);
        } else {
            $("table tbody input[type=checkbox]").prop("checked", false);
        }
    },
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
        if (postOrGet === 'post') {
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
                if (cb) {
                    cb(v);
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
    },
    showTips: function(msg) {
        $('.tips').html(msg).slideDown('fast');
        var tim = function () {
            $('.tips').slideUp('fast').empty();
        };
        setTimeout(tim, 2500);
    },
   showTipsLayer: function(msg, cls,timeout) {
       var t = $('.tips'), clsStr = '';

       switch (cls) {
       case ENVTYPE.default:
           clsStr = 'default';
           break;
       case ENVTYPE.info:
           clsStr = 'info';
           break;
       case ENVTYPE.warn:
           clsStr = 'warning';
           break;
       case ENVTYPE.danger:
           clsStr = 'danger';
           break;
       default:
           clsStr = 'success';
           break;
       }
       t.removeClass(clsStr).addClass(clsStr);
       t.html(msg).slideDown('fast');
           var tim = function () {
               t.slideUp('fast').empty();
               t.removeClass(clsStr);

           };
           if (timeout) {
               setTimeout(tim, timeout);
           } 
       },
       tipsClear: function() {
           $('.tips').slideUp('fast')
               .removeClass('danger info warning success default')
               .empty();
       },
       showTipsCls: function (msg, cls) {
        var t = $('.tips'),clsStr='';

        switch (cls) {
            case ENVTYPE.default:
                clsStr = 'default';
                break;
            case ENVTYPE.info:
                clsStr = 'info';
                break;
            case ENVTYPE.warn:
                clsStr = 'warning';
                break;
            case ENVTYPE.danger:
                clsStr = 'danger';
                break;
            default:
                clsStr = 'success';
                break;
        }
        t.removeClass(clsStr).addClass(clsStr);
        t.html(msg).slideDown('fast');
        var tim = function () {
            t.slideUp('fast').empty();
            t.removeClass(clsStr);

        };
        setTimeout(tim, 2500);

    }
};
 

app.ds = (function () {
    var frsh = function () {
        $('.sBox li.active a').trigger('click');
    },
        pagCb = function () {
            /*左侧导航栏向上显示隐藏功能*/
            $(".subNav").unbind('click').click(function() {
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
                    $(this).next().find('.sublist-titl').addClass('hidden');
                    $(this).parent().parent().find(".right-product").removeClass("right-full");
                    $(this).parent().parent().find(".right-product").addClass("right-off");

                } else {
                    $(this).parent().removeClass("left-off");
                    $(this).parent().addClass("left-full");
                    $(this).next().find('.sublist-titl').removeClass('hidden');
                    $(this).parent().parent().find(".right-product").removeClass("right-off");
                    $(this).parent().parent().find(".right-product").addClass("right-full");
                }

            });

            //page funcs inside
            var
                areaHead = $('.page-header'), title = $('.sBox li.active').find('.sub-title').text();
            areaHead.find('.title').text(title); //title change.

            var pageFor = $('.sBox li.active').find('a').attr('property');

            if (pageFor !== undefined && pageFor !== '') {
                pageFor = pageFor.substring(pageFor.indexOf('/') + 1);

            }

            $('th input[type=checkbox]').on('click',
                function() {
                    app.utility.checkAll();
                });

            areaHead.find('button.frsh').on('click', function () {
                frsh();
            });

            areaHead.find('button.delgrp').on('click', function () {
                $(this).parent('tr').siblings().removeClass('active');
                $(this).parents('tr').addClass('active');
                var k = $(this).parents('tr').attr('property');
                $('#' + pageFor + 'Del').modal('show');
            });

            areaHead.find('button.addMdl').on('click',
                function() {
                    //get add type
                    $('#' + pageFor + 'Add').modal('show');
                });


            //trainbase edit
            $('tr a.edit').on('click', function () {
                $(this).parent('tr').siblings().removeClass('active');
                $(this).parents('tr').addClass('active');
                var k = $(this).parents('tr').attr('property');
            
                $('#' + pageFor + 'Edit').modal('show');
                var u = '/school/trainbase/' + pageFor + 'Edit';
                if (k !== undefined && k !== null) {
                    app.utility.doAjaxFn(u,
                        { id: k },
                        function(r) {
                            $('#' + pageFor + 'Edit').on('shown.bs.modal',
                                function() {
                                    $('.modal-body').html(r);
                                });

                        },
                        'get');
                };

            });

            //del trainbase 
            $('.trainbase a.btn-danger').on('click',
                function() {
                    $(this).parents('tr').siblings().removeClass('act');
                    $(this).parents('tr').addClass('act');

                    if (confirm("真的要删除吗?")) {
                        var _id = $(this).parents('tr').attr('property'), _t = pageFor, t = 0;

                        switch (_t) {
                        case 'period':
                            t = 1;
                            break;
                        case 'subject':
                            t = 2;
                            break;
                        case 'genre':
                            t = 3;
                            break;
                        default:
                            t = 4;
                            break;
                        }
                        // return false;

                        $.get('/school/trainbase/delTrainBase', { type: t, id: _id }).done(function(v) {
                            if (v === '1') {
                                $('tr.act').hide().remove();
                            }
                        });
                    }

                });

        },
        sideBarCb = function () {
           
            $('.navContent').hide();
            var menuLi = $('.sBox ul li');
            $('div.left-main').removeClass('left-off').addClass('left-full').next().removeClass('right-off').addClass('right-full');
            $('.sublist-title').html($('.li-border.active').find('a').text());//change the sidebar title.

            /*click change main by click side bar*/
            menuLi.on('click',
                function() {
                    $('.sBox').find('li').removeClass('active');
                    $(this).addClass('active');

                    var ctr = $(this).find('a').attr('property');

                    app.utility.doAjax('/school/' + ctr,
                        null,
                        function(v) {
                            $("#main").html(v);
                            pagCb();
                        });

                });
            
            menuLi.eq(0).trigger('click');

        },
        _init = function () {
            /**
             * right main height.
             */
            $('.maincontent').height($('.left-full').height());
            topNav.eq(1).addClass('active');
            $('li.showtitle').eq(0).addClass('active');
            pagCb();

            /*左侧鼠标移入提示功能*/
            $(".sBox ul li").mouseenter(function() {
                if ($(this).find("span:last-child").css("display") === "none") {
                    $(this).find("div").show();
                }
            }).mouseleave(function() { $(this).find("div").hide(); });

            sideBarCb();

            /****************change main by click top bar:1 change side bar . 2 change main*********/
            topNav.on('click', function () {
           
                $("#main").empty();
                topNav.removeClass('active');
                $(this).addClass('active');

                var topid = $(this).find('a').attr('property'); //change main content by top menu id.
                //ajax change main content.
                app.utility.doAjax('/school/dashboard/SideBar', { topId: topid }, function (v) {
                   
                    $('.left-main').empty().html(v);
                    pagCb();
                    sideBarCb();
                    pagCb();

                });
            });

            /*user msgs show*/
            $('li.userMsg').on('click',
                function() {
                    app.utility.doAjax('/school/dashboard/userMsgs',
                        null,
                        function(v) {
                            $("#main").html(v);
                        });
                });

            topNav.first().trigger('click');
        },
        _ajaxSuccess = function () {
            frsh();
            $(".modal").toggle();
        };

    return {
        start: _init,
        ajaxSuccess: _ajaxSuccess,
        leftDom:pagCb
    };
})();

app.ds.start();