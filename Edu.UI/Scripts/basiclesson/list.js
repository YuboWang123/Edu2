


if (BindFn) {
    BindFn.serverUrl = "/lesson/getBindings";
}

changableDiv = $('#lsnTbl');

var bdMdl = {},
    infoStr = [],
    _bindId,
    lessonHandler =
    {
        loadingUrl: '/lesson/lessonList',
        lessonStart: function() {
            $('.bd a').on('click', function() { BindFn.search($(this)); });
            $('.bd:first a:first').trigger('click');
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
            $('td a.edit').on('click',
                function() { //edit lsn
                    var d = $(this).parent().attr('property');
                    lessonHandler.editLesson(d);
                });
            $('td a.mgr').on('click',
                function() { //magr vcr list.
                    var d = $(this).attr('data-lessonid');
                    $(this).parents('tr').siblings().removeClass('active');
                    $(this).parents('tr').addClass('active');
                    lessonHandler.mgrVcr(d);
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
            changableDiv.empty().html(data);
            lessonHandler.lessonListCallback();
        },

        getPageList: function(_pg) {

            changableDiv.empty().html('<p class="text-center  pad-top20">没有记录</p>');
            var m = lessonHandler.getDataActive(), j = 0; //i is index of json of the m;

            for (var i in m) {
                j++;
            }
            if (j < 4) {
                return;
            }

            bdMdl = new BindingModel(m.periodId, m.gradeId, m.subjectId, m.genreId);

            $.post(lessonHandler.loadingUrl, { datas: bdMdl, pg:_pg }).done(function (v) {
                lessonHandler.genLessonTable(v);
            });
     

        }
    };
//only when hit the page
function GetPageList(p) {
    lessonHandler.getPageList(p);
}


lessonHandler.lessonStart();
