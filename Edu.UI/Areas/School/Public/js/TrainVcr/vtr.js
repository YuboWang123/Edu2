
downTemp = function () {
    var outPortLocation = '/download/getFile';
    $.ajax({
        url: '/download/vcrTestTpl',
        contentType: 'application/json; charset=utf-8',
        data: null,
        success: function (r) {
            if (r.success) {
                window.location = outPortLocation + "?filename=" + r.fileName;
            } else {
                alert('遇到错误.');
            }
        }
    });

};

