var editIndex = undefined;
var url = '';


//----呼叫函式-------------------------------------------


function initialDataGrid()
{
    $('#dgProduct').datagrid({
        title: '',
        iconCls: 'icon-edit',
        width: 660,
        height: 250,
        singleSelect: true,
        idField: 'productID',
        url: '/Product/GetProductData',
        columns: [[
            { field: 'productName', title: '餐點名稱', width: 60, editor: { type: 'text' }, required: true },
            {
                field: 'productTypeID', title: '餐點類型', width: 100,
                formatter: function (value, row) {
                    return row.productTypeName;
                },
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'productTypeID',
                        textField: 'productTypeName',
                        method: 'get',
                        url: '/ProductType/GetProductTypeData',
                        required: true
                    }
                }
            },
            { field: 'productPrice', title: '餐點價格', width: 80, align: 'right', editor: { type: 'numberbox', options: { precision: 0 } } },
            {
                field: 'Action', title: '', width: 70, align: 'center',
                formatter: function (value, row, index) {
                    if (row.editing) {
                        var s = '<a href="#" onclick="saverow(this)">Save</a> ';
                        var c = '<a href="#" onclick="cancelrow(this)">Cancel</a>';
                        return s + c;
                    } else {
                        var e = '<a href="#" onclick="editrow(this)">Edit</a> ';
                        var d = '<a href="#" onclick="deleterow(this)">Delete</a>';
                        return e + d;
                    }
                }
            }
        ]],
        toolbar: '#tbProduct',
        autoLoad: false,
        onBeforLoad: function () {
            var opts = $(this).datagrid('options');
            return opts.autoLoad;
        },
        onBeforeEdit: function (index, row) {
            row.editing = true;
            updateActions(index);
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
            updateActions(index);
        },
        onCancelEdit: function (index, row) {

            row.editing = false;
            $('#dgProduct').datagrid("cancelEdit", index);
            updateActions(index);
            $('#dgProduct').datagrid('reload');
        }
    });
}

function updateActions(index) {
    $('#dgProduct').datagrid('refreshRow', index);
    

}


function insertRow() {
    var srow = $('#dg').datagrid('getSelected');
    var row = $('#dgProduct').datagrid('getSelected');
    var index = 0;
    $('#dgProduct').datagrid('insertRow', {
        index: index,
        row: {
            shopID: srow.shopID,
            isNewRecord: true
        }
    });
    $('#dgProduct').datagrid('selectRow', index);
    $('#dgProduct').datagrid('beginEdit', index);
}
function getRowIndex(target) {
    var tr = $(target).closest('tr.datagrid-row');
    return parseInt(tr.attr('datagrid-row-index'));
}
function editrow(target) {
    $('#dgProduct').datagrid('beginEdit', getRowIndex(target));
}
function deleterow(target) {
    $.messager.confirm('Confirm', '確定要刪除?', function (r) {
        if (r) {
            var index = getRowIndex(target);
            $('#dgProduct').datagrid('endEdit', index);
            var row = $('#dgProduct').datagrid('getRows')[index];
            var ajaxSets = {
                type: 'POST',
                datatype: 'json',
                url: '/Product/DeleteProductData',
                data: row,
                success: function (d, txtStatus, xhr) {
                    $('#dgProduct').datagrid('deleteRow', getRowIndex(target));
                    $('#dgProduct').datagrid('reload');
                },
                error: function (jqXHR, txtStatus, errorThrown) {
                    alert(jqXHR);
                }
            };

            $.ajax(ajaxSets);
            $('#dgProduct').datagrid('clearSelections');

        }
    });
}
function saverow(target) {
    var index = getRowIndex(target);
    $('#dgProduct').datagrid('endEdit', index);
    var row = $('#dgProduct').datagrid('getRows')[index];
    var ajaxSets = {
        type: 'POST',
        datatype: 'json',
        url: row.isNewRecord ? '/Product/NewProductData' : '/Product/EditProductData',
        data: row,
        success: function (d, txtStatus, xhr) {
            $('#dgProduct').datagrid('reload');
        },
        error: function (jqXHR, txtStatus, errorThrown) {
            alert(jqXHR);
        }
    };

    $.ajax(ajaxSets);
    $('#dgProduct').datagrid('clearSelections');
}
function cancelrow(target) {
    $('#dgProduct').datagrid('cancelEdit', getRowIndex(target));
}



