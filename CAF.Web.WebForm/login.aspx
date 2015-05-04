<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs"
    Inherits="EmptyProjectNet20.login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" />
    <f:Window ID="Window1" runat="server" Title="欢迎登陆诗味特统一管理平台V2.0" IsModal="false" EnableClose="false"
        WindowPosition="GoldenSection" Width="350px">
        <Items>
            <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px"
                LabelWidth="80px"  ShowHeader="false">
                <Items>
                    <f:TextBox ID="txtUserName" Label="用户名" Required="true" runat="server" TabIndex="1"
                        Text="00001">
                    </f:TextBox>
                    <f:TextBox ID="txtPassword" Label="密码" TextMode="Password" Required="true" runat="server"
                        Text="11111111" TabIndex="2">
                    </f:TextBox>
                    <f:TextBox ID="txtCaptcha" Label="验证码" Required="true" runat="server" TabIndex="3"
                        Text="1">
                    </f:TextBox>
                    <f:Panel ID="Panel1" CssStyle="" ShowBorder="false" ShowHeader="false"
                         runat="server">
                        <Items>
                            <f:Image ID="imgCaptcha" CssStyle="float:left;width:100px;" runat="server" ShowEmptyLabel="true">
                            </f:Image>
                            <f:LinkButton CssStyle="float:left;padding-top:8px;" ID="btnRefresh" Text="看不清？"
                                runat="server" OnClick="btnRefresh_Click">
                            </f:LinkButton>
                        </Items>
                    </f:Panel>
                </Items>
            </f:SimpleForm>
        </Items>
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" >
                <Items>
                    <f:Button ID="btnLogin" Text="登录" Type="Submit" ValidateForms="SimpleForm1" ValidateTarget="Top"
                        runat="server" OnClick="btnLogin_Click" TabIndex="4">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
    </f:Window>
    </form>
</body>
</html>
