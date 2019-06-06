
$.validator.unobtrusive.parse($("[id^=form]"));
$.extend($.validator.defaults, { ignore: ":hidden" });

chgPwd = function (d) {
    console.log(d.t);
    if (d.t===0) {
        layer.alert('修改成功');
    }
    if (d.t === 3) {
        layer.msg('新旧密码一样,未更改');
    }
    if (d.t === 4) {
        layer.msg('密码错误,修改失败');
    }
    if (d.t ===2) {
        layer.msg('参数错误,修改失败');
    }
};


