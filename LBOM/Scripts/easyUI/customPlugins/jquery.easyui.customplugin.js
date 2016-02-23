//COLUMNS拖曳移動的功能，需要額外引用

(function ($) {

    var ckName = 'userFields'
    var gridColumns = [[
						{ field: 'code', title: 'Code', width: 100 },
                        { field: 'quantity', title: '數量', width: 100 },
						{ field: 'name', title: '名稱', width: 130 },
						{ field: 'ename', title: '英文名稱', width: 130 }
    ]];
    var columnsFiliters = [
                { field: 'name', type: 'text', precision: 1, op: ['contains', 'equal', 'notequal', 'beginwith', 'endwith', ''] },
                { field: 'quantity', type: 'numberbox', op: ['equal', 'notequal', 'less', 'lessorequal', 'greaterorequal', 'greater', 'contains'] }
    ];
    var gridData = [
							{ 'code': '這是值1', 'name': '這是中文名稱1', 'ename': '這是英文名稱1', 'quantity': 20 },
							{ 'code': '這是值2', 'name': '這是中文名稱2', 'ename': '這是英文名稱2', 'quantity': 11 },
                            { 'code': '這是值3', 'name': '這是中文名稱3', 'ename': '這是英文名稱3', 'quantity': 18 },
                            { 'code': '這是值4', 'name': '這是中文名稱4', 'ename': '這是英文名稱4', 'quantity': 0 },
                            { 'code': '這是值5', 'name': '這是中文名稱5', 'ename': '這是英文名稱5', 'quantity': 5 },
                            { 'code': '這是值6', 'name': '這是中文名稱6', 'ename': '這是英文名稱6', 'quantity': 9 }
    ];


    var ck = Cookies.noConflict();
    var settings = null;
    var pWindow = null;
    var pGrid = null;
    var pText = null;
    //var codeHiddenField = null;

    var methods = {
        //初始化代碼選擇器
        init: function (options) {
            settings = $.extend({
                'textBox': '',
                'pickerWindow': '',
                'pickerGrid': '',
                'pickerText': '',
                'codeColumn': '',
                'title': 'code picker',
                'language': 'chn',
                'prompt': '',
                'data': [],
                'url': ''

            }, options);


            //START--初始化代碼儲存欄位
            if (pText === null) {

                pText = this;
                pText.textbox({
                    editable: true,
                    readonly: false,
                    buttonIcon: 'icon-search',
                    iconAlign: 'right',
                    prompt: settings.prompt,
                    onClickButton: function () {

                        //pWindow.window('open');
                        methods.open();
                    }
                });
            }

            //END--初始化代碼儲存欄位

            //START--初始化easyUI的'window物件
            if (pWindow === null) {
                pWindow = this.after('<div id="' + settings.pickerWindow + '"></div>')
                pWindow.window({
                    width: 600,
                    height: 400,
                    modal: true,
                    minimizable: false,
                    maximizable: false,
                    collapsible: false,
                    title: settings.title
                });

            }//end if

            //END--初始化easyUI的window物件


            //START--初始化easyUI的datagrid物件
            if (pGrid === null) {
                pGrid = pWindow.append('<div id="' + settings.pickerGrid + '"></div>')
                pGrid.datagrid({
                    //url: settings.url,
                    columns: gridColumns,
                    idField: settings.codeColumn,
                    singleSelect: true,
                    rownumbers: true,
                    onSelect: function (index, row) {

                        pText.textbox('setValue', row.code);
                        pText.textbox('setText', settings.language === 'chn' ? row.cname : row.ename);
                        //codeHiddenField.val(row.code);
                        methods.close();
                    },
                    onLoadSuccess: function (data) {



                    },
                    onBeforeDropColumn: function (toField, fromField, point) {
                        pGrid.datagrid('disableFilter');
                    },
                    onDropColumn: function (toField, fromField, point) {
                        var newFields = pGrid.datagrid('getColumnFields');

                        if (ck.getJSON(ckName, { path: '' }))
                            ck.remove(ckName, { path: '' });

                        pGrid.datagrid('enableFilter', columnsFiliters);

                        ck.set(ckName, newFields, { expires: 30, path: '' });

                    },
                    data: gridData
                });


                var cf = ck.getJSON(ckName);
                if (cf) {
                    ck.remove(ckName, { path: '' });
                    var nowColumns = pGrid.datagrid('options').columns[0];
                    var newColumns = [];


                    $.each(cf, function (i, v) {
                        var col = $.grep(nowColumns, function (item) { return item.field == v })[0];
                        if (col)
                            newColumns.push(col);

                    });


                    if (newColumns.length < nowColumns.length) {
                        var tmp = nowColumns;

                        $.each(cf, function (i, v) {
                            tmp = $.grep(tmp, function (item) { return item.field != v });
                        });

                        $.each(tmp, function (i, v) { newColumns.push(v) });
                    }

                    pGrid.datagrid({ columns: [newColumns] });
                }

                ck.set(ckName, pGrid.datagrid('getColumnFields'), { expires: 30, path: '' });
                pGrid.datagrid('columnMoving').datagrid('enableFilter', columnsFiliters);



            }//end if


            //END--初始化easyUI的datagrid物件

            pWindow.window('close');

            if (pGrid === null) {

                pGrid = $('#' + settings.pickerGgrid);
                //pGrid.datagrid();
            }//end if



        },
        //取得所選代碼值
        getvalue: function () {
            return pText.textbox('getValue');
        },
        //開啟代碼選擇器
        open: function () {
            pWindow.window('center');
            pWindow.window('open');
        },
        //關閉代碼選擇器
        close: function () { pWindow.window('close'); }
    };

    $.fn.codePicker = function (method) {

        // Method 呼叫邏輯
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on codePicker');
        }



    };

    //設定欄位過濾器
    function setRowFiliter() {

        pGrid.datagrid('enableFilter',
            [
                { field: 'name', type: 'textbox', precision: 1, op: ['equal', 'notequal', 'less', 'greater'] },
                { field: 'quantity', type: 'numberbox', precision: 1, op: ['equal', 'notequal', 'less', 'greater'] }
            ]);
    }
})(jQuery);