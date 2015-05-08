<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_ChangePass.aspx.cs" Inherits="CAF.Web.WebForm.System.User_ChangePass" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body >
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server"  />
    <f:CAFPanel ID="mainPanel" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:CloseButton ID="btnClose" runat="server" />
                    <f:UpdateButton ID="btnUpdate" runat="server" OnClick="btnExcute_Click">
                    </f:UpdateButton>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <items>
            <f:SubmitForm Width="350px" ID="submitForm" runat="server" >
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:Label ID="lblLoginName" Label="登录名" runat="server" >
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtOldPassword" Label="原密码" TextMode="Password" runat="server" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtNewPass" Label="新密码" runat="server" Required="true" ShowRedStar="true" TextMode="Password">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtConfirmPassword" Label="确认密码" runat="server"  ShowRedStar="true" TextMode="Password" CompareControl="txtNewPass"
                CompareOperator="Equal" CompareMessage="密码两次输入不一致！">
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
