var
    prefix = '/school/schoolsys/',
    delRoleUrl = prefix + 'delRole',
    roleAutheUrl = prefix + 'roleSidebars',
    roleUserUrl = prefix + 'roleUsers',
    roleAdd = prefix + 'addRole',
    editUserRole = prefix +'editUserRole',
    updateAuthUrl = prefix + 'updateRoleMenus',
    tree = $.jstree.reference('jstree-container'),
    currentRole = $('table tbody tr').first().find('.btn-group-xs').attr('data-bind'), //role chosen.
    tbl_tr = $('table tbody tr'), //role list
    currentUserPage = 1, //page index chosen.
    GetPageList=function(a) //get paged list for role users.
    {
        currentUserPage = a;
        $.get(prefix +'roleUserTable', { roleId: currentRole, pg: currentUserPage }).done(function (v) {
            $('#roleUsers').html(v);
        });
    },
    roleHandler = {
        getUsers: function(e) {
            currentRole = $(e).parent('div.btn-group-xs').attr('data-bind');
            $.get(roleUserUrl, { roleid: currentRole }).done(function(v) {
                $('#main').html(v);
            });
        }
    };


var roleMenuHandler = {
    addRole: function() {
        $('#addRoleModel')
            .on('shown.bs.modal', function() {
                app.utility.valiForm();
            })
            .modal('show');
    },
    freshRoleList: function() {
        $('.sBox li.active a').trigger('click');
    },

    freshUserList: function (d) { //fresh paged partial when update user role.
        if (d.i ===1) {
           window.GetPageList(currentUserPage);
            $('.in').modal('hide');
            app.utility.showTipsLayer('修改成功', ENVTYPE.success, 1000);
        }
    },
    changeRole: function (e) {
        var _id = $(e).attr('data-bind');
        $('#chgRoleModel')
            .on('shown.bs.modal', function() {
                $.get(editUserRole, { uid: _id }).done(function(v) {
                    $('.in .modal-body').html(v);
                });
            })
            .modal('show');
    },
    delRoleModalShow: function (e) {
        var roleid = $(e).attr('data-bind'),
              name = $(e).attr('data-bind');
        if (name === 'sys') {
            alert('系统权限不可以删除!');
        }

        if (roleid !== '-1' && name!==undefined) {
            //show modal.
            $('#delRoleModal')
                .on('shown.bs.modal', function () {
                    $('.in .modal-body label').html(name);
                    $('.in [name="id"]').val(roleid);
                })
                .modal('show');
        }
    },
    ui: function () {
     
        var roleDiv = $('#roleDiv'), left = roleDiv.find('div.left'), right = roleDiv.find('div.right');
        
        tbl_tr.on('click',
            function() {
                $(this).siblings().removeClass('active');
                $(this).addClass('active');
            });
        //get authed menu.
        $('.btn-info').on('click',
            function(e) {
                roleMenuHandler.getMenuChecked($(e.target).parents('.btn-group-xs').attr('data-bind'));
                var t = $(this).parents('tr').find('td:first').text();
                $('.title label').html(t);
            });


        tbl_tr.find('.btn-danger').on('click', roleMenuHandler.delRoleModalShow);
        $('.title label').html($('table tbody tr:first').find('td').first().text());
       

        //set layout
        var
            main = $('.left-main').height(),
            btm = $('.bottom .title').height(),
            tilte = $('.topDiv').height();
        $('.left,.right').height(main - tilte - btm);

        tbl_tr.find('.btn-info').first().trigger('click');
    },
    getMenuChecked: function(d) {
        if (d === '-1') {
            tree.uncheck_all();
            for (var i = 0, j = tree.node; i < j; i++) {
                var nd = tree.get_node(i);
                tree.disable_node(nd);
            }
           
            return;
        }

        if (d) {
            $.get(roleAutheUrl, { roleid: d }).done(function(v) {
                tree.uncheck_all();
                if (v.list.length > 0) {
                    var cnt_id=[];
                    $(v.list).each(function () {
                        cnt_id.push('content_' + this);
                    });
                    tree.select_node(cnt_id, false, false);
                }
              
            });
        }
    },
    menuAuthenSubmit: function() {

        currentRole = $('tr.active').find('.btn-group-xs').attr('data-bind');
        
        if (RS === null) {
            return;
        }

        if (currentRole !== '-1') {
            $.post(updateAuthUrl, { sidebar: RS, role: currentRole }).done(function(v) {
                
                if (v.i > 0) {
                    layer.msg('已保存!');
                } else {
                    layer.msg('没有保存!');
                }
                $('.in').modal('hide');
            });
        } else {
            layer.alert('普通用户都不可访问');
        }


    }
 

};
 



roleMenuHandler.ui();