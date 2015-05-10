<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomQuery.aspx.cs" Inherits="CAF.Web.WebForm.System.CustomQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <table>
        <tr><td ><asp:Button ID="btnSqlQuery" runat="server" OnClick="btnQuery_Click" Text="执行查询"/></td>
            <td><asp:TextBox ID="txtSql" runat="server" Width="600px" TextMode="MultiLine" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:GridView ID="grid" runat="server" Title="查询结果" /></td>
        </tr>
        <tr><td colspan="2"><asp:Label runat="server" ID="lblError"></asp:Label></td></tr>
    </table>
    </form>
</body>
</html>
