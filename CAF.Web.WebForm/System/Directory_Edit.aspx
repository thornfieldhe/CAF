<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Directory_Edit.aspx.cs" Inherits="CAF.Web.WebForm.Directory_Edit" %>
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
    <f:PageManager ID="manager" runat="server" AutoSizePanelID="mainPanel" />
    <f:MainPanel ID="mainPanel" Layout="HBox" runat="server">
        <toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:AddButton ID="btnAdd" runat="server" OnClick="btnExcute_Click">
                    </f:AddButton>
                    <f:UpdateButton ID="btnUpdate" runat="server" OnClick="btnExcute_Click">
                    </f:UpdateButton>
                    <f:DeleteButton ID="btnDelete" runat="server" OnClick="btnExcute_Click">
                    </f:DeleteButton>
                </Items>
            </f:Toolbar>
        </toolbars>
        <items>
            <f:CAFPanel ID="Panel11" BoxFlex="10" runat="server" Margin="10px;10px;10px;10px;">
                <Items>
                    <f:SubmitForm Width="350px" ID="submitForm" runat="server" >
                        <Rows>
                                                        <f:FormRow>
                                <Items>
                                    <f:TextBox ID="txtId" Label="编码(GUID)" Required="true" runat="server" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="txtName" Label="目录名称" Required="true" runat="server" ShowRedStar="true">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:DropDownList runat="server" ID="dropParentId" Label="父级目录">
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ColumnWidths="400px">
                                <Items>
                                    <f:TextBox ID="txtUrl" Label="目录地址" runat="server">
                                    </f:TextBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:NumberBox ID="txtSort" Label="目录排序" Required="true" runat="server" Text="100"
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
                                    <f:CheckBox ID="chkStatus" runat="server" Text="隐藏" Label="状态">
                                    </f:CheckBox>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:SubmitForm>
                </Items>
            </f:CAFPanel>
            <f:CAFPanel ID="Panel20" BoxFlex="10"  runat="server" AutoScroll="true">
                <Items>
                    <f:CAFTree ID="treeDirs"  runat="server" OnNodeCommand="treeDirs_NodeCommand">
                    </f:CAFTree>
                </Items>
            </f:CAFPanel>
        </items>
    </f:MainPanel>
    </form>
</body>
</html>
