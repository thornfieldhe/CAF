<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CAF.Web.Dashboard" %>
<%@ Register TagPrefix="f" Namespace="CAF.Web.WebForm.CAFControl" Assembly="CAF.Web.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>CAF管理平台</title>
    <link href="Content/default.css" rel="stylesheet" />

    <style>
         #header {
            position: relative;
            border-bottom-width: 2px;
            border-bottom-style: solid;
            padding: 8px 12px 6px;
        }

            #header a.logo {
                display: inline-block;
                margin-right: 5px;
            }

            #header a.title {
                font-weight: bold;
                font-size: 24px;
                text-decoration: none;
                line-height: 30px;
                color: #fff;
            }


            #header .themeroller {
                position: absolute;
                top: 10px;
                right: 10px;
            }

                #header .themeroller a {
                    font-size: 20px;
                    text-decoration: none;
                    line-height: 30px;
                    color: #fff;
                }



        .f-theme-neptune #header {
            background-color: #005999;
            border-bottom: 1px solid #1E95EC;
        }

            .f-theme-neptune #header .title a {
                color: #fff;
            }

        .f-theme-blue #header {
            background-color: #004BA8;
            border-bottom: 1px solid #034699;
        }

            .f-theme-blue #header .title a {
                color: #fff;
            }

        .f-theme-gray #header {
            background-color: #d3d3d3;
            border-bottom: 1px solid #bab9b9;
        }

            .f-theme-gray #header .title a {
                color: #333;
            }

        .f-theme-access #header {
            background-color: #343b48;
            border-bottom: 1px solid #1f232b;
        }

            .f-theme-access #header .title a {
                color: #fff;
            }

        .f-theme-access .maincontent .x-panel-body {
            background-image: none;
        }

        .isnew {
            color: red;
        }

        .bottomtable {
            width: 100%;
            font-size: 10px;
        }

         .mytable td.x-table-layout-cell {
            padding: 5px;
        }
 
        .mytable td.f-layout-table-cell {
            padding: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="manager" AutoSizePanelID="mainPanel" runat="server">
    </f:PageManager>
    <f:RegionPanel ID="mainPanel" ShowBorder="false" runat="server">
        <regions>
            <f:Region ID="Region1"  ShowBorder="false" Height="50px" ShowHeader="false"
                Position="Top" Layout="Fit" runat="server">
                <Items>
                    <f:ContentPanel ShowBorder="false" CssClass="jumbotron" ShowHeader="false" ID="ContentPanel5"
                        runat="server">
                        <div class="title">
                            <h1><a class="title" href="default.aspx">CAF管理平台</a>
                                </h1>
                        </div>
                    </f:ContentPanel>
                </Items>
            </f:Region>
            <f:Region ID="Region2" Split="true" Width="200px"  ShowHeader="false"
                 Icon="Outline" EnableCollapse="true" Layout="Fit" Position="Left"
                runat="server">
            </f:Region>
            <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit"  Position="Center"
                runat="server">
                <Toolbars>
                    <f:Toolbar runat="server" ID="toolbar3">
                        <Items>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                            </f:ToolbarFill>
                            <f:Button runat="server" ID="btnLoginOut" OnClick="btnLogOut_Click" Text="注销" Icon="Key"
                                ConfirmIcon="Question" ConfirmText="确认注销？">
                            </f:Button>
                            <f:Button runat="server" ID="btnChangePass" Text="修改密码">
                            </f:Button>
                            <f:Label ID="lblUserName" runat="server">
                            </f:Label>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                        <Tabs>
                            <f:Tab ID="Tab1" Title="首页" Layout="Fit" Icon="House" runat="server">
                                <Items>
                                    <f:Panel ID="PanelHome" runat="server" Width="850px" Height="350px" AutoScroll="true" ShowBorder="True" EnableCollapse="true"
                                        BodyPadding="5px" Layout="Column" ShowHeader="True" Title="欢迎使用CAF后台管理系统！">
                                        <Items>
                                            <f:Panel ID="Panel1" ColumnWidth="33%" runat="server"
                                                ShowBorder="false" ShowHeader="false" Margin="0 10px 0 0">
                                                <Items>
                                                    <f:Panel ID="Panel11" runat="server" Height="200px" Title="待办事项"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                    <f:Panel ID="Panel12" runat="server" Title="消息" Height="200px"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                    <f:Panel ID="Panel13" runat="server"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                </Items>
                                            </f:Panel>
                                            <f:Panel ID="Panel2" ColumnWidth="33%" runat="server" 
                                                ShowBorder="false" ShowHeader="false" Margin="0 10px 0 0">
                                                <Items>
                                                    <f:Panel ID="Panel21" runat="server" Height="200px" Title="已办事项"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                            
                                                    </f:Panel>
                                                    <f:Panel ID="Panel22" runat="server"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                    <f:Panel ID="Panel23" runat="server"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                </Items>
                                            </f:Panel>
                                            <f:Panel ID="Panel3" ColumnWidth="34%" runat="server"
                                                ShowBorder="false" ShowHeader="false">
                                                <Items>
                                                    <f:Panel ID="Panel31" runat="server" Height="200px" Title="已发事项"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                    <f:Panel ID="Panel32" runat="server"  Title="公告新闻" Height="200px"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                    <f:Panel ID="Panel33" runat="server"
                                                        BodyPadding="10px" ShowBorder="true" ShowHeader="true" EnableCollapse="true" Margin="0 0 5px 0">
                                                    </f:Panel>
                                                </Items>
                                            </f:Panel>
                                        </Items>
                                    </f:Panel>
                                </Items>
                            </f:Tab>
                        </Tabs>
                    </f:TabStrip>
                </Items>
            </f:Region>
        </regions>
    </f:RegionPanel>
    <f:CAFWindow ID="winChangePass" Title="修改用户信息" runat="server" Width="400px" Height="200px">
    </f:CAFWindow>
    </form>
</body>
<script src="Scripts/jquery-2.1.1.min.js"></script>
            <script src="Scripts/default.js"></script>
</html>
