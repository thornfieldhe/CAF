﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowProcess_Query.aspx.cs" Inherits="CAF.Web.WebForm.WorkflowProcess_Query" %>
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
            <f:CAFGrid ID="grid" runat="server" Title="模块查询"  SortField="CreatedDate" OnRowCommand="gridRowCommand" 
                 DataKeyNames="Id">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server" >
                        <Items >
                            <f:DeleteButton  ID="btnDelete" runat="server" OnClick="btnExcute_Click"/>
                            <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </f:ToolbarSeparator>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server"/>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Columns>
                    <f:EditWindowField WindowID="winEdit"  DataTextFormatString="{0}" DataIFrameUrlFields="Id"
                    DataIFrameUrlFormatString="WorkflowProcess_Edit.aspx?Id={0}" DataWindowTitleField="Name"
                    DataWindowTitleFormatString="编辑 - {0}"  />
                    <f:BoundField Width="140px" ColumnID="Name" DataField="Name" HeaderText="名称" SortField="Name"></f:BoundField>
                    <f:BoundField Width="140px" ColumnID="Document" DataField="Document" HeaderText="序列化工作流对象" SortField="Document"></f:BoundField>
                    <f:DeleteLinkButtonField  />
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                </PageItems>
            </f:CAFGrid>
        </Items>
    </f:CAFPanel>
    <f:CAFWindow ID="winEdit" Title="编辑"  runat="server"  Width="600px" Height="500px"/>
    </form>
</body>
</html>

