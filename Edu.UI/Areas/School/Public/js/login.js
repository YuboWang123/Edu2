


$('#loginForm .btn')
    .on({
        'mouseover': function () {
        $(this).animate({ opacity: 1 }, 800);
        this.style.boxShadow = "0 0 28px rgba(103, 166, 217, 1)";
    },
    'mouseout': function() {
        $(this).animate({ animate: 'linear', opacity: 0.6 }, 200);
        this.style.boxShadow = 'none';
    }
});

$(".input-group-addon")
    .on('click', function () {
        $(this).find('img')
            .attr('src', "/baseadmin/getRndCode?time=" + (new Date()).getTime());
    });

$('.visible-md p')
    .on('mouseover', function () { $(this).addClass('bck'); })
    .on('mouseout', function () { $(this).removeClass('bck'); });
