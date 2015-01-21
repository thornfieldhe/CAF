(function () {
    $.fn.extend({
        sidebarOnSelect: function () {
            if ($(this).children('[class=arrow]').length==0) {
                $('.page-sidebar-menu').find('li > a> span').remove('[class="selected"]');
                $('.page-sidebar-menu').find('li  ').removeClass('active open');

                $(this).parents('li').addClass('active');
                $(this).parents('li').find('span[class=title]').after(' <span class="selected"></span>');
                $(this).parents('li').find('span[class=title]').parent('a').parent('li').addClass(' open');
            }
        },
        name2: function () {
            //功能代码
        },
        //其他功能
    });
})(jQuery);