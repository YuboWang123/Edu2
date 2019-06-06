 
//left list refresh
GetPageList = function(p) {
    vcrFns.reloadLeft(p);
};

var
main = $('.maincontent'),
    AREA='/school',
    vcrEditUrl = AREA + '/trainvcr/vcredit',
    vtrUrl = AREA + '/trainvcr/vtr',
    resUrl = AREA + '/trainvcr/vcr_resource',
    testUrl = AREA + '/trainvcr/vcr_test',
    leftTableUrl = AREA + '/trainvcr/indexleft',
    vid = $('.layui-table tbody tr:first').attr('property'),
    
vcrFns =
{
    addVcr: function(parameters) {
        $('#add').on('show.bs.modal',
            function() {
                $(':input[type="text"]').val('');
                $("input[type='checkbox']").attr('checked', false);
                console.log('before modal works');
            }).modal('show');
    },
    genResTable: function() {
        var layTable = function() {
            layui.use(['table', 'jquery'],
                function() {
                    var layTbl = layui.table;
                    layTbl.init('fileList',
                        {
                            height: 315, //设置高度,
                            limit: 10
                        });
                });
        };
            $.get(resUrl, { vcrid: vid }).done(function (v) {
            $('.fileList').html(v);
            vcrFns._delVCR();
        });
    },
    del: function(vcr) {
        event.preventDefault();
        if (vcr !== '') {
            layer.confirm('删除此条的信息？',
                {
                     title:'删除',
                     btn: ['确定', '取消'] 
                },
                function (index, layero) {
                    $.get('/school/trainvcr/del', { key: vcr }).done(function (v) {
                        console.log(v.i);
                        if (v.i === 0) {
                            $('.left tr.act').hide().remove();
                            layer.msg('删除成功');
                            if ($('.left .layui-table tbody tr').length === 0) {
                                $('.left .layui-table tbody').append('<tr><td class="noCnt text-center">没有记录</td></tr>')
                            }
                        } else if (v.i === 1) {
                            layer.alert('相关资源没有删除,请先删除试题或相关的资源.', {icon:5});
                        }
                        else {
                            layer.msg('删除失败', { icon: 5 });
                        }
                    });
                    layer.close(index);
                },
                function (index) {
                  layer.close(index);
                });
        }
        return false;
    },
    partialVTR: function () { //get all lists   

        if (!vid) {
          //layer.msg('无参数,无法加载');
        } else {
            //get Vtr partial view.
            $.get(vtrUrl, { id: vid }).done(function (v) {
                $('.uploaders').html(v);
                vcrFns._uiRgt();
            });
        }
    },
    _delVCR: function () {
        var evn = 'mouseenter'
            , evnRetrv = 'mouseleave'
            , delDom =
              ' <button class="btn btn-danger  btn-xs btnDel" ><i class="glyphicon glyphicon-trash"></button>';

        //mouse events
        function msIn() {
            $(this).css('position', 'relative');
            $(this).prepend(delDom);
            $('.btnDel').show().unbind('click').on('click',
                function (e) {
                    e.preventDefault();
                    var tp = $('.nav-tabs>li.active').attr('property'),k = $(this).parent('div').attr('property');
                    $(this).parent('div').removeClass('deled');
                    $(this).parent('div').addClass('deled');
                    $(this).parents('.media').addClass('deled');
                    vcrFns. _delItem(tp, k);
                });
            }

        function msOut(event) {
            $(this).css('position', 'initial');
            $(this).parents('div').find('.btnDel').remove();
        }


        $('.fileList>.media').on(evn, msIn).on(evnRetrv, msOut);

        $('.testList>div').on(evn, msIn).on(evnRetrv, msOut);

        $('div.videoDiv').on(evn, msIn).on(evnRetrv, msOut);

    },
    uploaderInit: function (typ, cntrol) {
       var uploadCallback =
        {
            //upload  file, vcr and test  callbacks.
             genVcrDm: function (p) {
                if (String(p).startsWith('\/')) {
                    var outDm = $('.tab-content>div:first'),
                        dom = '<div class="videoDiv" property="' +
                            vid +'"><video class="video" src="' +
                            p +'" controls /></div>';
                    outDm.empty().html(dom);
                    vcrFns._delVCR();
                }
              },
           genFileDm: vcrFns.genResTable(),
           genFilesTable: function() {
               vcrFns.genResTable();
           },
             genTstDm: function () {
                $.get(testUrl, { vcrid: vid }).done(function (v) {
                    $('.testList').html(v);
                    vcrFns._delVCR();
                });
            }
        };

        if (VcrFileUploader) {
           VcrFileUploader(typ, cntrol, uploadCallback, { 'vid': vid });
        } else {
           layer.msg('错误提示:插件没有找到');
        }

    },
    _delRes: function (el) {
        var k = -1;
        if (el) {
            k = $(el).attr('data-id');
            $(el).parents('tr').siblings().removeClass('deled');
            $(el).parents('tr').addClass('deled');
        }
        vcrFns._delItem('resource', k);
    },
    _delItem: function (tp, key) {

    if (key === '' || tp === undefined) {
        layer.alert('参数为空,无法删除!');
        return;
    }
        var url = '/school/',delCb = null;
        switch (tp)
        {
            case "vcr":
                url += 'trainvcr/delVideo',
                delCb = function () {
                    var vcrDom = '<div class="title" onclick="">上传教学视频</div>';
                    vcrDom +=
                        '<input id="UploadVcr" name="" class="file" value="" type="file" data-max-file-count="1">';
                    vcrDom += ' <p class="margin-large-top">没有视频信息</p>';
                    $('.tab-content>div:first').empty().html(vcrDom);
                    vcrFns._uploaderInit(key, 'vcr', 'UploadVcr');

                    $('section div.title')
                        .on('mouseenter',
                            function () {
                                $(this).remove('i').prepend('<i class="glyphicon glyphicon-plus"></i>');
                                return false;
                            }).on('mouseleave',
                                function () {
                                    $(this).parent().find('i').eq(0).remove();
                                    return false;
                                }).on('click',
                                    function () {
                                        var tp = $(this).parent().attr('property');
                                        switch (tp) {
                                            case 'vcr':
                                                $(this).parent().find('[id=UploadVcr]').trigger('click');
                                                break;
                                        }
                                    });
                };
                break;
            case "test":
                url += 'vcrtest/del',
                delCb = function (r) {
                    if (r === 1)
                        $('.deled').slideUp('slow').remove();
                    layer.msg('已删除');
                };
                break;
            case "resource":
                url += 'vcrfiles/del',
                    delCb = function (r) {
                        if (r === 1)
                        $('.deled').slideUp('slow').remove();
                        layer.msg('已删除');
                    };
                break;
            default:
        }
        $.post(url, { id: key }).always(function (v) {
            if (v.i === 1) {
              delCb(v.i);
            } else {
              layer.msg('失败');
            }
        });
    },
    _uiRgt: function () {
        $('.nav-tabs li:first').find('a').trigger('click');
        vcrFns.uploaderInit( 'vcr', 'UploadVcr');
        vcrFns.uploaderInit( 'resource', 'res');
        vcrFns.uploaderInit( 'test', 'test');

        $('section div.title')
        .on('mouseenter',
        function () {
            $(this).prepend('<i class="glyphicon glyphicon-plus"></i>');
            return false;
        })
        .on('mouseleave',
        function () {
            $(this).parent().find('i').eq(0).remove();
            return false;
        })
        .on('click',function () {
            //open uploader.
            var tp = $(this).parent().attr('property');
            switch (tp) {
                case 'vcr':
                    $(this).parent().find('[id=UploadVcr]').trigger('click');
                    break;
                case 'test':
                    $(this).parent().find('[id=test]').trigger('click');
                    break;
                case 'resource':
                    $(this).parent().find('[id=res]').trigger('click');
                    break;
            }
        });

        vcrFns._delVCR();

      },
    init: function () {

          $('#vcrPanel>div').height(main.height());
        $(window).resize(function () {
          $('#vcrPanel>div').height(main.height());
        });

    //del vcr record in the left part.
    $('.left .del').on('click',
        function () {
        var vidArr = [], chk = $('table tr input[type=checkbox]'); //vcr tr.

        if ($("input[type='checkbox']:checked").length === 0) {
            layer.alert('没有选择任何记录');
            return;
        } else {
            chk.each(function () {
                if ($(this).is(':checked')) {
                    vidArr.push($(this).parents('tr').attr("property"));
                }
            });
        }

        if (vidArr.length > 0 && confirm('将删除相关的资源,试题及视频信息,是否继续?')) {
            // del start
            $.ajax({
                url: '/school/trainvcr/delMany',
                traditional: true,
                data: { vids: vidArr },
                success: function (v) {
                    if (v.v > 0) {
                        $('table tr input[type=checkbox]').each(function () {
                            if ($(this).is(':checked')) {
                                $(this).parents('tr').remove();
                            }
                        });

                        if ($('table tr').length === 0) {
                            $('table tbody')
                                .append('<tr class="text-center"><td colspan="3">没有记录</td></tr>');
                        } else {
                            var d = $('table tr:first').attr('property');
                            vcrFns._getVTRs(d);
                        }

                    }
                },
                error: function () {
                   layer.alert('ajax error');
                }
            });

        }
        });

    $('.bck').on('click',
    function () {
        $('.sBox li.active a').trigger('click');
        });

        vcrFns.reloadLeft(1);
    },
     addCb: function (data) {
        if (data.id !== '' && data.id !== undefined) {
            vid = data.id;
            vcrFns.reloadLeft();
            vcrFns.partialVTR();
            $('.in').modal('hide');
        }
    },
     goLsn: function (lsnId) { //edit lesson record.
        if (lsnId !== undefined && lsnId !== '') {
        $.get('/school/trainlesson/edit', { key: lsnId }).done(function (v) {
        $('#main').html(v);
        });
        }
    },
    ed: function (elm) {
        var d = elm.parents('tr').attr('property');
        if (d !== undefined) {
            $.get(vcrEditUrl, { key: d }).done(function (v) {
                $('#edit')
                    .on('show.bs.modal', function() {
                        $(':input[type="text"]').val('');
                        $("input[type='checkbox']").attr('checked', false);
                        console.log('before modal works');
                    })
                    .on('shown.bs.modal',
                    function () {
                        $(this).find('.modal-body').html(v);
                        app.utility.valiForm();
                    }).modal('show');
            });
        }
        event.preventDefault();
        return false;
    },
    reloadLeft: function (p) {
        if (p && p.i > 0) {
            layer.msg('保存成功！');
            $('.in').modal('hide');
        }

        if (p === null) {
            p = 1;
        }
      
        if (lessonId) {
            $.get(leftTableUrl,
            {
                lsnId: lessonId,
                pg: p
            })
            .done(function (v) {
                //change the doms
                $('#leftVcrs').html(v);
                //hook the events.
                $('.layui-table tbody tr').unbind('click').on('click',
                    function () {
                        $(this).siblings().removeClass('act');
                        $(this).addClass('act');
                        vid = $('.act').attr('property');
                        vcrFns.partialVTR(); //get right Partial view. 
                        return false;
                    }).on('mouseover', function() {
                    
                });
                $('.layui-table tbody tr:first').trigger('click');
            });
        }
    }
};



vcrFns.init();





