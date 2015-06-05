<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CAF.Web.Login" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" />
    <f:Window ID="Window1" runat="server" Title="CAF管理平台" IsModal="false" EnableClose="false"
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
                    <f:TextBox ID="txtCaptcha" Label="验证码" Required="true" runat="server" TabIndex="3">
                    </f:TextBox>
                    <f:Panel ID="Panel1" CssStyle="" ShowBorder="false" ShowHeader="false"
                         runat="server">
                        <Items>
                            <f:Image ID="imgCaptcha" CssStyle="float:left;width:100px;" runat="server" ShowEmptyLabel="true">
                            </f:Image>
                            <f:LinkButton CssStyle="float:left;" ID="btnRefresh" Text="看不清？"
                                runat="server" OnClick="btnRefresh_Click">
                            </f:LinkButton>
                        </Items>
                    </f:Panel>
                </Items>
            </f:SimpleForm>
        </Items>
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right" Position="Bottom">
                <Items>
                    <f:Button ID="btnLogin" Text="登录" Type="Submit" ValidateForms="SimpleForm1" ValidateTarget="Top"
                        runat="server" OnClick="btnLogin_Click" TabIndex="4">
                    </f:Button>
                    <f:Button ID="btnReset" Text="重置" Type="Reset" EnablePostBack="false"
                            runat="server">
                        </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
    </f:Window>
    </form>
  
</body>
</html>
