var Common = {

    //EasyUI用DataGrid用日期格式化
    TimeFormatter: function (value, rec, index) {
        if (value == undefined) {
            return "";
        }
        /*json格式时间转js时间格式*/
        value = value.substr(1, value.length - 2);
        var obj = eval('(' + "{Date: new " + value + "}" + ')');
        var dateValue = obj["Date"];
        if (dateValue.getFullYear() < 1900) {
            return "";
        }
        var val = dateValue.format("yyyy-mm-dd HH:MM");
        return val.substr(11, 5);
    },
    DateTimeFormatter: function (value, rec, index) {
        if (value == undefined) {
            return "";
        }
        /*json格式时间转js时间格式*/
        value = value.substr(1, value.length - 2);
        var obj = eval('(' + "{Date: new " + value + "}" + ')');
        var dateValue = obj["Date"];
        if (dateValue.getFullYear() < 1900) {
            return "";
        }

        return dateValue.format("yyyy-mm-dd HH:MM");
    },

    //EasyUI用DataGrid用日期格式化
    DateFormatter: function (value, rec, index) {
        if (value == undefined) {
            return "";
        }
        /*json格式时间转js时间格式*/
        value = value.substr(1, value.length - 2);
        var obj = eval('(' + "{Date: new " + value + "}" + ')');
        var dateValue = obj["Date"];
        if (dateValue.getFullYear() < 1900) {
            return "";
        }

        return dateValue.format("yyyy-mm-dd");
    }
};

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(H)、分(M)、秒(S)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(s)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-mm-dd HH:MM:SS.s") ==> 2015-07-02 08:09:04.423 
// (new Date()).Format("yyyy-m-d H:M:S.s")      ==> 2015-7-2 8:9:4.18 
Date.prototype.format = function (fmt) { //author: meizz 
    var o = {
        "m+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "M+": this.getMinutes(), //分 
        "S+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "s": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//调用： 
//var time1 = new Date().Format("yyyy-MM-dd");
//var time2 = new Date().Format("yyyy-MM-dd HH:mm:ss");


var core = {

    //開啟頁簽
    openTab: function (subTitle, url, icon) {
        var tbs = $('#tabs');
        if (tbs.tabs('exists', subTitle)) {
            tbs.tabs('select', subTitle);
        }
        else {
            if (subTitle === 'Home')
                tbs.tabs('select', subTitle);
            else {

                $('#bdy').showLoading();
                tbs.tabs('add', {
                    title: subTitle,
                    closable: true,
                    fit: true,
                    content: '<iframe scrolling="auto" frameborder="0"  style="width:100%;height:100%;" onLoad="closeTabLoading()" src="' + url + '" ></iframe>',
                    iconCls: icon
                });
            }
        }
    },

    //初始化MENU
    initMenu: function () {
        var tm = $('#tt');


        var nodes = [
            {
                text: 'Home', iconCls: 'icon-home', state: 'open', attributes: { url: '/home/index' },
                children: [
                  { text: '店家資訊維護', iconCls: 'icon-application-form', attributes: { url: '/shop/index' } },
                  { text: '訂購資訊維護', iconCls: 'icon-application-form', attributes: { url: '/order/index' } },
                  { text: '部門資訊維護', iconCls: 'icon-application-form', attributes: { url: '/dept/index' } }
                ]
            }
        ];

        tm.tree({
            data: nodes,
            onClick: function (target) {
                core.openTab(target.text, target.attributes.url, target.iconCls);
            }
        });



    }


};





