<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcuteSql.aspx.cs" Inherits="CAF.Web.WebForm.System.ExcuteSql" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
 <f:PageManager ID="manager" runat="server"  />
    <f:CAFPanel ID="mainPanel" runat="server" >
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnSave" runat="server" OnClick="btnExcute_Click" Text="执行">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:SubmitForm Width="800px" ID="submitForm" LabelWidth="100px" runat="server">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextArea ID="txtSqlString" Label="执行语句" runat="server" >
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:CheckBox ID="chkIsExcute" runat="server" Text="获取结果" Label="获取结果">
                            </f:CheckBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Label ID="lblResult" runat="server" Text="结果" Label="结果">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:SubmitForm>
        </Items>
    </f:CAFPanel>
    </form>
</body>
</html>
