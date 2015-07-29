$.fn.rgenmenu = function (options) {
    var settings = $.extend({
        mainitem: ".rg-nav > li"
    }, options);
    var menu = this;
    $(menu).find(settings.mainitem).on('mouseenter', function () {
        $(this).addClass('active');

        if ($(this).children('.sub').length != 0) {
            $(this).children('.sub').removeAttr('style');

            if (!$(menu).hasClass('v-menu')) {
                var submenu = $(this).children('.sub');
                var menuinner = $(menu).offset();
                var dropdown = $(submenu).offset();
                //i = (dropdown.left + $(submenu).outerWidth()) - (menu.left + $('.rg-nav').outerWidth());
                i = (dropdown.left + $(submenu).outerWidth()) - (menuinner.left + $(menu).outerWidth());
                if (i > 0) {
                    $(submenu).css({
                        marginLeft: '-' + (i) + 'px',
                        top: $(this).outerHeight()
                    });
                } else {
                    $(submenu).css({ top: $(this).outerHeight() });
                }
            };

            equalH('.active .rw', '[class*=cl]');
        }
    });

    $(menu).find(settings.mainitem).on('mouseleave', function () {
        var obj = this;
        $(this).removeClass('active');
    });

    $(menu).find(".rg-nav li").each(function () {
        if ($(this).children('.nav-fly').length > 0) {
            $(this).prepend('<b class="fa fa-plus nav-sub-handle"></b>');
            $(this).prepend('<i class="fa fa-angle-right"></i>');
        };
        if ($(this).children('.sub').length > 0) {
            $(this).prepend('<b class="fa fa-plus nav-sub-handle"></b>');
        };
    });

    /*$(menu).find(settings.mainitem).each(function() {
		var lbl = $(this).children('.main-item').find('.nav-lbl');
		lbl.css({
			marginLeft: -(lbl.outerWidth() / 2),
			top: -(lbl.outerHeight()-2)
		});
	});*/

    $(menu).find(".cat-block2").each(function () {
        var obj = this;
        $(this).find('a').hover(
			function () { $(obj).find('.cat-img img').attr('src', $(this).attr('data-image')); },
			function () { $(obj).find('.cat-img img').attr('src', $(obj).find('.hd a').attr('data-image')); }
		);
    });

    if ($('html').hasClass('res_y')) {
        enquire.register("only screen and (min-width: 200px) and (max-width: 979px)", {
            match: function () {

                $(menu).find(".rg-nav-handle").on('click', function () {
                    if ($(this).next(".rg-nav").hasClass('open')) {
                        $(this).find(".fa").addClass('fa-bars').removeClass('fa-times');
                        $(this).next(".rg-nav").removeClass('open');
                    } else {
                        $(this).find(".fa").addClass('fa-times').removeClass('fa-bars');
                        $(this).next(".rg-nav").addClass('open');
                        subhandle();
                    };
                    $(menu).find(settings.mainitem).each(function (index, val) {
                        if ($(this).find('.nav-lbl').length > 0) {
                            if ($(this).find('.nav-lbl-wrp').length == 0) {
                                $(this).find('.nav-lbl').wrap('<span class="nav-lbl-wrp">');
                                $(this).find('.nav-lbl-wrp').css({ marginTop: -Math.round($(this).find('.nav-lbl').outerHeight() / 2) });
                            };
                        };
                        subhandle(this);
                    });
                });

                $(menu).find(".rg-nav > li").off('mouseenter');
                $(menu).find(".rg-nav > li").off('mouseleave');



                $(menu).find(".rg-nav > li > .nav-sub-handle").on('click', function () {
                    var obj = this;
                    switchclass(obj);
                    subhandle1(obj);
                });

                $(menu).find(".rg-nav > li .sub li > .nav-sub-handle").on('click', function () {
                    var obj = this;
                    switchclass(obj);
                    subhandle1(obj);
                });

                function switchclass(obj) {
                    if ($(obj).parent().hasClass('active')) {
                        $(obj).addClass('fa-plus').removeClass('fa-minus');
                        $(obj).parent().removeClass('active');
                    } else {
                        $(obj).addClass('fa-minus').removeClass('fa-plus');
                        $(obj).parent().addClass('active');
                        subhandle();
                    };
                }
                function subhandle(obj) {
                    if ($(obj).children('.nav-sub-handle').length > 0) {
                        $(obj).children('.nav-sub-handle').css({ height: $(obj).find('.main-item').outerHeight() });
                    };
                }
                function subhandle1(obj) {
                    $(obj).parent().find('.sub .mg-items li').each(function (index, el) {
                        if ($(this).find('.nav-sub-handle').length > 0) {
                            $(this).find('.nav-sub-handle').css({ height: $(this).find('.sub-item').outerHeight() });
                        }
                    });

                }


            },
            unmatch: function () {
                $(menu).find(".rg-nav > li").on('mouseenter');
                $(menu).find(".rg-nav > li").on('mouseleave');

                $(menu).find(settings.mainitem).each(function (index, val) {
                    if ($(this).find('.nav-lbl').length > 0) {
                        $(this).find('.nav-lbl').unwrap();
                    };
                });
            }
        });
    };


};

