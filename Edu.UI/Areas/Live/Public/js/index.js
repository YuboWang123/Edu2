
var callback = function (data) {
        console.log(data);
        if (data.i ===1) {
            layer.alert('start download android app');
        } else {
            layer.alert(data);
        }
},
    transBytes = function (data, fileName) {
    if (!data) {
        return;
    }
    let url = window.URL.createObjectURL(new Blob([data]));
    let link = document.createElement('a');
    link.style.display = 'none';
    link.href = url;
    link.setAttribute('download', fileName);
    document.body.appendChild(link);
    link.click();
    link.remove();
},
    downloadPuaher = function () {
        console.log('start download');
        $.ajax({
            url: '../../api/file?name=pusher',
            type:'get',
            contentType: 'application/octet-stream',
           
            success: function (r) {
                transBytes(r,'pusher.sdk');
                console.log(r.length);
            }
        });
    };

layui.use('laydate', function () {
    var laydate = layui.laydate;
    laydate.render({
        elem: '#StartDate'
    });

    laydate.render({
        elem: '#TimeDuration'
        , type: 'time'
        , range: true
    });

    $('.btn-success').on('click', downloadPuaher);

});








