<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Query.aspx.cs" Inherits="CAF.Web.WebForm.User_Query" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户查询</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:CAFPanel runat="server" ID="mainPanel" Layout="Fit" >
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" >
                <Items >
                    <f:DropDownList runat="server" ID="dropDeps" Label="部门" LabelWidth="40px"  MatchFieldWidth="true">
                    </f:DropDownList>
                    <f:DropDownList runat="server" ID="dropRoles" Label="角色" LabelWidth="40px" Width="160px" MatchFieldWidth="true">
                    </f:DropDownList>
                    <f:DropDownList runat="server" ID="dropStatus" Label="状态" LabelWidth="40px" Width="120px" MatchFieldWidth="true">
                    </f:DropDownList>
                    <f:TextBox runat="server" ID="txtName" Label="用户名"  LabelWidth="60px">
                    </f:TextBox>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:QueryButton ID="btnQuery" runat="server" OnClick="btnQuery_Click"/>
                    <f:NewButton ID="btnNew"  Icon="Add" runat="server"  />
                    <f:DeleteButton  ID="btnDeleteRows" runat="server" OnClick="btnDeleteRows_Click"/>
                    <f:Button Text="锁定" ID="btnLockRows" runat="server" OnClick="btnLockRows_Click"/>
                    <f:Button Text="解锁" ID="btnUnLockRows" runat="server" OnClick="btnUnLockRows_Click"/>
                    <f:ToolbarFill ID="ToolbarFill1" runat="server"/>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:CAFGrid ID="grid" runat="server" Title="用户列表" SortField="Name" OnRowCommand="gridRowCommand">
                <Columns>
                    <f:EditWindowField WindowID="winEdit"  DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                    DataIFrameUrlFormatString="User_Edit.aspx?Id={0}" DataWindowTitleField="Name"
                    DataWindowTitleFormatString="编辑 - {0}"  />
                    <f:BoundField Width="80px" ColumnID="LoginName" SortField="LoginName" DataField="LoginName"
                        HeaderText="登陆名"></f:BoundField>
                    <f:BoundField Width="80px" ColumnID="Name" SortField="Name" DataField="Name"
                        HeaderText="用户名"></f:BoundField>
                    <f:BoundField Width="100px" ColumnID="PhoneNum" DataField="PhoneNum" HeaderText="移动电话">
                    </f:BoundField>
                    <f:BoundField Width="160px" ColumnID="OrganizeName" SortField="OrganizeName" DataField="OrganizeName"
                        HeaderText="部门"></f:BoundField>
                    <f:TemplateField ID="TemplateField1" runat="server" ColumnID="StatusName" SortField="StatusName"
                        HeaderText="用户状态">
                        <ItemTemplate>
                            <%# Eval("StatusName").ToString() == "锁定" ? "<span style='color: #FF0000'>锁定</span>" : Eval("StatusName").ToString()%>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:BoundField Width="140px" ColumnID="CreatedDate" SortField="CreatedDate" DataField="CreatedDate"
                        HeaderText="创建时间"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="ChangedDate" SortField="ChangedDate" DataField="ChangedDate"
                        HeaderText="更新时间"></f:BoundField>
                    <f:BoundField Width="200px" ColumnID="Note" DataField="Note" HeaderText="备注"></f:BoundField>
                <f:DeleteLinkButtonField  />
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                </PageItems>
            </f:CAFGrid>
        </Items>
    </f:CAFPanel>
    <f:CAFWindow ID="winEdit" Title="编辑"  runat="server" OnClose="winEdit_Close"  Width="600px" Height="600px"/>
    </form>
</body>
</html>
