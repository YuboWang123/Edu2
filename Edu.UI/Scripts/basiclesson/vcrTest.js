
 

function supportsStorage() {
    try {
        return 'sessionStorage' in window && window.sessionStorage === null;
    } catch (e) {
        return false;
    }
}
function TestItem(tid, v) {
    this.Id = tid;
    this.AnswerLetter = v;
}

function Test(opts) {
    this. _op = {
        vid: null
    };

    this._op = $.extend(this._op, opts || {});
}

//Test item class...
Test.prototype = {
    constructor: Test,
    clear: function () {
        var vcrid = this._op.vid;
        sessionStorage.removeItem(vcrid);
    },
    load: function () {
        var tst = this.getTestList();
        if (tst.length>0 ) {          
            return tst;
        }
    },

    add: function (value) {
        var conf = this._op, vid = conf.vid,list;       

        if (!supportsStorage()) {    
            list = this.del(value.Id);
            list.push(value);
            sessionStorage.setItem(vid, JSON.stringify(list));
        } else {
            layer.msg('当前浏览器不适用存储功能 无法进行答题');
            return;
        }

        
        //sessionStorage.setItem(vid, value);
    },

    del: function (testId) {
      
        var list = this.getTestList() , i;
      
        if (list.length === 0) {
            return [];
        }
        else {         
            i =list.findIndex(function (a) {
                return a.Id === testId;
            });

            //console.log(i);
            if (i !== -1) {
                list.splice(i, 1);
            }
            return list;
        }
       
    },
    get: function (testId) {
        var lst = this.getTestList();
        if (lst.length === 0) {
            return null;
        } else {           
            return lst.testId;
        }
    },
    getTestList: function () {
        var vcrid = this._op.vid;       
        if (sessionStorage.getItem(vcrid)!== null) {
            return JSON.parse(sessionStorage.getItem(vcrid));
        }
        return [];     
    },

    getValue: function (testId) {
        var list = this.getTestList();
        return JSON.parse(list).testId;
    },
    getAnswerString: function () {
        var vcrid = this._op.vid;      
        return JSON.stringify(sessionStorage.getItem(vcrid));
    }
};





