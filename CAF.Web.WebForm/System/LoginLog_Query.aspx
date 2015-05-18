<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginLog_Query.aspx.cs" Inherits="CAF.Web.WebForm.LoginLog_Query" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager  runat="server" ID="manager"  AutoSizePanelID="mainPanel"/>
    <f:CAFPanel runat="server" ID="mainPanel" Layout="Fit" >
        <Items>
            <f:CAFGrid ID="grid" runat="server" Title="登陆日志查询"  SortField="CreatedDate" 
                 DataKeyNames="Id" SortDirection="DESC">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server" >
                        <Items >
                            <f:TextBox runat="server" ID="txtName"  EmptyText="用户名">
                            </f:TextBox>
                            <f:DatePicker runat="server" ID="dateFrom" EmptyText="期日起"/>
                            <f:DatePicker runat="server" ID="dateTo" EmptyText="期日止"/>
                            <f:QueryButton ID="btnQuery"   runat="server"  OnClick="btnExcute_Click"/>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server"/>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Columns>
                    <f:BoundField Width="140px" ColumnID="UserName" DataField="UserName" HeaderText="用户名" SortField="UserName"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Ip" DataField="Ip" HeaderText="Ip" SortField="Ip"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="StatusName" DataField="StatusName" HeaderText="登陆动作" ></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="CreatedDate" DataField="CreatedDate" HeaderText="操作时间"  SortField="CreatedDate"></f:BoundField>
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                </PageItems>
            </f:CAFGrid>
        </Items>
    </f:CAFPanel>
    <f:CAFWindow ID="winEdit" Title="编辑"  runat="server"  Width="400px" Height="300px"/>
    </form>
</body>
</html>

