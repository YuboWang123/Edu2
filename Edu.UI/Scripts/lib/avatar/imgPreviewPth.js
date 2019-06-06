var dataURL = '';

function getPath(obj)
{
    if (obj) {
        if (window.navigator.userAgent.indexOf("MSIE") >= 1) {
            obj.select();
            return document.selection.createRange().text;
        }
        else if (window.navigator.userAgent.indexOf("Firefox") >= 1) {
            
            if (obj.files) {
               
                var source = document.getElementById('sourceimg').files[0];                
               
                return window.URL.createObjectURL(source);
            }
             return obj.value;

          //  return window.URL.createObjectURL(obj.files.item(0).createObjectURL());
        } else if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            var f = obj.files[0],src = window.URL.createObjectURL(f);
            return src;
        }
        return obj.value;
    }
}






function upload() {
    
    if (dataURL!=='' && dataURL !== 'data:,') {
      
        var _url = '/base/getBase64Avatar';
        $.ajax({
            url: _url,
            type: 'post',
            data: { source: dataURL },
            success: function(r) {
                if (r.kv.Key === false) {
                    $('.text-info p').html(r.kv.Value + ":保存失败!");
                }
                if (r.kv.Key === true) { //success
                    $('#avatar').empty().html('<img class="cropper img-circle" id="cropper" src="' + r.kv.Value + '" />');
                }
            }
        });

    }
}

function cropperBuild()
{
   
    //$('.text-info p').html('');
    var $image = $("#cropper"),
        $dataX = $("#dataX"),
        $dataY = $("#dataY"),
        $dataHeight = $("#dataHeight"),
        $dataWidth = $("#dataWidth"),
        console = window.console || { log: $.noop },
        cropper;

  $image.cropper({
                aspectRatio: 3/3,
                autoCropArea: 1,
                data: {
                    x: 20,
                    y: 20,
                    width:320,
                    height: 320
                },
                preview: ".preview",
                done: function (data)
                {
                    $dataX.val(data.x);
                    $dataY.val(data.y);
                    $dataHeight.val(data.height);
                    $dataWidth.val(data.width);
                }
                  });

        cropper = $image.data("cropper");

        $("#replace").click(function () {
            $image.cropper("replace", $("#replaceWith").val());
        });

      
        $("#upload-btn").unbind('click').click(function (e) {          
            
            dataURL = $image.cropper("getDataURL");
          
            upload(dataURL);     
        });
}



var imgHolder = $("#preview");

$('.file>:file').change(function () {
    
    var src = getPath(this);
    if ((window.navigator.userAgent.indexOf("MSIE") >= 1)) {
        imgHolder.css({
            "filter": "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + src + "',sizingMethod='scale')"
        });
        imgHolder.css({
            "-ms-filter": "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + src + "',sizingMethod='scale')"
        });
    }
    else {       
        imgHolder.empty().append("<img width='460px' class='cropper' id='cropper' src='" + src + "'/>");
    }
    cropperBuild();
});

