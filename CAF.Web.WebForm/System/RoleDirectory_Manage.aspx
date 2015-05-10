<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleDirectory_Manage.aspx.cs" Inherits="CAF.Web.WebForm.RoleDirectory_Manage" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server"  AutoSizePanelID="mainPanel"/>
    <f:Panel ID="mainPanel" Layout="HBox" runat="server" Title="角色目录配置">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:UpdateButton ID="btnUpdate" runat="server" OnClick="btnExcute_Click">
                    </f:UpdateButton>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Panel ID="Panel11" BoxFlex="1" ShowBorder="false" 
                ShowHeader="false" runat="server" Width="350px">
                <Items>
                    <f:SubmitForm Width="350px" ID="submitForm" runat="server">
                        <Rows>
                            <f:FormRow>
                                <Items>
                                    <f:DropDownList runat="server" ID="dropRoles" Label="角色列表" OnSelectedIndexChanged="dropRolesId_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:RadioButtonList ID="radioStatus" Label="操作权限" runat="server">
                                    </f:RadioButtonList>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:SubmitForm>
                </Items>
            </f:Panel>
            <f:Panel ID="Panel20" BoxFlex="1"  ShowBorder="false" 
                ShowHeader="false" runat="server" AutoScroll="true">
                <Items>
                    <f:CAFTree ID="treeDirs"  runat="server" 
                        Icon="None" OnNodeCheck="treeDirs_NodeCheck">
                    </f:CAFTree>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
