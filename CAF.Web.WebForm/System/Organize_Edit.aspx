<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Organize_Edit.aspx.cs" Inherits="CAF.Web.WebForm.Organize_Edit" %>
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
    <f:MainPanel ID="mainPanel" Layout="HBox" runat="server">
        <Toolbars>
            <f:Toolbar ID="toolbar1" runat="server">
                <Items>
                    <f:CloseButton ID="btnClose" runat="server" />
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
            <f:CAFPanel ID="Panel10" BoxFlex="10" runat="server" Margin="10px;10px;10px;10px;">
                <Items>
                    <f:SubmitForm Width="350px" ID="submitForm"  runat="server">
                        <Rows>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="txtName" Label="部门名称" Required="true" runat="server" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:DropDownList runat="server" ID="dropParentId" Label="父级部门">
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:Label ID="lblLevel" Label="部门层级" runat="server">
                                    </f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="txtCode" Label="编码" runat="server">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:NumberBox ID="txtSort" Label="部门排序" Required="true" runat="server" Text="100"
                                        NoNegative="true" NoDecimal="true" ShowRedStar="true">
                                    </f:NumberBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextArea ID="txtNote" Label="备注" runat="server" Height="50">
                                    </f:TextArea>
                                </Items>
                            </f:FormRow>      
                            <f:FormRow>
                                <Items>
                                    <f:HiddenField ID="txtId"  runat="server" >
                                    </f:HiddenField>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:SubmitForm>
                </Items>
            </f:CAFPanel>
            <f:CAFPanel ID="Panel20" BoxFlex="10"  runat="server" AutoScroll="true">
                <Items>
                    <f:CAFTree ID="treeDeps"   runat="server" OnNodeCommand="treeDeps_NodeCommand">
                    </f:CAFTree>
                </Items>
            </f:CAFPanel>
        </Items>
    </f:MainPanel>
    </form>
</body>
</html>
