$(document).ready(function(e) {
	
	enquire.register("only screen and (min-width: 1200px)", {
		match : function() {
			
		}
	})
	.register("only screen and (min-width: 980px) and (max-width: 1199px)", {
		match : function() {
			
		}
	})
	.register("only screen and (min-width: 200px) and (max-width: 979px)", {
		match : function() {
			
			$("#menu").addClass('mob');

			// Common
			$("#cart").appendTo('#menu .rg-nav-wrp');
			$("#menu #cart").click(function() {
				if ($('#menu').width() < 451) {
					$(this).find(".dropdown-menu li").css({minWidth: $('#menu').width()});	
				};
			});
			$("#search").appendTo('.m-search');
			$(".top-links").prependTo('.m-dd .t-dd-menu');

			// Topbar 1 changes
			if ($('.tbar1').length > 0) {
				$(".tbar-tools").prependTo('.tbar-row');
			};

			// Topbar 2 changes
			if ($('.logo-wrp').length > 0) {
				$(".logo-wrp #logo").prependTo('.tbar-cell.m');
			};
			
		},
		unmatch : function() {
			$("#menu").removeClass('mob');

			// Common
			$("#search").appendTo('.top-search-wrp');
			$("#cart .dropdown-menu li").removeAttr('style');

			// Topbar 1 changes reward
			if ($('.tbar1').length > 0) {
				$("#cart").appendTo('.tbar1 .tbar-row .top-other-wrp');
				$(".tbar-tools").prependTo('.tbar-row > .tbar-cell.r');
				$(".top-links").prependTo('.top-links-wrp');
			};

			// Topbar 2 changes
			if ($('.logo-wrp').length > 0) {
				$(".tbar-cell.m #logo").prependTo('.logo-wrp');
			};
			if ($('.tbar2').length > 0) {
				$(".l-tlinks").prependTo('.tbar2 .upper-top .l');
				$(".r-tlinks").prependTo('.tbar2 .upper-top .r');
				$("#cart").appendTo('.tbar2 .tbar-row > .r .cart-lg');
			};
			
		}
		
	})
	.register("only screen and (min-width: 768px) and (max-width: 979px)", {
		match : function() {},
		unmatch : function() {}
	})
	.register("only screen and (min-width: 200px) and (max-width: 767px)", {
		match : function() { 
			// Product page 
			$('.product-info1 .price-wrp').prependTo('.m-data'); 
			
			// Footer
			$('.ft-links-wrp .ft-hd + .ul-reset').hide();
			$('.ft-links-wrp .ft-hd').on('click', function () {
				$('.ft-links-wrp .ft-hd + .ul-reset').hide();
				$(this).next('.ul-reset').show();
			});
		},
		unmatch : function() {  
			// Product page 
			$('.m-data .price-wrp').prependTo('.product-info1 .r .buying-info');

			// Footer
			$('.ft-links-wrp .ft-hd + .ul-reset').show();
		}
	})
	.register("only screen and (min-width: 600px) and (max-width: 767px)", {
		match : function() {  },
		unmatch : function() {  }
	})
	.register("only screen and (min-width: 480px) and (max-width: 599px)", {
		match : function() {  },
		unmatch : function() {  }
	})
	.register("only screen and (min-width: 200px) and (max-width: 479px)", {
		match : function() {  },
		unmatch : function() {  }
	});
});