<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_Edit.aspx.cs" Inherits="CAF.Web.WebForm.Role_Edit" %>
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
    <f:MainPanel ID="mainPanel" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:AddButton ID="btnAdd" runat="server" OnClick="btnAdd_Click">
                    </f:AddButton>
                    <f:UpdateButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click">
                    </f:UpdateButton>
                    <f:DeleteButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">
                    </f:DeleteButton>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:SubmitForm Width="350px" ID="submitForm" LabelWidth="100px" runat="server">
                <Rows>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList runat="server" ID="dropRoles" Label="角色列表" OnSelectedIndexChanged="dropRolesId_SelectedIndexChanged"
                                AutoPostBack="true">
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="txtName" Label="角色名称" Required="true" runat="server" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea ID="txtNote" Label="备注" runat="server" Height="50">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:SubmitForm>
        </Items>
    </f:MainPanel>
    </form>
</body>
</html>
