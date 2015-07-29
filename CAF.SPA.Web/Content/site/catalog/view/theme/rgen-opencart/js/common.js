function getURLVar(key) {
	var value = [];

	var query = String(document.location).split('?');

	if (query[1]) {
		var part = query[1].split('&');

		for (i = 0; i < part.length; i++) {
			var data = part[i].split('=');

			if (data[0] && data[1]) {
				value[data[0]] = data[1];
			}
		}

		if (value[key]) {
			return value[key];
		} else {
			return '';
		}
	}
}

$(document).ready(function() {
	// Highlight any found errors
	$('.text-danger').each(function() {
		var element = $(this).parent().parent();
		
		if (element.hasClass('form-group')) {
			element.addClass('has-error');
		}
	});
		
	// Currency
	$('#currency .currency-select').on('click', function(e) {
		e.preventDefault();

		$('#currency input[name=\'code\']').attr('value', $(this).attr('data-name'));

		$('#currency').submit();
	});

	// Language
	$('#language a').on('click', function(e) {
		e.preventDefault();

		$('#language input[name=\'code\']').attr('value', $(this).attr('href'));

		$('#language').submit();
	});

	/* Search */
	$('#search input[name=\'search\']').parent().parent().find('button').on('click', function() {
		url = $('base').attr('href') + 'index.php?route=product/search';

		var value = $('header input[name=\'search\']').val();

		if (value) {
			url += '&search=' + encodeURIComponent(value);
		}

		location = url;
	});

	$('#search input[name=\'search\']').on('keydown', function(e) {
		if (e.keyCode == 13) {
			//$('header input[name=\'search\']').parent().find('button').trigger('click');
			$('#search .search-btn').trigger('click');
		}
	});

	// tooltips on hover
	$('[data-toggle=\'tooltip\']').tooltip({container: 'body'});

	// Makes tooltips work on ajax generated content
	$(document).ajaxStop(function() {
		$('[data-toggle=\'tooltip\']').tooltip({container: 'body'});
	});
});

// Cart add remove functions
var cart = {
	'add': function(product_id, quantity) {
		$.ajax({
			url: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart/add',
			type: 'post',
			data: 'product_id=' + product_id + '&quantity=' + (typeof(quantity) != 'undefined' ? quantity : 1),
			dataType: 'json',
			beforeSend: function() {
				$('#cart > button').button('loading');
			},
			complete: function() {
				$('#cart > button').button('reset');
			},
			success: function(json) {
				$('.alert, .text-danger').remove();

				if (json['redirect']) {
					location = json['redirect'];
				}

				if (json['success']) {
					msg('', json['success'], 'success');

					//$('#content').parent().before('<div class="alert alert-success"><i class="fa fa-check-circle"></i> ' + json['success'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');

					//$('#cart-total').html(json['total']);
					// Need to set timeout otherwise it wont update the total
					setTimeout(function () {
						//$('#cart > button').html('<span id="cart-total"><i class="fa fa-shopping-cart"></i> ' + json['total'] + '</span>');
						if ($('.tbar1').length > 0) {
							var total_val = json['total'].split(" - ");
							$('#cart > button #cart-total').html(total_val[1]);	
						}else{
							$('#cart > button #cart-total').html(json['total']);
						};
						
					}, 100);

					//$('html, body').animate({ scrollTop: 0 }, 'slow');

					$('#cart > ul').load('http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=common/cart/info ul li');
				}
			}
		});
	},
	'update': function(key, quantity) {
		$.ajax({
			url: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart/edit',
			type: 'post',
			data: 'key=' + key + '&quantity=' + (typeof(quantity) != 'undefined' ? quantity : 1),
			dataType: 'json',
			beforeSend: function() {
				$('#cart > button').button('loading');
			},
			complete: function() {
				$('#cart > button').button('reset');
			},
			success: function(json) {

				// Need to set timeout otherwise it wont update the total
				setTimeout(function () {
					//$('#cart > button').html('<span id="cart-total"><i class="fa fa-shopping-cart"></i> ' + json['total'] + '</span>');
					if ($('.tbar1').length > 0) {
						var total_val = json['total'].split(" - ");
						$('#cart > button #cart-total').html(total_val[1]);	
					}else{
						$('#cart > button #cart-total').html(json['total']);
					};
				}, 100);

				if (getURLVar('route') == 'checkout/cart' || getURLVar('route') == 'checkout/checkout') {
					location = 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart';
				} else {
					$('#cart > ul').load('http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=common/cart/info ul li');
				}
			}
		});
	},
	'remove': function(key) {
		$.ajax({
			url: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart/remove',
			type: 'post',
			data: 'key=' + key,
			dataType: 'json',
			beforeSend: function() {
				$('#cart > button').button('loading');
			},
			complete: function() {
				$('#cart > button').button('reset');
			},
			success: function(json) {
				// Need to set timeout otherwise it wont update the total
				setTimeout(function () {
					//$('#cart > button').html('<span id="cart-total"><i class="fa fa-shopping-cart"></i> ' + json['total'] + '</span>');
					if ($('.tbar1').length > 0) {
						var total_val = json['total'].split(" - ");
						$('#cart > button #cart-total').html(total_val[1]);	
					}else{
						$('#cart > button #cart-total').html(json['total']);
					};
				}, 100);

				if (getURLVar('route') == 'checkout/cart' || getURLVar('route') == 'checkout/checkout') {
					location = 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart';
				} else {
					$('#cart > ul').load('http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=common/cart/info ul li');
				}
			}
		});
	}
}

var voucher = {
	'add': function() {

	},
	'remove': function(key) {
		$.ajax({
			url: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart/remove',
			type: 'post',
			data: 'key=' + key,
			dataType: 'json',
			beforeSend: function() {
				$('#cart > button').button('loading');
			},
			complete: function() {
				$('#cart > button').button('reset');
			},
			success: function(json) {
				// Need to set timeout otherwise it wont update the total
				setTimeout(function () {
					$('#cart > button').html('<span id="cart-total"><i class="fa fa-shopping-cart"></i> ' + json['total'] + '</span>');
				}, 100);

				if (getURLVar('route') == 'checkout/cart' || getURLVar('route') == 'checkout/checkout') {
					location = 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=checkout/cart';
				} else {
					$('#cart > ul').load('http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=common/cart/info ul li');
				}
			}
		});
	}
}

var wishlist = {
	'add': function(product_id) {
		$.ajax({
			url: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=account/wishlist/add',
			type: 'post',
			data: 'product_id=' + product_id,
			dataType: 'json',
			success: function(json) {
				$('.alert').remove();

				if (json['success']) {
					msg('', json['success'], 'success');
					//$('#content').parent().before('<div class="alert alert-success"><i class="fa fa-check-circle"></i> ' + json['success'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
				}

				if (json['info']) {
					msg('', json['info'], 'info');
					//$('#content').parent().before('<div class="alert alert-info"><i class="fa fa-info-circle"></i> ' + json['info'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');
				}

				$('#wishlist-total').html(json['total']);
				$('#wishlist-total').attr('title', json['total']);

				//$('html, body').animate({ scrollTop: 0 }, 'slow');
			}
		});
	},
	'remove': function() {

	}
}

var compare = {
	'add': function(product_id) {
		$.ajax({
			url: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=product/compare/add',
			type: 'post',
			data: 'product_id=' + product_id,
			dataType: 'json',
			success: function(json) {
				$('.alert').remove();

				if (json['success']) {
					msg('', json['success'], 'success');
					//$('#content').parent().before('<div class="alert alert-success"><i class="fa fa-check-circle"></i> ' + json['success'] + '<button type="button" class="close" data-dismiss="alert">&times;</button></div>');

					$('#compare-total').html(json['total']);

					//$('html, body').animate({ scrollTop: 0 }, 'slow');
				}
			}
		});
	},
	'remove': function() {

	}
}

/* Agree to Terms */
$(document).delegate('.agree', 'click', function(e) {
	e.preventDefault();

	$('#modal-agree').remove();

	var element = this;

	$.ajax({
		url: $(element).attr('href'),
		type: 'get',
		dataType: 'html',
		success: function(data) {
			html  = '<div id="modal-agree" class="modal">';
			html += '  <div class="modal-dialog">';
			html += '    <div class="modal-content">';
			html += '      <div class="modal-header">';
			html += '        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
			html += '        <h4 class="modal-title">' + $(element).text() + '</h4>';
			html += '      </div>';
			html += '      <div class="modal-body">' + data + '</div>';
			html += '    </div';
			html += '  </div>';
			html += '</div>';

			$('body').append(html);

			$('#modal-agree').modal('show');
		}
	});
});

/* Autocomplete */
(function($) {
	function Autocomplete(element, options) {
		this.element = element;
		this.options = options;
		this.timer = null;
		this.items = new Array();

		$(element).attr('autocomplete', 'off');
		$(element).on('focus', $.proxy(this.focus, this));
		$(element).on('blur', $.proxy(this.blur, this));
		$(element).on('keydown', $.proxy(this.keydown, this));

		$(element).after('<ul class="dropdown-menu"></ul>');
		$(element).siblings('ul.dropdown-menu').delegate('a', 'click', $.proxy(this.click, this));
	}

	Autocomplete.prototype = {
		focus: function() {
			this.request();
		},
		blur: function() {
			setTimeout(function(object) {
				object.hide();
			}, 200, this);
		},
		click: function(event) {
			event.preventDefault();

			value = $(event.target).parent().attr('data-value');

			if (value && this.items[value]) {
				this.options.select(this.items[value]);
			}
		},
		keydown: function(event) {
			switch(event.keyCode) {
				case 27: // escape
					this.hide();
					break;
				default:
					this.request();
					break;
			}
		},
		show: function() {
			var pos = $(this.element).position();

			$(this.element).siblings('ul.dropdown-menu').css({
				top: pos.top + $(this.element).outerHeight(),
				left: pos.left
			});

			$(this.element).siblings('ul.dropdown-menu').show();
		},
		hide: function() {
			$(this.element).siblings('ul.dropdown-menu').hide();
		},
		request: function() {
			clearTimeout(this.timer);

			this.timer = setTimeout(function(object) {
				object.options.source($(object.element).val(), $.proxy(object.response, object));
			}, 200, this);
		},
		response: function(json) {
			html = '';

			if (json.length) {
				for (i = 0; i < json.length; i++) {
					this.items[json[i]['value']] = json[i];
				}

				for (i = 0; i < json.length; i++) {
					if (!json[i]['category']) {
						html += '<li data-value="' + json[i]['value'] + '"><a href="#">' + json[i]['label'] + '</a></li>';
					}
				}

				// Get all the ones with a categories
				var category = new Array();

				for (i = 0; i < json.length; i++) {
					if (json[i]['category']) {
						if (!category[json[i]['category']]) {
							category[json[i]['category']] = new Array();
							category[json[i]['category']]['name'] = json[i]['category'];
							category[json[i]['category']]['item'] = new Array();
						}

						category[json[i]['category']]['item'].push(json[i]);
					}
				}

				for (i in category) {
					html += '<li class="dropdown-header">' + category[i]['name'] + '</li>';

					for (j = 0; j < category[i]['item'].length; j++) {
						html += '<li data-value="' + category[i]['item'][j]['value'] + '"><a href="#">&nbsp;&nbsp;&nbsp;' + category[i]['item'][j]['label'] + '</a></li>';
					}
				}
			}

			if (html) {
				this.show();
			} else {
				this.hide();
			}

			$(this.element).siblings('ul.dropdown-menu').html(html);
		}
	};

	$.fn.autocomplete = function(option) {
		return this.each(function() {
			var data = $(this).data('autocomplete');

			if (!data) {
				data = new Autocomplete(this, option);

				$(this).data('autocomplete', data);
			}
		});
	}
})(window.jQuery);


/* THEME JAVASCRIPT
********************************************/

/* General functions
------------------------*/
generalFn = function(){
	$('.ly-column').find('.mod-hd').addClass('col-mod-hd').removeClass('mod-hd');

	$('.page-head-1 .breadcrumb').prependTo('header .breadcrumb-wrp .container');
	$('#pg-hd-out').prependTo('.page-head-out-wrp');
	
	if ($('.layout-wrp .page-content *').length == 0) {
		if ($('.layout-wrp .ly-column').length > 0) {
			if ($('.layout-wrp .ly-column *').length == 0) {
				$('.content-area > .container').addClass('empty-page');
			};
		} else {
			if ($('.layout-wrp > #content *').length == 1) {
				$('.content-area > .container').addClass('empty-page');
			};
		};
	};
	
	
	/* CATEGORIES
	------------------------*/
	$(".col-links .cc").each(function(index, element) {
		if($(this).parent().hasClass('cat-active') == true){
			$(this).addClass('open');
			$(this).next().addClass('active');
			$(this).find('.fa').removeClass('fa-plus-circle').addClass('fa-minus-circle');
		}	
	});
	$(".col-links .cc").click(function(){ 
		var hdl = $(this);
		if($(this).next().is(':visible') == false) {
			$('.box-category .col-subcat').slideUp(100, function(){
				$(this).removeClass('active');
				$('.cc').removeClass('open');
			});
		}
		if($(this).hasClass('open') == true) {
			$(this).next().slideUp(100, function(){
				$(this).removeClass('active');
				$(this).prev().removeClass('open');
				hdl.find('.fa').removeClass('fa-minus-circle').addClass('fa-plus-circle');
			});
		}else{
			$(this).next().slideDown(100, function(){
				$(this).addClass('active');
				$(this).prev().addClass('open');
				hdl.find('.fa').removeClass('fa-plus-circle').addClass('fa-minus-circle');
			});
		}
	});

	/* Top links drop down
	------------------------*/
	$('.t-dd').on('mouseenter', function() {
		$('.t-dd').removeClass('active');
		$(this).addClass('active');
	});
	$('.t-dd').on('mouseleave', function() {
		$('.t-dd').removeClass('active');
	});

	/* Menu
	------------------------*/
	$('#menu .dropdown-menu').each(function() {
		var menu = $('#menu').offset();
		var dropdown = $(this).parent().offset();
		var i = (dropdown.left + $(this).outerWidth()) - (menu.left + $('#menu').outerWidth());
		if (i > 0) {
			$(this).css('margin-left', '-' + (i + 5) + 'px');
		}
	});
	$('#menu').addClass($('#menu .rg-nav-wrp').attr('data-w'));

	// Vertical middle class
	$('.vm').each(function(){ $(this).children().addClass('vm-item'); });

	/* Page scroll back to top
	------------------------*/
	var offset = 220;
	var duration = 500;
	$(window).scroll(function() {
		if ($(this).scrollTop() > offset) {
			$('.scroll-top').fadeIn(duration);
		} else {
			$('.scroll-top').fadeOut(duration);
		}
	});
	$('body').on('click', '.scroll-top', function(event) {
		console.log('test');
		event.preventDefault();
		$('html, body').animate({scrollTop: 0}, duration);
		return false;
	});


	/* Product grids
	------------------------*/
	$('.prd-block1 .other-btn').css({ marginTop: -($('.prd-block1 .other-btn').outerHeight()/2) });
	$('.prd-block .normal-off').css({display: 'none'});
	$('.prd-block .normal-on').css({display: ''});

	
	$('.product-filter .display button').click(function() {
		$('#content .prd-container').css({opacity: 0}).animate({ opacity: 1 }, 1200);
	});
	
	// Product List
	$('#list-view').click(function() {
		$('#content .display button').removeClass('active');
		$(this).addClass('active');
		$('#content .product-grid').attr('class', 'product-layout product-list cl');
		$('#content .prd-container').removeClass($('#content .prd-container').attr('data-gridview')).addClass($('#content .prd-container').attr('data-listview'));
		$('.product-list .list-off').css({display: 'none'});
		$('.product-list .list-on').css({display: ''});
		equalH('.prd-container', '.product-list', 'reset');
		localStorage.setItem('display', 'list');
	});

	// Product Grid
	$('#grid-view').click(function() {
		$('#content .display button').removeClass('active');
		$(this).addClass('active');
		$('#content .product-list').attr('class', 'product-layout product-grid cl');
		$('#content .prd-container').removeClass($('#content .prd-container').attr('data-listview')).addClass($('#content .prd-container').attr('data-gridview'));
		$('.product-grid .grid-off').css({display: 'none'});
		$('.product-grid .grid-on').css({display: ''});
		equalH('.prd-container', '.product-grid');
		$('.product-grid img').load(function() {
			equalH('.prd-container', '.product-grid');
		});
		localStorage.setItem('display', 'grid');
	});

	if (localStorage.getItem('display') == 'list') {
		$('#list-view').trigger('click');
	} else {
		$('#grid-view').trigger('click');
	}

	/* Widgets
	------------------------*/
	// Carousel
	$('[data-widget="autoCarousel"]').each(function(index, el) {
		rgenCarousel(this);
	});

	// Tabs
	widgetTabs();
}

/* Widget - Carousel
------------------------*/
widgetCarousel = function (obj, responsiveData) {
	rgenCarousel(obj, responsiveData);
}
rgenCarousel = function (obj, responsiveData) {
	var owl = $(obj);
	owl.css({opacity: 0});
	// Default responsive
	var resObj = {
		0    : { items:1 },
		320  : { items:1 },
		400  : { items:2 },
		480  : { items:2 },
		600  : { items:3 },
		700  : { items:3 },
		768  : { items:3 },
		800  : { items:3 },
		900  : { items:4 },
		980  : { items:4 },
		1100 : { items: owl.attr('data-resitems') ? parseInt(owl.attr('data-resitems')) : 5 }
	}

	typeof responsiveData == 'undefined' ? responsiveData = resObj : responsiveData;
	
	var stagePadding = getvar(owl.attr('data-stpd'), 0, 'n');
	var items        = getvar(owl.attr('data-items'), 5, 'n');
	var margin       = getvar(owl.attr('data-margin'), 10, 'n');
	var nav          = getvar(owl.attr('data-nav'), true, 'b');
	var dots         = getvar(owl.attr('data-dots'), false, 'b');
	var slideby      = getvar(owl.attr('data-slideby'), 1, 'n');
	var rbase        = getvar(owl.attr('data-rbase'), $(obj).parent(), 's');
	var res          = owl.attr('data-res') && owl.attr('data-res') == 'true' ? responsiveData : false;

	owl.owlCarousel({
		stagePadding: stagePadding,
		items: items,
		margin: margin,
		nav: nav,
		dots: dots,
		slideBy: slideby,
		loop: false,
		navText: ['<i class="fa fa-chevron-left"></i>', '<i class="fa fa-chevron-right"></i>'],
		responsiveBaseElement: rbase,
		responsive: res,
		onInitialized: function () {
			owl.animate({opacity: 1}, 300);
		}
	});
	if (nav == false) { owl.find('.owl-nav').hide(); };

	if ($(obj).hasClass('ctrl-t')) {
		var arrowHtm = '<div class="owl-nav"><a class="prev"><i class="fa fa-chevron-left"></i></a><a class="next"><i class="fa fa-chevron-right"></i></a></div>';
		$(obj).parent().parent().find('.mod-hd').append(arrowHtm);
		$(obj).parent().parent().find('.mod-hd').find('.next').click(function() {
			owl.trigger('next.owl.carousel');
		});
		$(obj).parent().parent().find('.mod-hd').find('.prev').click(function() {
			owl.trigger('prev.owl.carousel');
		});
	};
}
getvar = function (v, default_v, val_type) {
	if (val_type == 'n') {
		return v ? parseInt(v) : default_v;
	} 
	if (val_type == 'b') {
		if (v == 'true') { return true; }
		else if (v == 'false') { return false; }
		else { return default_v; }
	}
	if (val_type == 's') {
		return v ? v : default_v;
	}
}

/* Banner grids
------------------------*/
bnrgrids = function (obj, settings) {
	var grids = $(obj);
	grids.each(function(index, el) {

		var wrp = this;
		$(wrp).responsivegrid({
			'gutter' : settings.gutter,
			'itemSelector' : settings.itemclass,
			'breakpoints': {
				'desktop' : {
					'range' : '1200-',
					'options' : {
						'column' : 12,
					}
				},
				'tablet-landscape' : {
					'range' : '980-1200',
					'options' : {
						'column' : 12,
					}
				},
				'tablet-portrate' : {
					'range' : '767-980',
					'options' : {
						'column' : 6,
					}
				},
				'mobile' : {
					'range' : '-767',
					'options' : {
						'column' : 6,
					}
				},
			}
		});

		// Set width on resizing
		setContainerWidth = function (wrp) {
			var w = 0;
			var gt = parseInt(settings.gutter.replace("px", ""), 0);
			$(wrp).find(settings.itemclass).each(function(index, el) {
				if ($(this).css('top') == '0px') {
					w += $(this).width()+gt;
				};
			});
			$(wrp).css({'max-width': w-gt});
		}
		
		$(wrp).css({'max-width': 'none', 'opacity': 0});
		$(wrp).stop().animate({opacity: 1}, 500, function () {
			setContainerWidth(wrp);
		});
		
		$(window).resize(function(event) {
			$(wrp).css({'max-width': 'none', 'opacity': 0});
			$(wrp).stop().animate({opacity: 1}, 500, function () {
				setContainerWidth(wrp);
			});
		});
	});
}

/* Sudo slider widget
------------------------*/
sudoSliderfn = function (obj, settings) {
	var auto        = true;
	var autostopped = false;

	var sudoSlider = $(obj).sudoSlider({
		responsive   : true,
		controlsAttr : 'class="owl-controls"',
		effect       : settings.effect,
		speed        : settings.speed,
		auto         : settings.auto,
		pause        : settings.pause,
		prevNext     : settings.prevNext,
		nextHtml     : '<a class="next"><i class="fa fa-chevron-right"></i></a>',
		prevHtml     : '<a class="prev"><i class="fa fa-chevron-left"></i></a>',
		numeric      : settings.pager,
		numericAttr  : 'class="dots ul-reset"',
		continuous   : settings.continuous,
		updateBefore : true,
		mouseTouch   : false,
		touch        : true,
		slideCount   : 1
	})
	.mouseenter(function() {
		if (settings.stoponhover) {
			auto = sudoSlider.getValue('autoAnimation');
			if (auto) { sudoSlider.stopAuto(); } else { autostopped = true; }
		};
	})
	.mouseleave(function() {
		if (settings.stoponhover) {
			if (!autostopped) { sudoSlider.startAuto(); }
		}
	})
}


/* PhotoSwipe gallery widget
------------------------*/
photoSwipe_fn = function(gallerySelector) {
	
	var	pswpHtm  = '<div class="pswp" tabindex="-1" role="dialog" aria-hidden="true">';
		pswpHtm += '	<div class="pswp__bg"></div>';
		pswpHtm += '	<div class="pswp__scroll-wrap">';
		pswpHtm += '		<div class="pswp__container"><div class="pswp__item"></div><div class="pswp__item"></div><div class="pswp__item"></div></div>';
		pswpHtm += '		<div class="pswp__ui pswp__ui--hidden">';
		pswpHtm += '			<div class="pswp__top-bar">';
		pswpHtm += '				<div class="pswp__counter"></div>';
		pswpHtm += '				<button class="pswp__button pswp__button--close" title="Close (Esc)"></button>';
		pswpHtm += '				<button class="pswp__button pswp__button--share" title="Share"></button>';
		pswpHtm += '				<button class="pswp__button pswp__button--fs" title="Toggle fullscreen"></button>';
		pswpHtm += '				<button class="pswp__button pswp__button--zoom" title="Zoom in/out"></button>';
		pswpHtm += '				<div class="pswp__preloader">';
		pswpHtm += '					<div class="pswp__preloader__icn">';
		pswpHtm += '						<div class="pswp__preloader__cut">';
		pswpHtm += '							<div class="pswp__preloader__donut"></div>';
		pswpHtm += '						</div>';
		pswpHtm += '					</div>';
		pswpHtm += '				</div>';
		pswpHtm += '			</div>';
		pswpHtm += '			<div class="pswp__share-modal pswp__share-modal--hidden pswp__single-tap">';
		pswpHtm += '				<div class="pswp__share-tooltip"></div>';
		pswpHtm += '			</div>';
		pswpHtm += '			<button class="pswp__button pswp__button--arrow--left" title="Previous (arrow left)"></button>';
		pswpHtm += '			<button class="pswp__button pswp__button--arrow--right" title="Next (arrow right)"></button>';
		pswpHtm += '			<div class="pswp__caption">';
		pswpHtm += '				<div class="pswp__caption__center"></div>';
		pswpHtm += '			</div>';
		pswpHtm += '		</div>';
		pswpHtm += '	</div>';
		pswpHtm += '</div>';

	if ($('.pswp').length == 0) {
		$('body').append(pswpHtm);
	};
	var pswpElement = document.querySelectorAll('.pswp')[0];

	var gallery_items = [];
	$(gallerySelector).find('.gallery-item').each(function(index, el) {
		var img_size = $(el).find('.pop').attr('data-size').split('x');
		$(el).attr("data-index", index);

		// Gallery data
		itemData = {
			src: $(el).find('.pop').attr('href'),
			w: img_size[0],
			h: img_size[1],
			msrc: $(el).find('img').attr('src'),
			title: $(el).find('figcaption').html()
		};
		gallery_items.push(itemData);
	});

	$(gallerySelector).on('click', '.pop', function(event) {
		event.preventDefault();
		var th = this;
		item_index = parseInt($(th).closest(".gallery-item").attr('data-index'));
		options = {
			index: item_index,
			shareButtons: [
				{id:'facebook', label:'Share on Facebook', url:'https://www.facebook.com/sharer/sharer.php?u={{url}}'},
				{id:'twitter', label:'Tweet', url:'https://twitter.com/intent/tweet?text={{text}}&url={{url}}'},
				{id:'pinterest', label:'Pin it', url:'http://www.pinterest.com/pin/create/button/?url={{url}}&media={{image_url}}&description={{text}}'}
				//{id:'download', label:'Download image', url:'{{raw_image_url}}', download:true}
			]
			/*getThumbBoundsFn: function(index) {
				var thumbnail = $(th).closest(".gallery-item"),
				pageYScroll   = window.pageYOffset || document.documentElement.scrollTop,
				rect          = thumbnail.offset();
				return {x:rect.left, y:rect.top, w:thumbnail[0].clientWidth};
			}*/
		}
		var gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, gallery_items, options);
		gallery.init();
	});
};


/* Widget - Tabs
------------------------*/
widgetTabs = function () {
	var tabItem = 0;
	$('[data-widget="autoTab"]').each(function(index, el) {
		rgenTabs(this, tabItem);
	tabItem++; });
}
rgenTabs = function (obj, item) {
	$(obj).find('.tab-panel .tab-item').each(function(index, el) {
		if (index == 0) { $(this).addClass('active'); };
		$(this).children('a').attr('href', '#htab'+item+index);
	});
	$(obj).find('.tab-pane-wrp > .tab-pane').each(function(index, el) {
		if (index == 0) { $(this).addClass('active'); };
		$(this).attr('id', 'htab'+item+index);
	});

	$(obj).find('.tab-panel .tab-item > a').click(function (e) {
		e.preventDefault();
		$(this).tab('show');
	})
	$(obj).animate({opacity: 1}, 200);
}

/* Logo in nav adjustment
------------------------*/
adjustLogo = function(){
	var logo_w = parseInt($('.logo-wrp').attr('data-w'));
	$(".logo-wrp").css({width: logo_w});
}

$(window).delay(100).resize(function(event) {
	equalH('.prd-container', '.product-grid');
});
$(document).ready(function() {
	adjustLogo();
	generalFn();
});

/* Basic slide show module functions
------------------------*/
var bss = {
	onResize: function (sizetype, h, slider, container) {
		var win = $(window);
		var obj = container.parent().attr('data-id')
		win.on("resize blur focus", function () {
			var height = sizetype == 'wfs' ? win.height() : h;
			slider.height(height);
			container.height(height);

			if (win.width() < 979) {
				container.css({ height: container.find(".slide > img").height() });
				container.find('.slideshow').css({ height: container.find(".slide > img").height() });
			};

		}).resize();
		if (sizetype == 'wfs' || sizetype == 'pfs') {
			setTimeout(function () {
				$(obj).css({ marginTop: $(obj).attr('data-top')+'px' });
			}, 200); 
		};
	},
	fullSlider: function (slider) {
		var win = $(window);
		slider.find(".slide").each(function () {
			var slide = $(this);
			var imageSrc = slide.attr("data-background");
			if (!imageSrc) { return; }

			$("<img />").attr("src", imageSrc).properload(function () {
				var backgroundImage = $(this);
				var imageHeight = backgroundImage[0].naturalHeight;
				var imageWidth = backgroundImage[0].naturalWidth;

				if (!imageHeight) {
					var img = new Image();
					img.src = imageSrc;
					imageWidth = img.width;
					imageHeight = img.height;
				}

				var aspectRatio = imageWidth / imageHeight;

				backgroundImage.appendTo(slide);

				slide.css({ zIndex: 0 });

				backgroundImage.css({
					position: "absolute",
					zIndex: -1,
					top: 0,
					left: 0
				});

				win.on("resize blur focus", function () {
					var sliderWidth = slider.width();
					var sliderHeight = slider.height();
					if (win.width() > 979) {
						if ((sliderWidth / sliderHeight) < aspectRatio ) {
							var leftMargin = ((sliderWidth - (sliderHeight * aspectRatio)) / 2) + "px";
							backgroundImage.css({
								top: 0,
								left: leftMargin,
								width: sliderHeight * aspectRatio,
								height: sliderHeight
							});
						} else {
							backgroundImage.css({
								left: 0,
								top: ((sliderHeight - (sliderWidth / aspectRatio)) / 2) + "px",
								height: sliderWidth / aspectRatio,
								width: sliderWidth
							});
						}
					}
				}).resize();

			}, true);
		});
	}
}

/* Ajax search
------------------------*/
ajaxSearch = function (obj) {
	$(obj).devbridgeAutocomplete({
		serviceUrl: 'http://rgenmodernstore.rgenesis.com/01/catalog/view/theme/rgen-opencart/js/index.php?route=rgen/search',
		deferRequestBy: 500,
		paramName: 'search',
		maxHeight: 400,
		width: 300,
		onSelect: function (suggestion) {
			location = suggestion.href;
		},
		transformResult: function (response) {
			response = $.parseJSON(response);
			return {
				suggestions: $.map(response, function(dataItem) {
					return { 
						value      : dataItem.name,
						data       : dataItem,
						product_id : dataItem.product_id,
						thumb      : dataItem.thumb,
						price      : dataItem.price,
						special    : dataItem.special,
						href       : unescape(dataItem.href)
					};
				})
			};
		},
		formatResult: function (suggestion, currentValue) {
			html  = '<div class="search-prd">';
			html += '	<a class="image" href="'+suggestion.href+'"><img src="'+suggestion.thumb+'" alt="'+suggestion.value+'" title="'+suggestion.value+'" class="img-responsive" /></a>';
			html += '	<div class="info">';
			html += '		<a class="name" href="'+suggestion.href+'">'+suggestion.value+'</a>';
							if (suggestion.price) {
			html += '		<p class="price">';
							if (!suggestion.special) {
			html += '			<span class="price-new">'+suggestion.price+'</span>';
							} else {
			html += '			<span class="price-new price-spl">'+suggestion.special+'</span>';
			html += '			<span class="price-old">'+suggestion.price+'</span>';
							}
			html += '		</p>';
							}
			html += '	</div>';
			html += '</div>';

			return html;
		},
		onSearchStart: function (query) {},
		onSearchComplete: function (query, suggestions) {
			if ($('.autocomplete-suggestions .view-more-wrp').length == 0) {
				$('.autocomplete-suggestions').append('<div class="view-more-wrp"><a onclick="$(\'#search .search-btn\').trigger(\'click\');" class="search-prd">View more...</a></div>');
			};
		},
		beforeRender: function (container) {
			if ($("#search").parent().hasClass('search-2')) {
				$(container).addClass('search2');
			}else{
				$(container).removeClass('search2');
			}
		}
	});
	
	function search1() {
		if($('.autocomplete-suggestions:visible').length != 0) {
			$(".search-1").addClass('active');
		} else {
			$(".search-1").removeClass('active');
		}
	}
	if ($(".top-search-wrp").hasClass('search-1')) {
		$("body").on('mousemove', function() { search1(); });
	};
}

/* Sticky html
------------------------*/
fn_stickyhtml = function (obj) {
	if ($(obj).hasClass('sticky-l')) {
		var w = $(obj).outerWidth();

		$(obj).css({left: -w});

		$(obj).find('.sticky-handle').on('mouseenter', function(){
			$(obj).addClass('active');
			$(this).parent().parent().stop().animate({'left' : '0px'});
		});

		$(obj).on('mouseleave', function(){
			$(this).stop().animate({'left' : -w}, function(){
				$(this).removeClass('active');
			});
		});
	};
	
	if ($(obj).hasClass('sticky-r')) {
		var w = $(obj).outerWidth();

		$(obj).css({right: -w});

		$(obj).find('.sticky-handle').on('mouseenter', function(){
			$(obj).addClass('active');
			$(this).parent().parent().stop().animate({'right' : '0px'});
		});

		$(obj).on('mouseleave', function(){
			$(this).stop().animate({'right' : -w}, function(){
				$(this).removeClass('active');
			});
		});
	};
}



/* Equal height
------------------------*/
/* Thanks to CSS Tricks for pointing out this bit of jQuery
http://css-tricks.com/equal-height-blocks-in-rows/
It's been modified into a function called at page load and then each time the page is resized. One large modification was to remove the set height before each new calculation. */
equalH = function(parent, child, reset){
	
	var reset = typeof reset == "undefined" ? 'no' : 'reset';
	
	if (reset != 'reset') {
		$(parent).each(function() {
			var currentTallest = 0,
				currentRowStart = 0,
				rowDivs = [],
				$el,
				topPosition = 0;
			$(this).children(child).each(function() {
				$el = $(this);
				$($el).css({minHeight: ''});
				
				topPostion = $el.position().top;

				if (currentRowStart != topPostion) {
					for (currentDiv = 0 ; currentDiv < rowDivs.length ; currentDiv++) {
						rowDivs[currentDiv].css({minHeight: currentTallest});	
					}
					rowDivs = []; // empty the array
					currentRowStart = topPostion;
					currentTallest = $el.height();
					rowDivs.push($el);
				} else {
					rowDivs.push($el);
					currentTallest = (currentTallest < $el.height()) ? ($el.height()) : (currentTallest);
				}
				for (currentDiv = 0 ; currentDiv < rowDivs.length ; currentDiv++) {
					rowDivs[currentDiv].css({minHeight: currentTallest});
				}
			});
		});
	} else {
		$(parent).each(function() {
			$(this).children(child).each(function() {
				$el = $(this);
				$el.css({minHeight: ''});
			});
		});
	};

}

/* Common alert */
function msg(title, text, type) {
	if (title != '') {
		new PNotify({ 
			title: title,
			text: text,
			type: type,
			animate_speed: 'fast',
			mouse_reset: false,
			icon: false
		});
	}else{
		new PNotify({ 
			text: text,
			type: type,
			animate_speed: 'fast',
			mouse_reset: false,
			icon: false
		});
	};
}

/* Custom scrollbar
------------------------*/
$(window).load(function(){
	$('.scrollbar').each(function(){
		//$(this).hasClass('h-bar') ? var direction = "x" : var direction = "y";
		var direction = $(this).hasClass('h-bar') ? "x" : "y";
		
		$(this).mCustomScrollbar({
			axis:direction,
			scrollbarPosition:"outside",
			autoDraggerLength: false/*,
			advanced:{autoExpandHorizontalScroll:true}*/
		});
	});
});

/* Quantity box
------------------------*/
qtyPlus = function(){
	var qty = parseInt($('#input-quantity').val());
	if(qty > 0){
		$('#input-quantity').val(qty+1);
	}
	return false;
}
qtyMinus = function(){
	var qty = parseInt($('#input-quantity').val());
	if(qty > 1){
		$('#input-quantity').val(qty-1);
	}
	return false;
}

/*******************************************/