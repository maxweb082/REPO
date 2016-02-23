var url;
function newDept() {
    $('#dlg').dialog('open').dialog('center').dialog('setTitle', '新增部門資料');
    $('#fm').form('clear');
    url = '/Dept/NewDeptData';
}
function editDept() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $('#dlg').dialog('open').dialog('center').dialog('setTitle', '編輯部門資料');
        $('#fm').form('load', row);
        url = '/Dept/EditDeptData';
    }
}
function saveDept() {
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
function destroyDept() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirm', '確定要刪除此部門資料?', function (r) {
            if (r) {
                $.post('/Dept/DeleteDeptData', { deptID: row.deptID }, function (result) {
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
function deptSearch() {
    $('#dg').datagrid('load', {
        deptAbbreviate: $('#deptAbbreviate').val(),
        deptName: $('#deptName').val()
    });
}

function deptSearchExport() {
    var deptAbbreviate = $('#deptAbbreviate').val();
    var deptName = $('#deptName').val();
    $('#dg').datagrid('load', {
        deptAbbreviate: deptAbbreviate,
        deptName: deptName
    });
    window.open("/Dept/GetExportData?deptAbbreviate=" + deptAbbreviate + "&deptName=" + deptName);
}
//--------部門員工相關函式-----------------------------------------



function openLoginUserNameWindow() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $('#wUser').window('open').window('resize');
        $('#dgUser').datagrid('options').autoLoad = true;
        $('#dgUser').datagrid('reload', { deptID: row.deptID });
    }
    else
        alert('請選取部門');
}

function closeProductWindow() {
    $('#wUser').window('close');
}


//------------------------------------------------------------------------
