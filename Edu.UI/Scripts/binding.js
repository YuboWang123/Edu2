BindingModel = function (xd, nj, xk, fenlei, bid) {
    this.periodId = xd;
    this.gradeId = nj;
    this.subjectId = xk;
    this.genreId = fenlei;
    this.schoolId = 1;
    this.bindId = bid;
},
changableDiv = null,
    BindFn = {
        serverUrl: "/school/trainbase/lessonBindingData",
        bindingCb: null,
        //设置筛选
        search: function(obj) {
            //获取点击的栏目名称
            var column = obj.attr("rel");
            if (column) {
                var colname = column.split('_')[0];
                //判断上一级栏目是否已经选择
                if (colname !== "xd") {
                    if (BindFn.doCheck(obj) === false) {
                        return false;
                    }
                }

                //设置选中状态
                if (obj.hasClass("active")) {
                    obj.removeClass("active");
                } else {
                    obj.addClass("active");
                }

                //判断是否为末级栏目
                if (colname === "lb") {
                    lessonHandler.getPageList(1);
                    return false;
                }
            }
            this.getSelected(obj);
        },
        doCheck: function(obj) {
            var sup = $(obj).parents("section").prev();
            if (sup.find("a.active").length === 0) {
                var name = sup.find("div.hd").text();
                if (name) {
                    name = name.replace("：", "");
                }
                alert("请先选择" + name);
                return false;
            }
        },
        //获取选中的下级栏目
        getSelected: function(obj) {

            //清除选中的下一级
            var next = $(obj).parents("section").nextAll().find("a").removeClass("catched").removeClass("active");

            //获取当前数据
            if ($(obj).hasClass("active")) {
                var objid = $(obj).attr("rel");
                if (objid) {
                    var colname = objid.split('_')[0];
                    if (colname !== "lb") {
                        //获取点击前的上级栏目并记录
                        var arr = [];
                        var list = $(".bd a.active");
                        $.each(list,
                            function(m, n) {
                                if ($(n).attr("rel") !== objid) {
                                    arr.push($(n).attr("rel"));
                                }
                            });

                        BindFn.getData(objid, arr, obj);

                    }
                }
            }
        },
        //获取top选项数据
        getData: function(objid, arr, ctrl) {

            var makeClick = function() {
                //绑定列的点击事件
                var liList = $("section a");
                if (liList.length > 0) {
                    liList.unbind("click").bind("click",
                        function() {
                            //前端设置：选中
                            $(this).parent().siblings().find('a').removeClass("active");
                            BindFn.search($(this));
                            var s = $('.bd a.active');
                            // add Check↓                        
                            var actDiv = $(this).parents("section").prev();
                            if ($(actDiv) !== null && $(actDiv).find('a.active').length === 0) {
                                return false;
                            }
                        });
                }
            };

            if (objid === undefined) {
                return false;
            }

            $.ajax({
                url: BindFn.serverUrl, //get down grade binding data of the lesson partial page.
                dataType: 'json',
                traditional: true,
                cache: true,
                data: { "last_a": objid, 'upperids': arr },
                success: function(v) {
                    if (v.err) {
                        return false;
                    }
                    $(ctrl).parents("section").nextAll().find('ul')
                        .html("<li><a class='noCnt'style='color:#808080'>无关联内容</a></li>");

                    $.each(v.jsn,
                        function(m, n) {
                            var result = "<ul>", k;
                            $.each(n,
                                function() {
                                    k = $(this).attr("Key");
                                    var vl = $(this).attr("Value");
                                    result += '<li><a href="javascript:;" rel="' +
                                        k +
                                        '" title="' +
                                        vl +
                                        '">' +
                                        vl +
                                        '</a></li>';
                                });
                            result += "</ul>";

                            if (k !== undefined) {
                                $("section").find('div.' + k.split('_')[0]).empty()
                                    .html(result); // replace certain downlevel ul       
                            } else {
                                return false;
                            }
                        });

                    makeClick();

                    $(ctrl).parents('section').nextAll().find('ul')
                        .find('li:first-child').find('a').addClass('active'); //get all the first a selected
                    lessonHandler.getPageList(1);
                }
            });
        }

    };