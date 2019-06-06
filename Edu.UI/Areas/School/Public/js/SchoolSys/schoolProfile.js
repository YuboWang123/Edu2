var
    prefix = '/school/schoolsys/';

$.validator.unobtrusive.parse($("[id^=form]"));
$.extend($.validator.defaults, { ignore: ":hidden" });
var isNew = maker==='0';

if (isNew) {
    layer.msg('请输入您的学校信息！');
}

function callback(ev) {
    if (ev === 'success') {
        layer.msg('已保存');
    } else {
        layer.msg(ev);
    }
}


