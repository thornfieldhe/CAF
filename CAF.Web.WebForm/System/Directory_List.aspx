<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Directory_List.aspx.cs" Inherits="CAF.Web.WebForm.System.Dir_List" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <script type="text/javascript">
        ///Grid中显示Dropdownlist的Text值
        var dropParentId = '<%= dropParentId.ClientID %>';

        function renderParentId(value, metadata, record, rowIndex, colIndex) {
            return X(dropParentId).x_getTextByValue(value);
        }
    </script>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:Panel ID="mainPanel" runat="server" Layout="Fit">
        <toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:DropDownList runat="server" ID="dropDirs" Label="父级目录" Width="210px">
                    </f:DropDownList>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnQuery" runat="server" OnClick="btnExcute_Click">
                    </f:Button>
                    <f:Button ID="btnSubmit" runat="server" OnClick="btnExcute_Click">
                    </f:Button>
                    <f:ToolbarFill ID="ToolbarFill1" runat="server">
                    </f:ToolbarFill>
                    <f:Button ID="btnReset" EnablePostBack="false" runat="server">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </toolbars>
        <items>
            <f:CAFGrid ID="grid" runat="server" AllowCellEditing="true" ClicksToEdit="1" OnPreDataBound="grid_PreDataBound">
                <Columns>
                    <f:RenderField runat="server" ColumnID="Id" DataField="Id" HeaderText="Id" Hidden="true">
                    </f:RenderField>
                    <f:RenderField Width="200px" ColumnID="Name" DataField="Name" FieldType="String"
                        HeaderText="目录名称">
                        <Editor>
                            <f:TextBox ID="txtName" Required="true" runat="server">
                            </f:TextBox>
                        </Editor>
                    </f:RenderField>
                    <f:RenderField ColumnID="ParentId" DataField="ParentId" FieldType="String" HeaderText="父目录"
                        RendererFunction="renderParentId">
                        <Editor>
                            <f:DropDownList ID="dropParentId" Required="true" runat="server" Text="Name">
                            </f:DropDownList>
                        </Editor>
                    </f:RenderField>
                    <f:RenderField Width="100px" ColumnID="Sort" DataField="Sort" FieldType="String"
                        HeaderText="目录排序">
                        <Editor>
                            <f:NumberBox ID="txtDirSort" Required="true" NoDecimal="true" NoNegative="true" MinValue="0"
                                runat="server">
                            </f:NumberBox>
                        </Editor>
                    </f:RenderField>
                    <f:RenderField runat="server" ColumnID="DirLink" DataField="DirLink" HeaderText="目录连接"
                        Width="250px">
                        <Editor>
                            <f:TextBox ID="tbxDirLink" runat="server">
                            </f:TextBox>
                        </Editor>
                    </f:RenderField>
                    <f:RenderCheckField Width="60px" ColumnID="Status" DataField="Status" HeaderText="隐藏" />
                    <f:RenderField Width="100px" ColumnID="Note" DataField="Note" FieldType="String"
                        ExpandUnusedSpace="true" HeaderText="备注" ID="rowNote">
                        <Editor>
                            <f:TextBox ID="txtNote" runat="server">
                            </f:TextBox>
                        </Editor>
                    </f:RenderField>
                    <f:LinkButtonField HeaderText="删除" Width="60px" ColumnID="Delete" Icon="Delete" />
                </Columns>
            </f:CAFGrid>
        </items>
    </f:Panel>
    </form>
</body>
</html>
