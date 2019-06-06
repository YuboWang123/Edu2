
function cityCountry() {
  
    $('.cityCntry .topbar-nav-col a').on('click', function () { //change city id
   
        var prp = $(this).attr('property');
        var t = $(this).find('span').eq(1).text(); //get selct text.
        
        $(this).parents('.panel-body').find('.form-control').eq(2).val(t);

        $(this).parents('.panel-body').find('[name=countryId]').val(prp); 

     
    });
}


function city() {
    $('.citys .topbar-nav-col a').on('click', function () { //change city id
        var prp = $(this).attr('property');
        var t = $(this).find('span').eq(1).text(); //get selct text.

        $(this).parents('.panel-body').find('.form-control').eq(1).val(t);
        $(this).parents('.panel-body').find('[name=cityId]').val(prp); 

        $('input[name=citycntry]').val('');
        $('input[name=countryId]').val('');

       
        //get city country
        $.ajax('/console/school/citycountryselect?cityId=' + prp, null).done(function (v) {
            $('#cityCnyList').html(v);
            cityCountry();
        })

    });
}




function getCity(c) {
    if (c !== undefined) {       
        $.ajax('/console/school/citySelect?pvnId=' + c, null).done(function (v) {
            $('#cityList').html(v);           
            city();
        })
    }
}

var addHandler = {
    valiForm: function () {
       
        $.validator.unobtrusive.parse($("[id*=form]"));
        $.extend($.validator.defaults, { ignore: "" });
    },
    init: function () { 
       addHandler.valiForm();
        //prvn change
        $('.prvn .topbar-nav-col a').on('click', function () {
            var prp = $(this).attr('property');
            var t = $(this).find('span').eq(1).text(); //get selct text.
            
            $(this).parents('.panel-body').find('.form-control').eq(0).val(t);
            getCity(prp);

            $('.panel-body').find('[name=cityId]').val(''); //cls
            $('.panel-body').find('[name=countryId]').val('');

            $('input[name=cityname]').val('');
            $('input[name=citycntry]').val('');

        })

        $('.schtype .topbar-nav-col a').on('click', function () { //school type
            var prp = $(this).attr('property');
            var t = $(this).find('span').eq(1).text(); //get selct text.

            $(this).parents('.panel-body').find('.form-control').eq(3).val(t);
            $(this).parents('.panel-body').find('[name=sctype]').val(prp);
        })

     
    }


}


addHandler.init();