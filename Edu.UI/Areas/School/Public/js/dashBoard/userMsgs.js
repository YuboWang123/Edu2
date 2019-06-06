msgTable = $('.msgTbl');


delMsg = function (_id) {
    if (_id) {
        $.get('/school/dashboard', { id: _id }).done(function (v) {
            if (v.i) {
                console.log(v.i);
            }
        });
    }
}

msgTable.find('.btn-danger').on('click', function () {
    $(this).parents('tr').siblings().removeClass('act');
    $(this).parents('tr').addClass('act');
    //alert('dk');
})