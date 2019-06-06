var 
    page = 1,
    batchType = $('[name="batchType"]').val(),
    ConfigId = $('[name="configid"]').val(),
    status = $('[name="status"]').val(),
    GetPageList = function(p) {
       page = p;
       if (ConfigId && page && status)
       {
            $.get(statusCardListUrl, { statusStr: status, configId: ConfigId, pg:page }).done(function (v) {
                mainPage.html(v);
                return false;
            });
       }
    },
    statusList = {
        bck: function ()
        {
            if (ConfigId) {
                config.getPageList(ConfigId, 1);
            }

        },
        delcards: function ()
        {
            if (cardlist.delCards) {
                cardlist.delCards();
            }
        }

    };


cardlist = $.extend(cardlist,
    cardlist = {
        refresh: function() {
           
        },
        freezeRslt: function (d) {
            var b = $('tr.selected td:last').find('.btn:last');
            console.log(b.length);
            if (d.i===6) {
                b.removeClass('btn-default').addClass('btn-info');
                $('tr.selected').find('.st').text('AdminFreezed');
            } else if (d.i===2) {
                b.removeClass('btn-info').addClass('btn-default');
                $('tr.selected').find('.st').text('InUse');
            }
            $('.in').modal('hide');

            layer.msg('修改成功!');
        }
    });


tableHandler();