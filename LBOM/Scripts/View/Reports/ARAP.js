



function queryReport() {
    var salesID = $('#txtSalesID').val();



    var settings = {
        url: '/Reports/GetArapReport',
        data: { salesID: salesID },
        type: 'POST',
        success: function (data) {
            $('#dvFastReport').html(data);
        },
        error: function (xhr, status, error) {
            $.messager.alert('Warning', error);
        }
    };

    $.ajax(settings);
}

function EexcelExport() {

    var settings = {
        url: '/Reports/CheckExportData',
        type: 'GET',
        success: function (result) {
            if (!result.canExport)
            { $.messager.prompt('Server回應', '無資料可匯出', 'icon-tip'); }

            $.messager.confirm('請確認', '總計有' + result.total + '筆資料，真的要匯出嗎？', function (r) {

                if (!r) return;

                window.open('/Reports/GetArapRarExcel');
            });

            },
        error: function (xhr, status, error) {
            $.messager.alert('Warning', error);
        }
    };

    $.ajax(settings);

}