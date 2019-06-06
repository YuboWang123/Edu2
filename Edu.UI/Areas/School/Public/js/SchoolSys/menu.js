 


var
    menuTbl = $("table"),
 

    domLeft=$('.left'),urlPrfiex = "/school/schoolsys/",
    showSidebar = urlPrfiex + 'menuSidebarTable',
    sideBarAdd = urlPrfiex + "addsidebar",
    sideBarDel = urlPrfiex + "delSidebar",
    sideBarEdit = urlPrfiex + "editSidebarModule",
    topEdit = urlPrfiex + "editTopMenu",
    topDel = urlPrfiex + 'delTopMenu',
    jsTree = urlPrfiex + 'sideBarTree';

    //global var -->node top menu id.
    TopId = 0,
    //gloal var -->node clicked
    NODECLICKED=null;
    

//table row click 
var
    updateTreeView = function () {
        if (TopId === 0) {
            return;
        }
        $.get(jsTree, { id: TopId }).done(function (v) {
            domLeft.html(v);
            JstreeInit();
        });
    },

    tableTR = function () {
        $("table tbody tr").on("click",
            function () {
                $(this).siblings().removeClass("active");
                $(this).addClass("active");
            });
        $("table tbody tr:first").trigger("click");
    },

    doAjaxFn = function (url, data, cb, postOrGet) {
        "use strict";
        if (postOrGet === 'post') {
            $.post(url, data).done(function (v) {
                if (cb) {
                    cb(v);
                }
            });
        } else if (postOrGet === 'get') {
            $.get(url, data).done(function (v) {
                if (cb) {
                    cb(v);
                }
            });
        }
    },
    nodeHasChildren = function () {
        return NODECLICKED.children.length > 0;
    };


//show submenu lst
function showSub(e) {
    var _id = $(e.target).attr("data-id");

    $.get(showSidebar+"/" + _id, null).done(function(v) {
        $("#main").html(v);
        updateTopId(_id);
    });
}
function showTip() {
    $('.tips').html('更新成功！');
}
function hd() {
    $('.tips').html('').hide();
}

//after submitting
function submitOk(data) {
    if (data.i > 0) {
        $('.in').modal('hide');
        $('.frsh').trigger('click');
        
        app.utility.showTips('保存成功');
        //in the partial of menusidebarTable.
        if (JstreeInit!==undefined) {
            updateTreeView();
        }
       
        //setTimeout(hd, 2000);
    } else {
        app.utility.showTips('遇到错误,操作失败');
    }
}

function delOk(d) {
    if (d.i > 0) {
        app.utility.showTips('已删除');
    }
}

 

function delTop(e) {
    var id = $(e.target).attr('data-id'),
        txt = $(e.target).parents('tr').find('td').eq(2).text();
    if (id) {
        $('#topmenuDel').on('shown.bs.modal',
            function () {
                $('.in .modal-body label').html(txt);
                $('.in input[name="id"]').val(id);
            }).modal('show');
    }
    
}

function updateTopId(topId) {
    TopId = topId;
}

function addTop() {
    $("#topMenuAdd").modal("show");
}

function editTop(e) {
    var actTr = $(e.target).parents('tr'),
        topid = actTr.attr("data-bind"),
        editCb = function(v) {
            $("#topmenuEdit").on("shown.bs.modal",
                function() {
                    $(".in .modal-body").empty().html(v);
                });
        };
    if (topid) {
        doAjaxFn(topEdit, { id: topid,d:new Date().getTime() }, editCb, "get");
        $("#topmenuEdit").modal("show");
    }
}


app.utility.valiForm();
tableTR();

menuTbl.find("a.btn-info").on("click", showSub); //show submenus
menuTbl.find("a.btn-default").on("click", editTop);
menuTbl.find("a.btn-danger").on("click", delTop);


$(".addMdl").on("click", addTop);


