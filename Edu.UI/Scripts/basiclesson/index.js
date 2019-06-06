 
tabFunc();

var PayModel = function(b,t,s,n) {
    this.Body = b;
    this.TotalAmount = t;
    this.Subject = s;
    this.OutTradeNo = n;
};




function post(URL, PARAMS) {
    var temp_form = document.createElement("form");
    temp_form.action = URL;
    temp_form.target = "_blank";
    temp_form.method = "post";
    temp_form.style.display = "none";
    for (var x in PARAMS) {
        var opt = document.createElement("textarea");
        opt.name = x;
        opt.value = PARAMS[x];
        temp_form.appendChild(opt);
    }
    document.body.appendChild(temp_form);
    temp_form.submit();
} 


function byLsn(ev) {
    var _addr,lsnAmount=$('li.price').text();
    if (ev.data.en === "ali") {
        _addr = '../../api/alipay';
      
    }
    var pm = new PayModel("课程id", lsnAmount.substr(1), "购买课程", "userid" + new Date().getMilliseconds());
    post(_addr, pm);
    return false;
}

$('ul .btn:contains("支付宝")').on('click', { en:'ali'}, byLsn);

 