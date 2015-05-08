<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Organize_Query.aspx.cs" Inherits="CAF.Web.WebForm.Organize_Query" %>
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
    <f:CAFPanel runat="server" ID="mainPanel" Layout="Fit">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" >
                <Items >
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:NewButton ID="btnNew"  Icon="Add" runat="server"  />
                    <f:DeleteButton  ID="btnDeleteRows" runat="server" OnClick="btnDeleteRows_Click"/>
                    <f:ToolbarFill ID="ToolbarFill1" runat="server"/>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:CAFGrid ID="grid" runat="server" Title="部门查询" AllowSorting="False" SortField="Level" OnRowCommand="gridRowCommand" 
                EnableCollapse="true" DataKeyNames="Id">
                <Columns>
                    <f:EditWindowField WindowID="winEdit"  DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                    DataIFrameUrlFormatString="Organize_Edit.aspx?Id={0}" DataWindowTitleField="Name"
                    DataWindowTitleFormatString="编辑 - {0}"  />
                    <f:BoundField Width="140px" ColumnID="Name" DataField="Name" HeaderText="部门名称" DataSimulateTreeLevelField="SysLevel" ExpandUnusedSpace="True"  DataFormatString="{0}"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Code" DataField="Code" HeaderText="部门编码" ></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Level" DataField="Level" HeaderText="部门层级" ></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Sort" DataField="Sort" HeaderText="排序" ></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="ParentName" DataField="ParentName" HeaderText="父部门名称" ></f:BoundField>
                    <f:DeleteLinkButtonField  />
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

