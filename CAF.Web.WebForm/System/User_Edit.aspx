<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Edit.aspx.cs" Inherits="CAF.Web.WebForm.User_Edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:Panel ID="mainPanel" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" runat="server" />
                    <f:Button runat="server" ID="btnAdd" OnClick="btnExcute_Click">
                    </f:Button>
                    <f:Button ID="btnUpdate" runat="server" Visible="false" OnClick="btnExcute_Click" />
                    <f:Button ID="btnDelete" runat="server" Visible="false" OnClick="btnExcute_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form Width="500px" ID="submitForm" runat="server">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtLoginName" Label="登陆名" Required="true" runat="server" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtName" Label="用户名" Required="true" runat="server" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtUserPassword" Label="密码" runat="server" ShowRedStar="true" TextMode="Password"
                                MinLength="8">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtConfirmPass" Label="确认密码" runat="server" ShowRedStar="true" TextMode="Password"
                                CompareOperator="Equal" CompareMessage="密码不一致！" CompareControl="txtUserPassword"
                                MinLength="8">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:NumberBox ID="txtUserTel" Label="手机" runat="server">
                            </f:NumberBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList ID="dropDepId" Label="部门" runat="server" ShowRedStar="true">
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea ID="txtNote" Label="备注" runat="server" Height="50px">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:RadioButtonList ID="radioStatus" runat="server" Label="状态">
                            </f:RadioButtonList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:HiddenField ID="txtId" runat="server" />
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:GroupPanel runat="server" AutoHeight="true" Title="用户角色" ID="GroupPanel1" EnableCollapse="True"
                                Width="470px" EnableAjax="false">
                                <Items>
                                    <f:CheckBoxList runat="server" ID="chkUserRoles" ColumnNumber="2">
                                    </f:CheckBoxList>
                                </Items>
                            </f:GroupPanel>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
