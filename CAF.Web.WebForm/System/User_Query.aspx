<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Query.aspx.cs" Inherits="CAF.Web.WebForm.User_Query" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:Panel runat="server" ID="mainPanel" Layout="Fit">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:DropDownList runat="server" ID="dropDeps" Label="部门" Width="210px">
                    </f:DropDownList>
                    <f:DropDownList runat="server" ID="dropRoles" Label="角色" Width="150px">
                    </f:DropDownList>
                    <f:DropDownList runat="server" ID="dropStatus" Label="状态" Width="80px">
                    </f:DropDownList>
                    <f:TextBox runat="server" ID="txtName" Label="用户名">
                    </f:TextBox>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnNew" runat="server" OnClick="btnExcute_Click" />
                    <f:Button ID="btnQuery" runat="server" OnClick="btnExcute_Click">
                    </f:Button>
                    <f:Button Text="锁定" ID="btnLockRows" runat="server" OnClick="btnLockRows_Click">
                    </f:Button>
                    <f:Button Text="解锁" ID="btnUnLockRows" runat="server" OnClick="btnUnLockRows_Click">
                    </f:Button>
                    <f:Button Text="删除" ID="btnDeleteRows" runat="server" OnClick="btnDeleteRows_Click"
                        ConfirmIcon="Question" ConfirmText="确认删除选定项？">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:CAFGrid ID="grid" runat="server" OnSort="grid_Sort" OnPageIndexChange="grid_PageIndexChange"
                SortColumnIndex="3">
                <Columns>
                    <f:BoundField ID="BoundField1" runat="server" ColumnID="Id" DataField="Id" HeaderText="Id"
                        Hidden="true"></f:BoundField>
                    <f:WindowField ColumnID="winEdit" Width="40px" WindowID="winEdit" HeaderText="编辑"
                        Icon="Pencil" ToolTip="编辑" DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                        DataIFrameUrlFormatString="User_Edit.aspx?Id={0}" DataWindowTitleField="UserName"
                        DataWindowTitleFormatString="编辑 - {0}" />
                    <f:WindowField ColumnID="winEditRoleDirs" Width="100px" WindowID="winEditRoleDirs"
                        HeaderText="配置角色目录" Icon="Pencil" ToolTip="配置角色目录" DataTextFormatString="{0}"
                        DataIFrameUrlFields="Id" DataIFrameUrlFormatString="UserRoleDeps_Manage.aspx?Id={0}"
                        DataWindowTitleField="UserName" DataWindowTitleFormatString="配置角色目录 - {0}" />
                    <f:BoundField Width="80px" ColumnID="LoginName" SortField="LoginName" DataField="LoginName"
                        HeaderText="登陆名"></f:BoundField>
                    <f:BoundField Width="80px" ColumnID="UserName" SortField="UserName" DataField="UserName"
                        HeaderText="用户名"></f:BoundField>
                    <f:BoundField Width="100px" ColumnID="UserTel" DataField="UserTel" HeaderText="移动电话">
                    </f:BoundField>
                    <f:BoundField Width="160px" ColumnID="DepName" SortField="DepName" DataField="DepName"
                        HeaderText="部门"></f:BoundField>
                    <f:TemplateField ID="TemplateField1" runat="server" ColumnID="Status" SortField="Status"
                        HeaderText="用户状态">
                        <ItemTemplate>
                            <%# Eval("Status").ToString() == "锁定" ? "<span style='color: #FF0000'>锁定</span>" : Eval("Status").ToString()%>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:BoundField Width="140px" ColumnID="Created" SortField="Created" DataField="Created"
                        HeaderText="创建时间"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Updated" SortField="Updated" DataField="Updated"
                        HeaderText="更新时间"></f:BoundField>
                    <f:BoundField Width="200px" ColumnID="Note" DataField="Note" HeaderText="备注"></f:BoundField>
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                </PageItems>
            </f:CAFGrid>
        </Items>
    </f:Panel>
    <f:Window ID="winEdit" Title="编辑" runat="server" Width="600px" Height="600px" OnClose="winEdit_Close">
    </f:Window>
    <f:Window ID="winEditRoleDirs" Title="角色目录" runat="server" Width="600px" Height="600px">
    </f:Window>
    </form>
</body>
</html>
