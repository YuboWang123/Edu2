
$('dt a:contains("AdminFreezed")').text('冻结');
$('dt a:contains("Freezed")').text('锁定');
$('dt a:contains("InUse")').text('使用中');
$('dt a:contains("Created")').text('正常');
$('dt a:contains("NeverUsed")').text('未使用');
$('dt a:contains("Deleted")').text('删除');
$('dt a:contains("Sold")').text('售出');
$('dt a:contains("Outdated")').text('过期');

var
    delSingleUrl = urlPrefix + 'delCard',
    freezeCard = urlPrefix + 'freezeCard',
    cardListUrl = urlPrefix + 'cardList',
    statusCardListUrl = urlPrefix +'statusCards',
    STATUS = '',
    STATUS_PG='', //when paging the status list.
    GetPageList = function (p) {
            currentPage = p;
        if (CONFIG_ID) {
            if (STATUS === '') {
                $.get(cardListUrl, { id: CONFIG_ID, pg: currentPage }).done(function(v) {
                    mainPage.html(v);
                    return false;
                });
            }  
        }
    },
    cardlist = {
        //get status card list
        getStatusList: function (e) {
           
            STATUS = $(e).parents('dl').attr('class');
            if ($(e).parents('dl').find('dd').text() === '0') {
                layer.msg('没有数据.');
            } else {
                $.get(statusCardListUrl, { statusStr: STATUS, configId:CONFIG_ID, pg: 1 }).done(function (v) {
                    mainPage.html(v);
                    return false;
                });
            }
        },
        refresh: function() {
            GetPageList(currentPage);
        },
        //get check id list.
        getCheckedId: function () {
            var chk_value = [], idlist = $('table tbody input[name="Id"]:checked');
            if (idlist) {
                idlist.each(function () {
                    chk_value.push($(this).val());
                }); 
            }
            return chk_value;
        },
        clear: function () {
            if (CONFIG_ID) {
                $.get(urlPrefix + 'clear', { configid: CONFIG_ID }).done(function (v) {
                    if (v.i === 0) {
                        layer.msg('已全部删除');
                    } else {
                        layer.msg('失败');
                    }
                });
            } else {
                layer.msg('参数Config 没有定义');
            }
        },
        freezeRslt: function (d) {
            cardlist.refresh();
            $('.in').modal('hide');
        },
        freezeCard: function (e) {
            var id = $(e).attr('data-bind');

            if ($(e).hasClass('btn-default'))
            {
                $('#freezeCard').on('shown.bs.modal',
                    function () {
                        $('.in .modal-body').find(':hidden').val(id);
                    }).modal('show');
            }
            else if ($(e).hasClass('btn-info'))
            {
                $('#unfreezeCard').on('shown.bs.modal',
                    function () {
                        $('.in .modal-body').find(':hidden').val(id);
                    }).modal('show');
            }
        },
        delSingle: function (_d) // del a cardList record with class'selected'
        {
            if (!_d) {
                return;
            }
           
            $.ajax({
                url: delSingleUrl,
                data: { id: _d },
                success: function (v) {
                    if (v.i > 0) {
                        $('tr.selected').hide(1000).remove();
                        layer.msg('已删除');
                    }
                }
            });

        },
        delCards: function () {
            var ids = cardlist.getCheckedId();
          
            if (ids.length > 0) {
            
                $.ajax({
                    url: urlPrefix+'delcards',
                    traditional: true,
                    data: { Ids: ids },
                    success: function (x) {
                        layer.msg('已删除' + x.i + '个卡记录');
                        cardlist.refresh();
                    }
                });
            } else {
                alert('没有选中任何记录');
            }
        }

    };


tableHandler();