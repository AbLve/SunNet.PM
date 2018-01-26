<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestUI.aspx.cs" Inherits="SunNet.PMNew.Web.TestUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/datagrid/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="Styles/datagrid/icon.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.8.0-vsdoc.js" type="text/javascript"></script>

    <script src="Scripts/jquery-1.8.0.js" type="text/javascript"></script>

    <script src="Scripts/jquery.easyui.min.js" type="text/javascript"></script>

</head>
<body>
    <h1>
        DataGrid</h1>
    <div style="margin-bottom: 10px;">
        <a href="#" onclick="resize()">resize</a> <a href="#" onclick="getSelected()">getSelected</a>
        <a href="#" onclick="getSelections()">getSelections</a> <a href="#" onclick="clearSelections()">
            clearSelections</a> <a href="#" onclick="selectRow()">selectRow</a> <a href="#" onclick="selectRecord()">
                selectRecord</a> <a href="#" onclick="unselectRow()">unselectRow</a></div>
    <table id="test">
    </table>
    
    <input type="checkbox" />

    <script type="text/javascript">
        $(function() {
            $('#test').datagrid({
                title: 'My Title',
                iconCls: 'icon-save',
                width: 800,
                height: 450,
                nowrap: true,
                striped: true,
                url: 'datagrid_data.json',
                sortName: 'code',
                sortOrder: 'desc',
                idField: 'code',
                frozenColumns: [[
                { field: 'ck', checkbox: true },
                { title: 'code', field: 'code', width: 80, sortable: true }
                ]],
                columns: [
                    [
                    { title: 'Base Information', colspan: 3 },
                    { field: 'opt', title: 'Operation', width: 100, align: 'center', rowspan: 2,
                        formatter: function(value, rec) {
                            return '<span style="color:red">Edit Delete</span>';
                        }
                    }
                    ],
                    [
                    { field: 'name', title: 'Name', width: 120 },
                    { field: 'addr', title: 'Address', width: 120, rowspan: 2, sortable: true },
                    { field: 'col4', title: 'Col41', width: 150, rowspan: 2 }
                    ]
                 ],
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                toolbar: [
                            { text: 'Add', iconCls: 'icon-add',
                                handler: function() {
                                    alert('add');
                                }
                            },
                        { text: 'Cut', iconCls: 'icon-cut', disabled: true,
                            handler: function() { alert('cut') }
                        },
                         '-',
                          { text: 'Save', iconCls: 'icon-save',
                              handler: function() { alert('save') }
                          }
                    ]
            });
            var p = $('#test').datagrid('getPager');
            if (p) {
                $(p).pagination({ onBeforeRefresh: function() { alert('before refresh'); } });
            }
        });

        function resize() {
            $('#test').datagrid({ title: 'New Title', striped: true, width: 650,
                queryParams: { p: 'param test', name: 'My Name' }
            });
        }

        function getSelected() {
            var selected = $('#test').datagrid('getSelected'); alert(selected.code + ":" + selected.name);
        }

        function getSelections() {
            var ids = []; var rows = $('#test').datagrid('getSelections');
            for (var i = 0; i < rows.length; i++) { ids.push(rows[i].code); } alert(ids.join(':'))
        }

        function clearSelections() {
            $('#test').datagrid('clearSelections');
        }

        function selectRow() {
            $('#test').datagrid('selectRow', 2);
        }

        function selectRecord() {
            $('#test').datagrid('selectRecord', '002');
        }

        function unselectRow() {
            $('#test').datagrid('unselectRow', 2);
        }
    </script>

</body>
</html>
