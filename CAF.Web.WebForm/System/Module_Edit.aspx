<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Module_Edit.aspx.cs" Inherits="CAF.Web.WebForm.Module_Edit" %>
<%@ Register TagPrefix="f" Namespace="FineUI" Assembly="FineUI" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body >
    <form id="form1" runat="server">
    <f:PageManager  runat="server" ID="manager"  AutoSizePanelID="mainPanel"/>
    <f:CAFPanel ID="mainPanel" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:CloseButton ID="btnClose" runat="server" />
                    <f:AddButton ID="btnAdd" runat="server" OnClick="btnExcute_Click">
                    </f:AddButton>
                    <f:UpdateButton ID="btnUpdate" runat="server" OnClick="btnExcute_Click">
                    </f:UpdateButton>
                    <f:DeleteButton ID="btnDelete" runat="server" OnClick="btnExcute_Click">
                    </f:DeleteButton>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <items>
            <f:SubmitForm Width="350px" ID="submitForm" runat="server" >
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtName" Label="模块名" runat="server" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtKey" Label="模块标识" runat="server" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:SubmitForm>
        </items>
    </f:CAFPanel>
    </form>
</body>
</html>

