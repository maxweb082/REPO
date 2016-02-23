var url;
function newShop() {
    $('#dlg').dialog('open').dialog('center').dialog('setTitle', '新增店家資料');
    $('#fm').form('clear');
    url = '/Shop/NewShopData';
}
function editShop() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $('#dlg').dialog('open').dialog('center').dialog('setTitle', '編輯店家資料');
        $('#fm').form('load', row);
        url = '/Shop/EditShopData';
    }
}
function saveShop() {
    $('#fm').form('submit', {
        url: url,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (result) {
            var result = eval('(' + result + ')');
            if (result.errorMsg) {
                $.messager.show({
                    title: 'Error',
                    msg: result.errorMsg
                });
            } else {
                $('#dlg').dialog('close');        // close the dialog
                $('#dg').datagrid('reload');    // reload the user data
            }
        }
    });
}
function destroyShop() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirm', '確定要刪除此店家資料?', function (r) {
            if (r) {
                $.post('/Shop/DeleteShopData', { shopID: row.shopID }, function (result) {
                    if (result.success) {
                        $('#dg').datagrid('reload');    // reload the user data
                    } else {
                        $.messager.show({    // show error message
                            title: 'Error',
                            msg: result.errorMsg
                        });
                    }
                }, 'json');
            }
        });
    }
}
function doSearch() {
    $('#dg').datagrid('load', {
        shopName: $('#shopName').val(),
        shopTEL: $('#shopTEL').val()
    });
}

//--------產品餐點相關函式-----------------------------------------



function openProductWindow() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $('#wProduct').window('open').window('resize');
        $('#dgProduct').datagrid('options').autoLoad = true;
        $('#dgProduct').datagrid('reload', { shopID: row.shopID });
    }
    else
        alert('請選擇店家');
}

function closeProductWindow() {
    $('#wProduct').window('close');
}

