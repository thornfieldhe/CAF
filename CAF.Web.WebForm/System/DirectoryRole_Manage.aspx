<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DirectoryRole_Manage.aspx.cs" Inherits="CAF.Web.WebForm.System.DirectoryRole_Manage" %>
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
        var dropRoles = '<%= dropRoles.ClientID %>';
        var dropStatus = '<%= dropStatus.ClientID %>';

        function renderDicKey(value) {
            return F(dropRoles).f_getTextByValue(value);
        }
        function renderStatus(value) {
            return F(dropStatus).f_getTextByValue(value);
        }
    </script>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:CAFPanel ID="mainPanel" runat="server" Layout="Fit" >
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:DropDownList runat="server" ID="dropDirs" Label="目录" Width="300px">
                    </f:DropDownList>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:QueryButton ID="btnQuery" runat="server" OnClick="btnQuery_Click">
                    </f:QueryButton>
                    <f:AddButton ID="btnAdd" runat="server" EnablePostBack="false">
                    </f:AddButton>
                    <f:SubmitButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click">
                    </f:SubmitButton>
                    <f:ToolbarFill ID="ToolbarFill1" runat="server">
                    </f:ToolbarFill>
                    <f:Button ID="btnReset" EnablePostBack="false" runat="server">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:CAFGrid ID="grid" runat="server" OnPreDataBound="grid_PreDataBound" SortField="Status"  
                AllowCellEditing="true" ClicksToEdit="1" Title="目录角色批量编辑">
                <Columns>
                    <f:RenderField Width="200px" ColumnID="RoleId" DataField="RoleId" FieldType="String" ExpandUnusedSpace="true"
                        HeaderText="角色"  RendererFunction="renderDicKey">
                        <Editor>
                            <f:DropDownList ID="dropRoles" Required="true" runat="server">
                            </f:DropDownList>
                        </Editor>
                    </f:RenderField>
                    <f:RenderField ColumnID="Status" DataField="Status" FieldType="Int"  RendererFunction="renderStatus" 
                        HeaderText="权限" ExpandUnusedSpace="true">
                        <Editor>
                            <f:DropDownList ID="dropStatus" Required="true" runat="server">
                            </f:DropDownList>
                        </Editor>
                    </f:RenderField>
                    <f:DeleteLinkButtonField  EnablePostBack="false"/>
                </Columns>
            </f:CAFGrid>
        </Items>
    </f:CAFPanel>
    </form>
</body>
</html>
