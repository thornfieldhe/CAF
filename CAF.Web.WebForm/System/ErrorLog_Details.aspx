<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorLog_Details.aspx.cs" Inherits="CAF.Web.WebForm.ErrorLog_Details" %>
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
    <f:PageManager  runat="server"  ID="manager"  AutoSizePanelID="mainPanel"/>
    <f:CAFPanel ID="mainPanel" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:CloseButton ID="btnClose" runat="server" />
                </Items>
            </f:Toolbar>
        </Toolbars>
        <items>
            <f:SubmitForm Width="580px" ID="submitForm" runat="server" >
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:Label ID="lblUserName" Label="用户名" runat="server" >
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Label ID="lblPageCode" Label="错误代码" runat="server" >
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Label ID="lblPage" Label="错误页" runat="server" >
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Label ID="lblIp" Label="Ip" runat="server">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea ID="txtDetails" Label="详细错误" runat="server" Width="570px" Height="300"
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

