<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoLog_Query.aspx.cs" Inherits="CAF.Web.WebForm.InfoLog_Query" %>
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
            <f:CAFGrid ID="grid" runat="server" Title="操作日志"  SortField="CreatedDate" 
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
                    <f:BoundField Width="240px" ColumnID="Page" DataField="Page" HeaderText="操作页"  SortField="Page"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Action" DataField="Action" HeaderText="操作"  SortField="Action"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="CreatedDate" DataField="CreatedDate" HeaderText="操作时间"  SortField="CreatedDate"></f:BoundField>

                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                </PageItems>
            </f:CAFGrid>
        </Items>
    </f:CAFPanel>
    </form>
</body>
</html>

