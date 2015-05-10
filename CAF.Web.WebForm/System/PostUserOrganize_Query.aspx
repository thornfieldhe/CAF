<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostUserOrganize_Query.aspx.cs" Inherits="CAF.Web.WebForm.PostUserOrganize_Query" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server"   AutoSizePanelID="mainPanel"/>
    <f:CAFPanel runat="server" ID="mainPanel" Layout="Fit">
        <Items>
            <f:CAFGrid ID="grid" runat="server"  AllowSorting="False" SortField="PostName" OnRowCommand="gridRowCommand" 
                 Title="岗位用户部门查询">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server" >
                        <Items >
                            <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </f:ToolbarSeparator>
                            <f:NewButton ID="btnNew"  Icon="Add" runat="server"  />
                            <f:DeleteButton  ID="btnDeleteRows" runat="server" OnClick="btnExcute_Click"/>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                            </f:ToolbarFill>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Columns>
                    <f:EditWindowField WindowID="winEdit"  DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                    DataIFrameUrlFormatString="PostUserOrganize_Edit.aspx?Id={0}" DataWindowTitleField="Name"
                    DataWindowTitleFormatString="编辑 - {0}"  />
                    <f:BoundField Width="140px" ColumnID="UserName" DataField="UserName" HeaderText="用户" ></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="PostName" DataField="PostName" HeaderText="岗位" ></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="OrganizeName" DataField="OrganizeName" HeaderText="部门" ></f:BoundField>
                    <f:DeleteLinkButtonField  />
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                </PageItems>
            </f:CAFGrid>
        </Items>
    </f:CAFPanel>
    <f:CAFWindow ID="winEdit" Title="编辑"  runat="server"  Width="400px" Height="200px"/>
    </form>
</body>
</html>

