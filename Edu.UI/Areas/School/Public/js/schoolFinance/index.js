var
    STATUS=0,
    currentPage,
    urlPrefix = "/school/schoolFinance/",
    CONFIG_ID,
    mainPage = $('#main'),
    configListUrl = urlPrefix+ 'index',
    cardListUrl = urlPrefix + 'cardList', //get card list .
    configEditUrl = urlPrefix + 'configEdit',
    getXleUrl = urlPrefix +'listExport',
    delConfigUrl = urlPrefix +  'delConfig',
    delCard = urlPrefix +  'delCard',
    statusBtnGroup = $('.statusBtnGroup li a'),
    CARDTYPE=$('.pull-left .dropdown-menu').attr('data-type');


//paging the table of configs.
function GetPageList(p) {
    var configType = function () {
        if (CARDTYPE === 'rechargableCard') {
            return 0;
        }
        return 1;
    };

    $.get(urlPrefix + '_configTable', { cardType: configType(), state: STATUS, pg: p }).done(function (v) {
        $('.configTbl').html(v);
        tableHandler();
        return false;
    });
}

function checkAll()
{
    var t = $('table thead').find('[type=checkbox]').is(':checked');
    if (t) {
        $("table tbody input[type=checkbox]").prop("checked", true);
    }
    else {
        $("table tbody input[type=checkbox]").prop("checked", false);
    }
}

tableHandler = function ()
{
    $('table .st:contains("AdminFreezed")').text('(冻结)');
    $('table .st:contains("InUse")').text('正常');
    $('table .st:contains("Created")').text('(正常)');
    $('table .st:contains("NeverUsed")').text('(未使用)');
    $('table .st:contains("Deleted")').text('(已删除)');
    $('table .st:contains("Outdated")').text('(过期)');
    $('table thead .checkbox-inline').on('click', checkAll);
    $('table tbody tr').on('click',
        function () {
            $(this).siblings().removeClass('selected');
            $(this).addClass('selected');
    });

},

config =
{
    ajaxSuccess: function(v) {
        console.log('ajax successed');
    },
    ui: function() {
        tableHandler();
        statusBtnGroup.on('click', { id:$(this) }, config.getStatusList);
        $('.modal-footer .waiting').removeClass('shw');
        $('.addMdl').on('click', config.newConfig);
    },
    startChg: function(e) {
        console.log('changed' + $(e).target.val());
    },
    //when the duration blurs.
    durationSelect: function(e) {
        var s = $(e).parents('.input-group').find('select').val()
            , v = $(e).parents('.input-group').find('.ValidPeriodInput'), r = 1,
            x = parseInt(v.val());

        switch (s) {
            case 'week':
                r = x*7;
                break;
            case 'month':
                r =x*30;
                break;
            case 'year':
                r= x*365;
                break;
            default:
                r = v.val();
        }
    
        $(e).parents('.input-group').find(':hidden[name="ValidPeriod"]').val(r);
    },
    newConfig: function() {
        $('#ConfigNew').
            on('shown.bs.modal', function () {
                app.utility.valiForm();
                layui.use('laydate', function () {
                    var laydate = layui.laydate;
                    laydate.render({
                        elem: '#Start',
                        min: 0
                        , theme: '#393D49'
                        , range: '~'
                    });
                });
            })
            .modal('show');
    },
    onFailure: function () {
        $('#CardGen').find('.waiting').hide();
    },
    genSuccess: function () {
        $('#CardGen').find('.waiting').show();
        var slp = function() {
            $('#CardGen').modal('hide').find('.waiting').hide();
            config.back();
            layer.alert('卡片已生成完毕');
        };
        setTimeout(slp, 3000);
    },
    delCfg: function() {
        console.log(CONFIG_ID);

        if (CONFIG_ID!=='') {
            $.ajax({
                url: delConfigUrl,
                data: { id: CONFIG_ID },
                success: function (v) {
                    if (v.i === 0) {
                        layer.msg('已删除');
                        config.back();
                    }
                   
                }
            });
        } else {
            $('.modal').hide();
            alert('参数错误！');
        }
    },
    delCardCfg: function (e) { 
         
        var c = parseInt($(e).attr('data-bind'));
        if (c !== 0) {
            alert('请先删除卡记录');
            return;
        }

        CONFIG_ID = $(e).attr('rel');
        $("#ConfigDel").on('shown.bs.modal',
            function() {
            }).modal('show');


    },
    delNotUsed: function() { //删除没有用过的
        var _url = '/admin/Fin/CardDelNotUsed',
            confId = $('#confId').val();
        if (confId === undefined) return false;
        $.ajax({
            url: _url,
            data: { confgId: confId },
            success: function(v) {
                alert("删除了" + v + "张卡");
            }
        });

    },
    getStatusList: function (e) {
        if ($(e.target).attr('rel')) {
            STATUS = $(e.target).attr('rel');
            GetPageList(1);
        }
    },
    getPageList: function (configId, pg) { //view card list with config id.
        CONFIG_ID = configId;
        if (pg === undefined)
        {
            pg = 1;
        }
        $.ajax({
            url: cardListUrl,
            data: { id: configId, pg: pg },
            dataType: "html",
            success: function(v) {
                mainPage.html(v);
                tableHandler();
            },
            error: function(ex) {
            }
        });
    },
    back: function() {
        $('.sBox .active').click();
    },
    exportExcle: function (e) {
        //console.log($(e).attr('data-bind'));
        var c =parseInt($(e).attr('data-bind'));
        if (c===0) {
            alert('没有数据');
        } else {
            $.ajax({
                url:getXleUrl,
                contentType: 'application/json; charset=utf-8',
                data: { id: $(e).attr('rel')},
                success: function(r) {
                    if (r.success) {
                        window.location = urlPrefix+'GetXls' + "?ListName=" + r.ListName;
                    }
                }
            });
        }

    

    },
    cardEditShow: function (_d) {
        $.get(configEditUrl, { id:_d }).done(function(v) {
            $('#CardEdit').on('shown.bs.modal',
                function () {
                    $('.in .modal-body').html(v);

                    app.utility.valiForm();

                    layui.use('laydate', function () {
                        var laydate = layui.laydate;
                      
                        laydate.render({
                            elem: '.in #Start',
                            min: 0
                            , theme: '#393D49'
                            ,range: '~'
                        });

                        $('.in #Start').bind('change',
                            function() {
                                console.log($(this).val());
                            });

                    });
                    return false;
                }).modal('show');
        });
    },
    cardGen: function (e) {
        var dm = $(e);
        CONFIG_ID = dm.attr('data-config');

        $('#CardGen').on('shown.bs.modal',
            function () {
                $('.in .modal-body .cardprefix').html(dm.attr('data-bind'));
                $('.in .modal-body [name="configId"]').val(CONFIG_ID);
                app.utility.valiForm();
            })
            .on('hidden.bs.modal', function() {
                $('.in .modal-footer').find('.waiting').hide();
            })
            .modal('show');
    }
};


config.ui();

