F.ready(function () {
    // IDS：default.aspx.cs 中向页面输出的控件客户端ID集合
    var btnExpandAll = F(IDS.btnExpandAll);
    var btnCollapseAll = F(IDS.btnCollapseAll);
    var mainMenu = F(IDS.mainMenu);
    var mainTabStrip = F(IDS.mainTabStrip);
    console.log(mainTabStrip);

    if (window.Ext) {
        F.Button = Ext.Button;
        F.Toolbar = Ext.Toolbar;
    }


    // 当前展开的手风琴面板
    function getExpandedPanel() {
        var panel = null;
        mainMenu.items.each(function (item) {
            if (!item.getCollapsed()) {
                panel = item;
            }
        });
        return panel;
    }



    function createToolbar(tabConfig) {

        // 由工具栏上按钮获得当前标签页中的iframe节点
        function getCurrentIFrameNode(btn) {
            return $('#' + btn.id).parents('.f-tab').find('iframe');
        }

        var openNewWindowButton = new F.Button({
            text: '新标签页中打开',
            type: 'button',
            icon: './res/icon/tab_go.png',
            listeners: {
                click: function () {
                    var iframeNode = getCurrentIFrameNode(this);
                    window.open(iframeNode.attr('src'), '_blank');
                }
            }
        });

        var refreshButton = new F.Button({
            text: '刷新',
            type: 'button',
            icon: './res/icon/reload.png',
            listeners: {
                click: function () {
                    var iframeNode = getCurrentIFrameNode(this);
                    iframeNode[0].contentWindow.location.reload();
                }
            }
        });

        var toolbar = new F.Toolbar({
            items: ['->', refreshButton, '-', openNewWindowButton]
        });

        tabConfig['tbar'] = toolbar;
    }



    // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
    // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
    // mainTabStrip： 选项卡实例
    // createToolbar： 创建选项卡前的回调函数（接受tabConfig参数）
    // updateLocationHash: 切换Tab时，是否更新地址栏Hash值
    // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
    // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
    F.util.initTreeTabStrip(mainMenu, mainTabStrip, createToolbar, true, false, false);



    // 添加示例标签页
    window.addExampleTab = function (id, url, text, icon, refreshWhenExist) {
        // 动态添加一个标签页
        // mainTabStrip： 选项卡实例
        // id： 选项卡ID
        // url: 选项卡IFrame地址 
        // text： 选项卡标题
        // icon： 选项卡图标
        // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
        // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        F.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
    };

    // 移除选中标签页
    window.removeActiveTab = function () {
        var activeTab = mainTabStrip.getActiveTab();
        mainTabStrip.removeTab(activeTab.id);
    };

});