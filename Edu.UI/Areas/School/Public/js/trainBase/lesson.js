
 
changableDiv = $('#lsnTbl');

var 
    chatScore = function (looks, behv, wholefeel) {
        this.wareLooks = look || 0;
        this.chatBehavor = behv || 0;
        this.wholeFeels = wholefeel || 0;
        var sc = this;
        if (typeof this.giveScore !== 'function') {
            chatScore.giveScore = function() {
                $.post('/user/review', { review: sc }).done(function(v) {
                    if (v.result === 1) {
                        layer.alert('成功!');
                    }
                });

            };
        }
    },
    bdMdl = {},
    infoStr = [],

    lessonHandler = {
        getInfoStr: function() {
            $('.bd').each(function() { infoStr.push($(this).find('a.active').text()) });
        },
        mgrLesson: function(el) {
            var d = $(el).attr('data-lessonid');
            $(el).parents('tr').siblings().removeClass('active');
            $(el).parents('tr').addClass('active');
            lessonHandler.mgrVcr(d);
        },
        back: function() {
            $('.bck').trigger('click');
        },
        loadingUrl: '/school/trainlesson/index',
        lsnUrl: '/school/trainlesson/addStart', // add/edit a lesson
        lsnEdUrl: '/school/trainlesson/edit',
        lsnVcrUrl: '/school/trainvcr/index', //manage url
        delLsns: '/school/trainlesson/delmany',
        lessonStart: function() {
            $('.bd a').on('click', function() {
                if (!BindFn) {
                    layer.msg('缺少js库,无法运行');
                    return;
                }
                BindFn.search($(this));
            });
            $('.bd:first a:first').trigger('click');

            setTimeout('lessonHandler.getInfoStr()', 1000);

            $('.lessonBtns button:first').on('click', lessonHandler.delLesson);
            $('.lessonBtns button:last').on('click', lessonHandler.addLesson);

        },
        lessonListCallback: function() {
            $('table tr').on('click',
                function() {
                    $(this).siblings().removeClass('active');
                    $(this).addClass('active');
                });

            $('th input[type=checkbox]').on('click',
                function() {
                    app.utility.checkAll();
                });

            $('button.edit').on('click',
                function() { //edit lsn
                    var d = $(this).parent().attr('property');
                    lessonHandler.editLesson(d);
                });
     
        },
        delLesson: function () { //del lesson selected.  
            var tr_checked = $('table#lessonTable  tbody input[type=checkbox]'),arr=[];
            tr_checked.each(function() {
                if ($(this).prop('checked')) {
                    arr.push($(this).attr('property'));
                }
            });
        
            if (arr.length===0) {
                layer.msg('没有选中', { icon: 0 });

            } else if (arr.length > 0) { //lesson deleted bulkly.
                
                $.post(lessonHandler.delLsns, { idList: arr }).done(function(v) {

                    layer.msg('删除了(' + v.i + ' )条记录');
                });

            }
        },
        getSelectCount: function() { //count active a.
            var j = 0, m = lessonHandler.getDataActive();
            for (var i in m) {
                j++;
            }
            return j;
        },
        addLesson: function() { //add lesson with initial parameters.

            if (lessonHandler.getSelectCount() < 4) {
                layer.msg('信息不完整,无法添加.');
                return;
            }
            infoStr = [];
            lessonHandler.getInfoStr(); //reset string inf.

            $.post(lessonHandler.lsnUrl, { model: bdMdl, info: infoStr.join('/') }).done(function(v) {
                $('#main').html(v);
            });
        },
        editLesson: function(id) {
            
            $.get(lessonHandler.lsnEdUrl, { key: id }).done(function(v) {
                $('#main').html(v);
            });

        },
        mgrVcr: function(id) { //go to vcr file resource page.
          
            var lsnNm = $('table tr.active td').eq(1).text(),
                picPath = $('table tr.active td').eq(1).find('img').attr('src');

            $.get(lessonHandler.lsnVcrUrl, { lessonid: id, lsn: lsnNm, pic: '' }).done(function(v) {
                $('#main').html(v);
            });

        },
        getDataActive: function() {
            var mdl = {};
            $('section a.active').each(function() {
                var title = $(this).attr('rel'),
                    id = function() {
                        if (title !== null && String(title).indexOf('_') !== -1) {
                            return title.split('_')[1];
                        }
                        return '';
                    };

                //need insure the downgrade level exists.
                if (title !== undefined && title.indexOf('xd') !== -1) {
                    mdl.periodId = id();
                }
                if (title !== undefined && title.indexOf('nj') !== -1) {
                    mdl.gradeId = id();
                }
                if (title !== undefined && title.indexOf('xk') !== -1) {
                    mdl.subjectId = id();
                }
                if (title !== undefined && title.indexOf('lb') !== -1) {
                    mdl.genreId = id();
                }
            });
            return mdl;
        },

        genLessonTable: function(data) { //show lesson table.
            changableDiv.empty().html(data).slideDown('fast').find('tr:odd').addClass('odd');
            lessonHandler.lessonListCallback();
        },
        getPageList: function(pg) {
            var m = lessonHandler.getDataActive(), j = 0; //i is index of json of the m;

            for (var i in m) {
                j++;
            }
            if (j < 4) {
                changableDiv.empty().html('<p class="text-center">没有记录</p>');
                return;
            }

            bdMdl = new BindingModel(m.periodId, m.gradeId, m.subjectId, m.genreId);

            $.post(lessonHandler.loadingUrl, { model: bdMdl, info: infoStr.join('/') }).done(function(v) {
                lessonHandler.genLessonTable(v);
            });
        }
    };
//only when hit the page
function GetPageList(p) {
   
    var m = lessonHandler.getDataActive(), j = 0; //i is index of json of the m;
    for (var i in m) {
        j++;
    }
    bdMdl = new BindingModel(m.periodId, m.gradeId, m.subjectId, m.genreId);
    if (j < 4) {
        changableDiv.empty().html('<p class="text-center">没有记录</p>');
        return;
    }

    $.post(lessonHandler.loadingUrl, { model: bdMdl, info: infoStr.join('/'), pg:p }).done(function(v) {
        lessonHandler.genLessonTable(v);
    });
}


lessonHandler.lessonStart();


