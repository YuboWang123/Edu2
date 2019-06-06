
/* train base list utility.*/
var pfx = '/school/trainbase/',asc=true,cpg=1;
dataFn = {
    chgCb: function() {
        $('.frsh').trigger('click');
    },
    edit: function (d) {
        $.get(pfx + tp + 'Edit', { id: d }).done(function (v) {
          
           $('#' + tp + 'Edit').on('shown.bs.modal',
               function () {
                   $('.in .modal-body').html(v);
               }).modal('show');
        });


        //
        //$('#' + tp + 'Edit').on('shown.bs.modal',
        //    function() {
        //        $.get(pfx + tp + 'Edit', {id:d}).done(function(v) {
        //            $('.in .modal-body').html(v);
        //        });
        //    }).modal('show');
    },
    orderList: function (is_asc, p) {
        if (p!==undefined) {
            cpg = p;
        }
        if (is_asc === 0) {
            asc = false;
        } else {
            asc = true;
        }

        $.get(pfx + tp, { pg:cpg,asc:asc}).done(function(list) {
            $('#main').html(list);
        });

    },
    del: function (d) {
        if (d) {
            layer.confirm('将要删除记录？', {
                btn: ['删除', '取消'] //按钮
            }, function () {
                    $.post(pfx + tp + 'Del', { id: d }).done(function(v) {
                        if (v.t === 1) {
                            layer.msg('有关联数据,不能删除');
                        }
                        if (v.t === 0) {
                            layer.msg('删除成功!');
                            dataFn.chgCb();
                        }
                    });
                },
                function () {
                layer.msg('已取消', {
                    time: 2000 //20s后自动关闭
                });
            });
           
        }
        
     
    }
};