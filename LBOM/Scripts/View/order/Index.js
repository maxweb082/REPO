
var url;

$(function () {


    //訂購datagrid的初始化
    $('#dgOrder').datagrid({
        title: '目前可進行的訂購',
        idField: 'orderID',
        url: '/Order/GetOrderData',
        singleSelect: true,
        pagination: true,
        toolbar: '#tbOrder',
        pageSize: 20,
        columns: [[
            { field: 'orderLoginuserName', title: '訂購發起人', width: 100 },
            {
                field: 'orderStartDatetime', title: '訂購起訖時間', width: 250, formatter: function (value, row, index) {
                    return Common.DateTimeFormatter(value, row, index) + ' ~ ' + Common.DateTimeFormatter(row.orderCloseDatetime, row, index);
                }
            },
            { field: 'shopName', title: '店家名稱', width: 100 },
            { field: 'orderDescript', title: '訂購描述', width: 200 },
            {
                field: 'action', width: 250,
                formatter: function (value, row, index) {

                    var buttons = "";
                    buttons +=
                        row.isClosed == '1' ? '' : '<a href="#" class="easyui-linkbutton" onclick="openOrderItemWindow(this)"> [訂購] </a>';

                    if ($('#nowLogin').val() == row.orderLoginuserID) {
                        var e = '<a href="#" class="easyui-linkbutton" onclick="editrow(this)"> [修改] </a>';
                        buttons += e;
                        if (row.isClosed == '0') {
                            var s = '<a href="#" class="easyui-linkbutton" onclick="stopOrder(this)"> [停止訂購] </a> ';
                            buttons += s;
                        }
                    }

                    buttons +=
                        '<a href="#" class="easyui-linkbutton" onclick="exportExcel(this)"> [匯出訂單] </a>' 
                        //'<a href="#" name="fast" class="easyui-linkbutton" onclick="exportReport(this)"> [開啟訂單報表] </a>'+
                        //'<a href="#" name="dev" class="easyui-linkbutton" onclick="exportReport(this)"> [開啟訂單報表] </a>';

                    return buttons;
                }
            }
        ]]
    });

    //店家資訊comboGrid的初始化
    $('#cgShop').combogrid({
        panelWidth: 500,
        url: '/Order/GetShopList',
        idField: 'shopID',
        textField: 'shopName',
        mode: 'local',
        fitColumns: true,
        columns: [[
            { field: 'shopName', title: '店家名稱', width: 80 },
            { field: 'shopTEL', title: '電話', align: 'left', width: 60 },
            { field: 'shopAddress', title: '地址', align: 'left', width: 60 },
        ]]
    });
});

//-----呼叫函式----------------------------------------------

function openDlg(dlgID, title) {
    $('#' + dlgID).dialog('open');
    $('#' + dlgID).dialog('open').dialog('center').dialog('setTitle', title);
}

function newOrder() {
    openDlg('dlgOrder', '新訂購');
    $('#cgShop').combogrid('grid').datagrid('reload');
    $('#fmOrder').form('clear');
    url = '/Order/NewOrderData';
}

function saveOrder() {
    var val = $('#cgShop').combogrid('getValue');
    $('#shopID').val(val);

    $('#fmOrder').form('submit', {
        url: url,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (result) {

            if (result.length > 0) {
                $.messager.show({
                    title: 'Error',
                    msg: result
                });
            } else {
                $('#dlgOrder').dialog('close');        // close the dialog
                $('#dgOrder').datagrid('reload');    // reload the user data
            }
        }
    });
}

function openOrderItemWindow(target) {
    var index = getRowIndex(target);
    var rowOrder = $('#dgOrder').datagrid('getRows')[index];

    $('#orderID').val(rowOrder.orderID);
    $('#orderItemLoginuserID').val($('#nowLogin').val());

    $('#wOrderItem').window('open');

    $('#dlProduct').datalist({
        url: '/Product/GetProductData',
        queryParams: { shopID: rowOrder.shopID },
        textField: 'productName',
        valueField: 'productID',
        onSelect: function (index, row) {
            $('#fmOrderItem').form('clear').form('load', row);
        }
    });

    $('#dgOrderItem').datagrid({
        singleSelect: true,
        toolbar: '#tbOrderItem',
        showFooter: true,
        columns: [[
            { field: 'productName', title: '餐點名稱', width: 80 },
            { field: 'productPrice', title: '價格', width: 80 },
            { field: 'orderItemQuantity', title: '訂購數量', width: 40, align: 'right', halign: 'center' },
            {
                field: 'subtotal', title: '小計', width: 40, align: 'right', halign: 'center', sum: true, formatter: function (value, row, index) {

                    if (row.productPrice != undefined && row.orderItemQuantity != undefined)
                        return row.productPrice * row.orderItemQuantity;
                    else
                        return row.subtotal;
                }
            },
            {
                field: 'action', title: '', width: 80, formatter: function (value, row, index) {
                    if (row.orderID != undefined)
                        return '<a href="#" class="easyui-linkbutton" iconCls="icon-cancel" onclick="deleteOrderItem(this)">刪除</a>';
                }
            },

            { field: 'orderID', hidden: true },
            { field: 'productID', hidden: true },
            { field: 'orderItemLoginuserID', hidden: true },

        ]]
    });
}


function deleteOrderItem(target) {
    var index = getRowIndex(target);
    $('#dgOrderItem').datagrid('deleteRow', index);

    accountingTotal();
}

function addOrderItem() {
    var qty = $('#orderItemQuantity').val();
    if (qty == '')
        return;

    if (qty <= 0) {
        $.messager.alert('Warning', '請輸入正確的訂購數量');
        return;
    }

    var row = {
        productName: $('#productName').val(),
        productPrice: $('#productPrice').val(),
        orderItemQuantity: $('#orderItemQuantity').val(),
        orderID: $('#orderID').val(),
        productID: $('#productID').val(),
        orderItemLoginuserID: $('#orderItemLoginuserID').val()

    };

    $('#dgOrderItem').datagrid('insertRow', {
        index: 0,
        row: row
    });

    accountingTotal();
}

function getRowIndex(target) {
    var tr = $(target).closest('tr.datagrid-row');
    return parseInt(tr.attr('datagrid-row-index'));
}

function saveOrderitem() {
    var itemRows = $('#dgOrderItem').datagrid('getRows');

    if (itemRows.length == 0) {
        $.messager.alert('Warning', '請至少訂購一筆');
        return;
    }

    $.messager.confirm('Confirm', '確定要送出?', function (r) {
        if (r) {

            var ajaxSets = {
                type: 'POST',
                datatype: 'json',
                url: '/OrderItem/NewOrderItemDatas',
                data: { 'orderItems': itemRows },
                success: function (d, txtStatus, xhr) {
                    if (d.isSuccess) {

                        $('#fmOrderItem').form('clear');
                        $('#fmOrderItem').form('reset');
                        //for (var i = 0; i < rows.length; i++) {
                        //    $('#dgOrderItem').datagrid('deleteRow', i);
                        //}
                        $('#dgOrderItem').datagrid('loadData', { "total": 0, "rows": [], "footer": [] });
                        $('#wOrderItem').window('close');

                        $.messager.alert('資訊', '訂購資訊已儲存成功', 'info');

                    }
                    else {
                        $.messager.alert('錯誤', '訂購資訊儲存失敗：' + d.errorMsg, 'error');
                    }




                },
                error: function (jqXHR, txtStatus, errorThrown) {
                    alert(jqXHR);
                }
            };

            $.ajax(ajaxSets);
        }
    });

}

function accountingTotal() {

    var itemRows = $('#dgOrderItem').datagrid('getRows');
    var total = 0;

    itemRows.forEach(function (element, index, array) {
        total += element.orderItemQuantity * element.productPrice;
    });

    $('#dgOrderItem').datagrid('reloadFooter', [{ productName: '總計:', subtotal: total }]);
}

function exportExcel(target) {
    var index = getRowIndex(target);
    var orderID = $('#dgOrder').datagrid('getRows')[index].orderID;

    var ajaxSets = {
        type: 'GET',
        datatype: 'json',
        url: '/Order/GenExportExcel',
        data: { 'orderID': orderID },
        success: function (d, txtStatus, xhr) {
            window.open('/Order/ExportExcel?ord=' + orderID);

        },
        error: function (jqXHR, txtStatus, errorThrown) {
            $.messager.alert('EXCEL匯出失敗', jqXHR.statusText);
        },
        complete: function () {
            $('')
        }
    };

    $.ajax(ajaxSets);


}

function exportReport(target) {
    var index = getRowIndex(target);
    var orderID = $('#dgOrder').datagrid('getRows')[index].orderID;

    var ajaxSets = {
        type: 'GET',
        datatype: 'json',
        url: '/OrderItem/OpenReport',
        data: { 'orderID': orderID },
        success: function (d, txtStatus, xhr) {
            if(target.name=='fast')
                window.open('/OrderItem/FRPT_OrderItem?orderID=' + orderID);
            else
                window.open('/OrderItem/XtraReport_OrderItem?orderID=' + orderID);

        },
        error: function (jqXHR, txtStatus, errorThrown) {
            $.messager.alert('報表開啟失敗', jqXHR.statusText);
        }
    };

    $.ajax(ajaxSets);
}