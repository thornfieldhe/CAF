<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowProcess_Edit.aspx.cs" Inherits="CAF.Web.WebForm.WorkflowProcess_Edit" %>
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
                    <f:UpdateButton ID="btnUpdate" runat="server" OnClick="btnExcute_Click">
                    </f:UpdateButton>
                    <f:DeleteButton ID="btnDelete" runat="server" OnClick="btnExcute_Click">
                    </f:DeleteButton>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <items>
            <f:SubmitForm Width="580px" ID="submitForm" runat="server" >
                <Rows>
                     <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtId" Label="Id" runat="server" Readonly="True">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtName" Label="模块名称" runat="server" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea ID="txtDocument" Label="序列化对象" runat="server" Width="570px" Height="300"
                                 Readonly="True">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:SubmitForm>
        </items>
    </f:CAFPanel>
    </form>
</body>
</html>

