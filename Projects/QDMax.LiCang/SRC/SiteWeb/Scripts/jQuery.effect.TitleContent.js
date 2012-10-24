(function ($) {
    $.fn.effectTitleContent = $.effectTitleContent = function () {
        //$(".contentContainer").hide();
        $(".contentTitle").toggle(
            function () { $(this).next().slideUp(); },
            function () { $(this).next().slideDown(); }
        );
    };
})(jQuery);