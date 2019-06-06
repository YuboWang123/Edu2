
function supportsStorage() {
    try {
        return 'localStorage' in window && window.localStorage=== null;
    } catch (e) {
        return false;
    }
}

 function Test(Id) {
     this.id = Id;      
}

//Test item class...
Test.prototype = {
    add: function (value) {
        this.value = value || [];
        var rmvNull = JSON.stringify(value).replace('null,', '');
        localStorage.setItem(this.id, rmvNull);
    },
    del: function (key) {
        localStorage.removeItem(key);
    },   
    get: function () {        
        return JSON.stringify(localStorage);
    },
    getValue: function (k) {
       return localStorage.getItem(k)||'';         
    }
    
};

function getUserTestResult() {
    var dataObj=[];     
    for (var i = 0; i < localStorage.length; i++) {
        var oj = {};
        oj.Key = localStorage.key(i);        
        oj.Value = localStorage.getItem(oj.Key);
            dataObj.push(oj);
    }     
    return dataObj;
}

//加载用户答案
function userAnswerInit() {
    var userStorage = JSON.stringify(localStorage);
    if (userStorage.length > 0 && userStorage !== {})
    {
     //set the Letter to be 'active';  
        $('.ua p').each(function () {
            setActiveToHtml(this);            
            var ans = $(this).parents('.testLi').find('.answerDiv').find('span.answ').text(), userans = $(this).parents('.testLi').find('.userAnswer').html();
            compLetter(this, userans);
        });
    }
}

function setActiveToHtml(eleLi) {
    var dat = getUserTestResult();    
    $.each(dat, function (k, v) {
        //set active if key equals to the attr        
        if (v.Key === $(eleLi).parents('.testLi').attr('property')) {
            var ansLt = String(v.Value).substr(1, v.Value.length - 2).split('、');
            $(eleLi).parents('.testLi').find('.userAnswer').html(String(v.Value).substr(1, v.Value.length - 2));
            $(ansLt).each(function () {
                $(eleLi).find('a:contains(' + this + ')').addClass('active');
            });
        }
    });  
}
 
//split answer items
function splitAnswerLetter() {
    $('.testLi').each(function () {
        var s = $(this).find('.answer_hid').val();
        var slc = String(s).substring(1, String(s).length - 1);
        var f = slc.split(',');
        var chks = '';
        for (var i = 0; i < f.length; i++) {
            chks += '<a href="javascript:void(0);" class="ck_ans">' + $(f)[i] + '</a>';
        }
        $(this).find('.ua').find('p').empty().html(chks).find('.ck_ans')
            .on('click', function () {               
                  $(this).toggleClass('active'); //add active for the clicked.
            });   
    });
}

//save user answer
function saveAnswer() {     
    var ele = $(this).parents('li.testLi');
    var us = $(ele).find('.ua').find('.active'),_id=$(ele).attr('property');    //get Id.
    var usArray = [];    
    
    $(us).each(function () { //user answer letters Array.
        usArray.push($(this).html());
    });   
    
    $(ele).find('.userAnswer').html(usArray.join('、'));
    //save it to localstorage.
    var testItem = new Test(_id);
    testItem.del(_id);
    testItem.add(usArray.join('、')); 
}


function showTestResult() {
    
    $('div.answerDiv').show(1000);
    compareAnswer();    
}

//copmare answers,give√ || ×--?.
function compLetter(ele, ans,userAnswr) {    
      var domEle= $(ele).parents('.testLi').find('.chk').show();
      if (ans !== userAnswr)
      {          
            $(domEle).html('X').css('color','green');
        } else {
            $(domEle).html("√").css('color','red');
      }  
}

function compareAnswer() { //compare user answer.
    console.log('compare start!');
    var elem = $('div.answerDiv');
    $(elem).each(function () {
        var lbl = $(this).find('lable'), myAnswer = $(this).find('span').eq(1).html();
        console.log('manswer:' + myAnswer);
        if (myAnswer === undefined || myAnswer.indexOf('没有内容') !== -1)
        {
            myAnswer = '';             
        }
        compLetter(this, $(this).find('span').eq(0).html(), myAnswer);
    });
}

var r = 0, w = 0; //r=>right answer,w=>wrong answer.
function resultSpan() {   
    var spnele = $('.MsgLi div').removeClass('hidden').find('span');
    //show the result counted:
    $(spnele[0]).find('em').html(r);
    $(spnele[1]).find('em').html(w);
}

function submitUsrResult() {    //submit test.
    var ansJson = JSON.stringify(localStorage),
        vid = $('#vid').val(), 
        subm = new Test('submit');
        subm.add("true");

        console.log('count:' + getUserTestResult().length);       

        if (getUserTestResult().length>1) {
            $.get('/nettrain/AjaxSubmitTest', { videoId: vid, jsonAnswer: ansJson }).done(function (data) {
                r = data.v;
                w = data.x;        
                resultSpan();                
            });
        }
        resultSpan();
        showTestResult();
}


$('#testPanel .btn-outline-danger').on('click', function () { //重做，删class
    localStorage.clear();//local storage clear
    $('.ua a').removeClass('active'); //answer sheet reset
    $('.MsgLi div').addClass('hidden');  //score panel hide
    $('div.answerDiv').hide().parents('.testLi').find('.chk').hide(); //analysing panel hide & check sign hide
    init(); //reload all
}); 

$('#testPanel .btn-danger').on('click', submitUsrResult);

var init = function () {
    var sb = new Test().getValue('submit');
    splitAnswerLetter();
    userAnswerInit();

    if (sb !== '' && String(sb).indexOf('true') !== -1) {
        console.log('--test is over...');
        showTestResult();
        resultSpan();
    }
    else {
        
        $(".testLi .ua").on('click', saveAnswer);
        $('div.answerDiv').hide();
        $('.chk').hide();
    }

};

init();