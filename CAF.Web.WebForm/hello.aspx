<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="hello.aspx.cs" Inherits="EmptyProjectNet20.hello" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:AddButton  runat="server" ID="btnHello" OnClick="btnHello_Click">
        </f:AddButton>
    </form>
</body>
</html>
