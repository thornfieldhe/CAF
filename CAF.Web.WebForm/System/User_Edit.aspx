<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Edit.aspx.cs" Inherits="CAF.Web.WebForm.User_Edit" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:CAFPanel ID="mainPanel" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:CloseButton ID="btnClose" runat="server" />
                    <f:AddButton runat="server" ID="btnAdd" OnClick="btnAdd_Click"/>
                    <f:UpdateButton ID="btnUpdate" runat="server"  OnClick="btnUpdate_Click" />
                    <f:DeleteButton ID="btnDelete" runat="server"  OnClick="btnDelete_Click"/>
                    
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:WindowForm Width="550px" ID="submitForm" runat="server">
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
                            <f:TextBox ID="txtPass" Label="密码" runat="server" ShowRedStar="true" TextMode="Password"
                                MinLength="8">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtConfirmPass" Label="确认密码" runat="server" ShowRedStar="true" TextMode="Password"
                                CompareOperator="Equal" CompareMessage="密码不一致！" CompareControl="txtPass"
                                MinLength="8">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:NumberBox ID="txtPhoneNum" Label="手机" runat="server">
                            </f:NumberBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList ID="dropOrganizeId" Label="部门" runat="server" ShowRedStar="true">
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                             <f:TextBox ID="Email" Label="电子邮件" Required="true" runat="server" ShowRedStar="true">
                            </f:TextBox>
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
                            <f:GroupPanel runat="server"  Title="用户角色" ID="GroupPanel1" EnableCollapse="True"
                                Width="540px" EnableAjax="false">
                                <Items>
                                    <f:CheckBoxList runat="server" ID="chkUserRoles" ColumnNumber="2">
                                    </f:CheckBoxList>
                                </Items>
                            </f:GroupPanel>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:WindowForm>
        </Items>
    </f:CAFPanel>
    </form>
</body>
</html>
