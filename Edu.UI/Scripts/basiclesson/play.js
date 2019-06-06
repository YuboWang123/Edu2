
var Tst, VcrId, sbmitBtn = $('.infoPanel .btn-warning'), resetBtn = $('.infoPanel .btn-default'),fileLink=$('.fileRow'), chckOk=$('.infoPanel .col-md-2');
Media = document.getElementById("media");
//play event
var eventTester = function (e) {
   
    Media.addEventListener(e,
        function() {
            console.log((new Date()).getTime(), e);
        },
        false);
};

function AnswerReset() {
    var b = confirm('会删除已经完成的选项 ,继续?');
    if (b) {
        $('.testitem a.act').toggleClass('act');
        $('.check').removeClass('rightAnswer').hide().html('<i class="glyphicon glyphicon-remove"></i>');
        chckOk.find('span').html(0);
        Tst.clear();
    }
    return;
}

function submitTest() {
    var w = Tst.getTestList(), g = 0;
    if (w === null) {
        return;
    }


    $.post('/lesson/testSubmit', { vcrid: VcrId, answers: w }).done(function(v) {
       
        if (v.OperResult === 4) {
            layer.msg('请先登录');
            return;
        }
        if (v.OperResult === 0) {
            $('.check').show();
            g = JSON.parse(v.Message);
            //console.log(g);
            if (g.length > 0) {
                for (var i = 0; i < g.length; i++) {
                    console.log(g[i]);
                    var d = $('[property=' + g[i] + ']');
                    d.find('.check').html('<i class="glyphicon glyphicon-ok"><i>').addClass('rightAnswer');
                }
            }

            $('.infoPanel .col-md-2').eq(0).find('.success').html(g.length);
            $('.infoPanel .col-md-2').eq(1).find('.failed').html($('.testitem').length - g.length);
        }
    });
    return false;

}

  //inital the test item with user answers
function TestLoad(vcrId) {
    if (Test !== undefined) {  
        Tst = new Test({ vid:vcrId});
        var aswDom = $('.choices'),testAnswers=Tst.load(),

            //get previous choices.
            userChosed = function (testId) {
                if (testAnswers===undefined || testAnswers.length === 0) {
                    return;
                }
                for (var i = 0; i < testAnswers.length; i++) {
                    if (testAnswers[i].Id === testId) {
                        return testAnswers[i].AnswerLetter;
                    }
                }

            };


        aswDom.each(function() {
            var s = $(this).attr('property').split(','),
                m = '',
                answerSheet = $(this).find('p'),
                _id = $(this).parents('.testitem').attr('property');
            //set the sys choices
            for (var i = 0; i < s.length; i++) {
                m += '<a href="javascript:;" >' + s[i] + '</a>';
            }
          
            answerSheet.html(m);

            //get user choices
            var preChoice = userChosed(_id);
            //make pre-choices choosed
            if (preChoice !== null && String(preChoice).indexOf(',') !== -1) {
                var arr = preChoice.split(',');
                $(arr).each(function() {
                    answerSheet.find('a:contains(' + this + ')').addClass('act');
                })
            }
            if (preChoice !== undefined && preChoice.length === 1) {

                $(this).find('p').find('a:contains(' + preChoice + ')').addClass('act');
            }


            $(this).find('p').find('a').click(function() {
                var thisId = $(this).parents('.testitem').attr('property');
                $(this).toggleClass('act');
                //log the choice
                var slected = $(this).parent().find('.act'),
                    asr = '',
                    myAnswerSplit = function() {
                        slected.each(function() {
                            asr += $(this).text() + ',';
                        });
                        asr = asr.substr(0, asr.length - 1);
                    };
                myAnswerSplit();
                if (asr !== '') {
                    Tst.add(new TestItem(thisId, asr));
                } else {
                    console.log(asr);
                }


            });

        });


    } else {
        layer.msg('Test is undefined,test can\'t start ');
    }

    $('.infoPanel .btn-warning')
        .on('click', function () {
            var str = Tst.getTestList();
            return false;
         
        });

    $('.infoPanel .btn-default').on('click', AnswerReset);
}

function downLoad(file) {
    if (file) {
        var outPortLocation = '/download/getResources';
        $.ajax({
            url: outPortLocation,
            contentType: 'application/json; charset=utf-8',
            data: { fileId:file},
            success: function (r) {
                console.log(r);
              
                if (r.OperResult === 4) {
                    layer.msg('请先登录');
                    return;
                }
                if (r.OperResult===0) {
                    window.location ="/download/getFile?filename=" + r.Message;
                } else {
                    alert('遇到错误.');
                }
            }
        });
  
    }
}

function init() {
    tabFunc();
    eventTester('play');

    eventTester("ended");


    $('.playList li').on('click',
        function () {
            layer.load(1,
                {
                    shade: [0.1, '#fff'] //0.1透明度的白色背景
                });
            VcrId = $(this).attr('property');
            $(this).siblings().removeClass('act');
            $(this).addClass('act');
            //change video for playing.
            $.get('/lesson/getPath', { id: VcrId }).done(function (v) {
                 
                if (v.OperResult ===4) {
                    layer.msg('请先登录');
                    layer.closeAll('loading');
                    return;
                }

                if (!v.Message) {
                    layer.msg('路径错误');
                    layer.closeAll('loading');
                    return;
                }

                if (v.OperResult === 3) {
                    layer.msg('记录不存在!');
                    layer.closeAll('loading');
                    return;
                }

                if (v.OperResult === 0) {
                    layer.closeAll('loading');
                    if (v.Message !== null)
                        $('.videoDiv').find('video').attr('src', v.Message);
                    else {
                        $('.videoDiv').find('video').attr('src', '');
                    }
                }

                if (v.OperResult === 5) {
                    layer.alert('尚未购买本课程', {
                          time: 0 //不自动关闭
                        , btn: ['是', '否']
                        , yes: function (index) {
                            window.location = '/lesson/index/'+lsn;
                        },
                        btn2: function() {
                            layer.closeAll('loading');
                        }

                    });
                 
                }

            });

            ///change content
            $.get('/lesson/playContent', { vcr: VcrId }).done(function(v) {
                $('.rest>div').html(v);
                tabFunc();
                TestLoad(VcrId);
                $('.infoPanel .btn-warning').click(submitTest);
                $('.fileRow').on('click',
                    function () {
                        downLoad($(this).find('a').attr('property'));
                    });
            });

        });



    VcrId=  $('.playList li:first').attr('property');
    TestLoad(VcrId);
    //test submit
    function submitCallBack(successed,errors) {
        $('.infoPanel').find('span.success').text(successed),
            fa = $('.infoPanel').find('span.failed').text(successed);

    }

    sbmitBtn.unbind('click').on('click', submitTest);
    resetBtn.unbind('click').on('click', AnswerReset);
    fileLink.on('click',
        function() {
            downLoad($(this).find('a').attr('property'));
        });

  


}

init();

