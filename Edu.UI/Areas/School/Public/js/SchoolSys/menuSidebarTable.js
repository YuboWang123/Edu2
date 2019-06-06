
var
    module,
    urlPrfx = '/schoolSys',
    topDiv = $('.topDiv'),
    right = $('.right'),
    getNodeId = function (data) {
    if (data && data.indexOf('_') !== -1) {
        return data.split('_')[1];
    } else {
        alert('无法得到合适的数字参数');
    }
    },
    leftTree = $.jstree.reference('jstree1'),
    ///outerDom ==>dom id, innerClass==>dom class,v==>value
    modalChanger = function (outerDom, innerClass, v) {
        console.log(v);
    $(outerDom)
    .on('shown.bs.modal',
        function () {
            if (innerClass)
                {
                    app.utility.valiForm();
                    $('.in .modal-body').find(innerClass).val(v);
                }
            })
            .modal('show');

};
 
module = {
    showModalDialog: function (el) {
        //set parent node in the parial view of editcontent.
        $('.moduleNew .jstreeModule')
            .on('changed.jstree', function (e, data) {
                var k = data.instance.get_node(data.selected[0]);
                 
                if (k.parent === 'node_1') {
                    $(el).val(k.text.trim());
                    $(el).parents('.form-horizontal')
                        .find("input[name='ConsoleSideMenu.ModuleId']")
                        .val(getNodeId(k.id));
                }
            })
            .jstree();
        $('.moduleNew').toggle();
        return false;
    },
    ui: function() {
        //height reset.
        var
            main = $('.left-main').height(),
            btm=$('.bottom .title').height(),
            tilte = topDiv.height();
        $('.left,.right').height(main - tilte-btm);
    },
    //open the jstree when click the final input
    addModule: function () {
        if (TopId) {
            modalChanger('#AddModule', '.consoleTopMenuId', TopId);
        }
          
 
    },
    delModule: function () {
       
        modalChanger('#delModule', '[name=id]', getNodeId(NODECLICKED.id));
    },
    editModule: function () {
        if (NODECLICKED) {
            var
                tp = module.getNodeType(),
                getPartial = function (_id) {
                    //change the modal body html .
                    $.get(sideBarEdit, { id:_id})
                        .done(function (v) {
                            $('#editModule').on('shown.bs.modal',
                                function () {
                                    $('.in .modal-body').html(v);
                                }).modal('show');
                            app.utility.valiForm();
                        });
                };

            if (tp === 'content') {
                if (NODECLICKED.parent) {
                    var prnt = NODECLICKED.parent;
                    getPartial(getNodeId(prnt));
                }
            } else if (tp === 'module') {
                console.log(NODECLICKED.id);
                getPartial(getNodeId(NODECLICKED.id));
            } else {
                if (confirm('是否需要修改顶部导航')) {
                    module.bck();
                }
            }
        }

   
    },
    addContent: function () {
         
        var sele = leftTree._model.data;
        if (sele.module_0) { //no module still
            
            if (confirm('先增加一个目录模块')) {
                module.addModule();
            }

        } else { //has module already.
            console.log(NODECLICKED);
            if (!NODECLICKED) {
                //get first node id that stars with module.
                var first = '';
                for (obj in leftTree._model.data) {
                    if (String(obj).indexOf('module') !== -1) {
                        first = obj;
                        break;
                    }
                }
                //add content under the first module
                leftTree.select_node(first);
            } 
            modalChanger('#AddSideBarContent', null, null);

        }
    },
    delContent() {
        modalChanger('#delSideBarContent', '[name=id]', NODECLICKED.id);
    },
    bck: function() {
        $('.sBox li.active').trigger('click');
    },
    //get node clicked type
    //content, module,top
    getNodeType: function() {
        var result = 'content';
        if (NODECLICKED)
        {
            if (NODECLICKED.id === 'node_1') {
                result = 'top';
            } else if (NODECLICKED.parent === 'node_1') {
                result = 'module';
            } 
        } else if (isDefaultOnly()) { //only default node 'module_0'.
            
            result = 'firstModule';
        }
        else {
            alert('没有选中的节点,请点击左侧列表');
        }
        return result;
    },
    contentFuncs: function () { //del module or del content
        if (NODECLICKED) { //first child clicked
            var tp = module.getNodeType();
            if (tp === 'content') {
                module.delContent();
            } else if (tp === 'module') {
                module.delModule();
            }

        } else { //no first child
            
        }
    }
};
app.utility.valiForm();
module.ui();
 