﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="EmptyProjectNet20._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>FineUI（开源版）空项目</title>
    <style>
        body.f-theme-neptune .header {
            background-color: #005999;
            border-bottom: 1px solid #1E95EC;
        }

            body.f-theme-neptune .header .x-panel-body {
                background-color: transparent;
            }

            body.f-theme-neptune .header .title a {
                font-weight: bold;
                font-size: 24px;
                text-decoration: none;
                line-height: 50px;
                margin-left: 10px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server" >
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" Height="50px" ShowHeader="false"
                    Position="Top" Layout="Fit" runat="server">
                    <Items>
                        <f:ContentPanel ShowBorder="false" ShowHeader="false" ID="ContentPanel1" CssClass="header"
                            runat="server">
                            <div class="title">
                                <a href="./default.aspx" style="color: #fff;">CAF管理系统后台</a>
                            </div>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Split="true" Width="200px" ShowHeader="true" Title="菜单"
                    EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <f:Tree runat="server" ShowBorder="false" ShowHeader="false" EnableArrows="true" EnableLines="true" ID="leftMenuTree">
                            <Nodes>
                                <f:TreeNode Text="默认分类" Expanded="true">
                                    <f:TreeNode Text="开始页面" NavigateUrl="~/hello.aspx"></f:TreeNode>
                                    <f:TreeNode Text="登录页面" NavigateUrl="~/login.aspx"></f:TreeNode>
                                    <f:TreeNode Text="目录" NavigateUrl="~/System/Directory_Edit.aspx"></f:TreeNode>
                                    <f:TreeNode Text="用户查询" NavigateUrl="~/System/User_Query.aspx"></f:TreeNode>
                                    <f:TreeNode Text="部门编辑" NavigateUrl="~/System/Organize_Edit.aspx"></f:TreeNode>
                                    <f:TreeNode Text="角色编辑" NavigateUrl="~/System/Role_Edit.aspx"></f:TreeNode>
                                </f:TreeNode>
                            </Nodes>
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Position="Center"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="首页" Layout="Fit" Icon="House" runat="server" CssStyle="background-color:blueviolet">
                                    <Items>
                                        <f:ContentPanel ID="ContentPanel2" ShowBorder="false" BodyPadding="10px" ShowHeader="false" AutoScroll="true"
                                            runat="server">
                                            <h2>FineUI（开源版）</h2>
                                            基于 ExtJS 的开源 ASP.NET 控件库
                                        
                                            <br />
                                            <h2>FineUI的使命</h2>
                                            创建 No JavaScript，No CSS，No UpdatePanel，No ViewState，No WebServices 的网站应用程序
                                        
                                            <br />
                                            <h2>支持的浏览器</h2>
                                            IE 8.0+、Chrome、Firefox、Opera、Safari
                                        
                                            <br />
                                            <h2>授权协议</h2>
                                            Apache License v2.0（ExtJS 库在 <a target="_blank" href="http://www.sencha.com/license">GPL v3</a> 协议下发布）
                                            
                                            <br />
                                            <h2>相关链接</h2>
                                            首页：<a target="_blank" style="font-weight: bold;" href="http://fineui.com/">http://fineui.com/</a>
                                            <br />
                                            论坛：<a target="_blank" href="http://fineui.com/bbs/">http://fineui.com/bbs/</a>
                                            <br />
                                            示例：<a target="_blank" href="http://fineui.com/demo/">http://fineui.com/demo/</a>
                                            <br />
                                            文档：<a target="_blank" href="http://fineui.com/doc/">http://fineui.com/doc/</a>
                                            <br />
                                            <br />
                                            <br />
                                            <h2><a target="_blank" href="http://fineui.com/pro/">FineUI（专业版）：基于 jQuery 的专业 ASP.NET 控件库</a></h2>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
    <script>
        var menuClientID = '<%= leftMenuTree.ClientID %>';
        var tabStripClientID = '<%= mainTabStrip.ClientID %>';

        // 页面控件初始化完毕后，会调用用户自定义的onReady函数
        F.ready(function () {

            // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
            // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
            // mainTabStrip： 选项卡实例
            // createToolbar： 创建选项卡前的回调函数（接受tabConfig参数）
            // updateLocationHash: 切换Tab时，是否更新地址栏Hash值
            // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
            // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
            F.util.initTreeTabStrip(F(menuClientID), F(tabStripClientID), null, true, false, false);

        });
    </script>
</body>
</html>
