

var BindingModel = function(xd, nj, xk, arrFeilei) {
    this.periodId = xd;
    this.gradeId = nj;
    this.subjectId = xk;
    this.genreId = arrFeilei;
    this.schoolId = 1;
};

var db = {
    urlsv: '/school/trainbase/saveBinding',
    urlDel: '/school/trainbase/delBinding',
    urlLoad: '/school/trainbase/loadBinding',
    ckUpper: function(dx) {
        if (dx === 0) {
            return true;
        } else { // form a text msg to inform client user.      
            var txt = '';
            for (var i = 0; i < dx; i++) {
                var act = $('section').eq(i).find('a.active').html();
                if (act === undefined || act === null) {
                    if (i !== dx - 1)
                        txt += $('section').eq(i).find('div').eq(0).text() + '=>';
                    else {
                        txt += $('section').eq(i).find('div').eq(0).text();
                    }
                } else {
                    continue;
                }
            }

            if (txt !== '') {
                layer.msg('请选择:' + txt);
                return false;
            }
            return true;

        }

    },
    getSecIndex: function(x) { //get  index till the clicked row.
        for (var i = 0; i < $('div.bd').length; i++) {
            if ($('div.bd').eq(i).attr('name') === x) {
                return i;
            }
        }
    },
    reSetActClass: function() {
        var bgIndex = -1;
        for (var i = 0; i < $('section').length; i++) {
            var thisAct = $('section').eq(i).find('a.active').length;
            if (thisAct === 0) {
                bgIndex = i;
                break;
            }
        }
        if (bgIndex === 0) {
            $('section:gt(' + bgIndex + ')').find('a').removeClass('catch');
        }
        $('section:gt(' + bgIndex + ')').find('a').removeClass('active');

    },
    init: function() {
        $('section a').on('click', this.dataClk);
        $('.delBind').on('click', this.delBinding).on('mouseout',
            function(v) {
                $('.ms').slideUp();
            });
        $('.clrScreen').on('click', this.clrScreen);
        $('.sv').on('click', this.saveBind).on('mouseout',
            function(v) {
                $('.ms').slideUp();
            });
    },
    clrScreen: function() {
        $('section a').removeClass('active').removeClass('catch');
    },
    dataClk: function() {
        var
            arr,
            last_a = '',
            cls = $(this).parents('div').attr('name'),
            indx = db.getSecIndex(cls),
            uperOk = db.ckUpper(indx);

        $('section:gt(' + indx + ')').find('a').removeClass('active');
        if (uperOk) {
            $(this).parents('section').nextAll().find('a').removeClass('catch');
            if (indx !== 3) {
                $(this).parents('li').siblings().find('a').removeClass('active');
            }
            $(this).toggleClass('active');
            db.reSetActClass(); //del active class after the empty row.

            arr = db.getUppers(indx);
            last_a = $('section').eq(indx).find('a.active').attr('rel');
        }
        if (arr !== undefined && last_a !== undefined) {

            db.loadData(arr, last_a);
        }
    },
    delBinding: function() {
        var x = db.getUppers(3), arr = []; //upper 3 lvls of id array.
        if ($('section:last').find('a.active').length > 0 && x.length === 3) {
            $('section:last').find('a.active').each(function() {
                arr.push($(this).attr('rel').split('_')[1]);
            });
            $.ajax({
                url: db.urlDel,
                type: 'post',
                traditional: true,
                data: { clickedId: arr, upperIds: x },
                success: function(v) {
                    if (parseInt(v.t) > 0) {
                        layer.msg('删除了' + v.t + '条记录');
                        $('section a.active').removeClass('active').removeClass('catch');
                    } else {
                        layer.msg('有关联课程,没有删除任何关联');
                    }
                }
            });

        } else {
            db.ckUpper(4);
        }

    },

    getAttr: function(i) {

        if (i > 3) {
            layer.msg('error index in function:getAttr');
        } else if (i === 3) {
            var sct = $('section').eq(i).find('a.active,.catch'), arr = [];
            //console.log(sct);
            sct.each(function() {
                arr.push($(this).attr('rel').split('_')[1]);
            });
            return arr;
        }
        return $('section').eq(i).find('a.active').attr('rel').split('_')[1];
    },
    saveBind: function() {
        var isValid = db.ckUpper(4);
        if (isValid) {
            var bdmdl = new BindingModel(db.getAttr(0), db.getAttr(1), db.getAttr(2), db.getAttr(3));
            $.post(db.urlsv, { BindingModel: bdmdl }).done(function(v) {
                if (v.i >0) {
                  layer.msg('保存成功'+v.i+'条新数据 !');
                } else if(v.i===0){
                    layer.msg('关联已经存在');
                }
            });

        }
    },
    getUppers: function(d) { //d : last active a indx.
        if (d === 0) {
            return [];
        } else {
            var arr = [], list = $("section");
            $.each(list,
                function(m, n) {
                    if (m < d) {
                        var obj = $(n).find("a.active");
                        if (obj.length > 0) {
                            arr.push(obj.attr("rel").split('_')[1]);
                        }
                    }
                });

            return arr;
        }

    },
    loadData: function(arr, last_a) { //download data for lower level
        if (arr === null)
            arr = [];
        $.ajax({
            url: db.urlLoad,
            dataType: 'json',
            traditional: true,
            data: { 'clickedId': last_a, 'upperIds': arr },
            success: function(v) {
                var d = v.result;
                db.setCatched(d.Key, d.Value);
            }
        });
    },
    setCatched: function(k, v) {

        if (k !== null && v !== null) {
            for (var i = 0; i < v.length; i++) {
                $('.bd').find('a[rel=' + k + '_' + v[i] + ']').addClass('catch');
            }

        }
    }

};

db.init();