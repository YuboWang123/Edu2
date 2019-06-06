

var
    roleUserTable = $('#roleUsers'),
    btnFrsh = $('button.frsh'),
    btnAdd = $('button.addMdl'),
    userListUrl ='/school/schoolsys/roleUserTable',
    GetPagedList = function(p) {
        $.get(userListUrl, { roleId: roleId, pg: p }).done(function(v) {
            roleUserTable.html(v);
        });
    },

RoleUser = {
    userAdded: function (data) {
        if (data.i === 2) {
            layer.msg('参数有误');
        }else if(data.i === 0) {
            GetPagedList(1);
            layer.msg('添加成功');
        } else if (data.i === 3) {
            layer.msg('用户已存在,添加失败！');
        }
    },
    start: function() {
        btnAdd.on('click',
            function() {
                $('#addUser').on('bs.modal.shown',
                    function() {
                        app.utility.valiForm();
                    }).modal('show');
            });

        btnFrsh.on('click',
            function() {
             
            });
    }
};

RoleUser.start();