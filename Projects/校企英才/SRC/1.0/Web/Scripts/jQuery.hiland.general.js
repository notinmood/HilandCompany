(function ($) {
    /*0.标签的快速对齐*/
    $.fn.quickAlign = function () {
        var max = 0;
        this.each(function () {
            if ($(this).width() > max) {
                max = $(this).width();
            }
        });
        this.width(max);
    };

    /*1.1.此"实例"方法可以指定某(些)ul产生效果*/
    $.fn.effectListItem = function (options) {
        var opts = $.extend({}, $.fn.effectListItem.defaults, options);

        this.each(function () {
            var thisContainer = $(this);
            //添加奇偶行颜色 
            $(thisContainer).find("li:odd").css("backgroundColor", opts.oddColor);
            $(thisContainer).find("li:even").css("backgroundColor", opts.evenColor);
            //添加活动行颜色 
            var originalBackGroundColor;
            $(thisContainer).find("li").bind("mouseover", function () {
                originalBackGroundColor = $(this).css("backgroundColor");
                $(this).css("backgroundColor", opts.overColor);
            });
            $(thisContainer).find("li").bind("mouseout", function () {
                $(this).css("backgroundColor", originalBackGroundColor);
            });
        });
    };

    $.fn.effectListItem.defaults = {
        oddColor: "#C4E1FF",
        evenColor: "#F2F9FD",
        overColor: "#C7C7E2",
        selColor: "#336666"
    };



    /*1.2.此"静态"方法将会在页面上的所有ul上产生效果*/
    $.extend({
        effectListItem: function (options) {
            var opts = $.extend({}, $.fn.effectListItem.defaults, options);
            $.fn.effectListItem.options = opts;

            //奇偶异色
            $("li:odd").css("backgroundColor", opts.oddColor);
            $("li:even").css("backgroundColor", opts.evenColor);

            //MouseOver
            var originalBackGroundColor;
            $("li").mouseover(function () {
                originalBackGroundColor = $(this).css("backgroundColor");
                $(this).css("backgroundColor", opts.overColor);
            });

            //MouseOut
            $("li").mouseout(function () {
                $(this).css("backgroundColor", originalBackGroundColor);
            });
        }
    });

    /*2.1.此"实例"方法可以指定某(些)table产生效果*/
    $.fn.effectRow = function (options) {
        var opts = $.extend({}, $.fn.effectRow.defaults, options);

        this.each(function () {
            var thisTable = $(this);
            //添加奇偶行颜色 
            $(thisTable).find('tr:odd > td').css('backgroundColor', opts.oddColor);
            $(thisTable).find('tr:even > td').css('backgroundColor', opts.evenColor);
            //添加活动行颜色 
            var originalBackGroundColor;
            $(thisTable).find("tr").bind("mouseover", function () {
                originalBackGroundColor = $(this).find('td').css('backgroundColor');
                $(this).find('td').css('backgroundColor', opts.overColor);
            });
            $(thisTable).find("tr").bind("mouseout", function () {
                $(this).find('td').css('backgroundColor', originalBackGroundColor);
            });
        });
    };

    $.fn.effectRow.defaults = {
        oddColor: '#C4E1FF',
        evenColor: '#F2F9FD',
        overColor: '#C7C7E2',
        selColor: '#336666'
    };



    /*2.2.此"静态"方法将会在页面上的所有table上产生效果*/
    $.extend({
        effectRow: function (options) {
            var opts = $.extend({}, $.fn.effectRow.defaults, options);
            $.fn.effectRow.options = opts;

            //奇偶异色
            $('tr:odd > td').css('backgroundColor', opts.oddColor);
            $('tr:even > td').css('backgroundColor', opts.evenColor);

            //MouseOver
            var originalBackGroundColor;
            $("tr").mouseover(function () {
                originalBackGroundColor = $(this).find('td').css('backgroundColor');
                $(this).find('td').css('backgroundColor', opts.overColor);
            });

            //MouseOut
            $("tr").mouseout(function () {
                $(this).find('td').css('backgroundColor', originalBackGroundColor);
            });
        }
    });

    /*3.对Url的各种操作*/
    $.extend({
        UrlUtil: function (url) {


            /*0.展示url信息；参数isAppendRandom表示是否在url的最后附加一个随机参数（通常在Ajax请求页面防止缓存时使用）*/
            this.show = function (isAppendRandom) {
                if (isAppendRandom == true) {
                    this.concat("r", Math.random());
                }

                return url;
            };

            /*1.拼接url参数*/
            this.concat = function (key, value) {
                if (url.indexOf('?') > -1) {
                    url += "&" + key + "=" + encodeURIComponent(value);
                }
                else {
                    url += "?" + key + "=" + encodeURIComponent(value);
                }

                return this;
            };

            return this;
        }
    });

    /*4.以下逻辑获取url中的各个部分*/
    $.extend({
        UrlParser: function (url) {
            var _fields = {
                'Username': 4,
                'Password': 5,
                'Port': 7,
                'Protocol': 2,
                'Host': 6,
                'Pathname': 8,
                'URL': 0,
                'Querystring': 9,
                'Fragment': 10
            };
            var _values = {};
            var _regex = /^((\w+):\/\/)?((\w+):?(\w+)?@)?([^\/\?:]+):?(\d+)?(\/?[^\?#]+)?\??([^#]+)?#?(\w*)/;

            var _initValues = function () {
                for (var f in _fields) {
                    _values[f] = '';
                }
            };

            var _parse = function (url) {
                _initValues();
                var r = _regex.exec(url);
                if (!r) throw "DPURLParser::_parse -> Invalid URL";

                for (var f in _fields) if (typeof r[_fields[f]] != 'undefined') {
                    _values[f] = r[_fields[f]];
                }
            };

            var _makeGetter = function (field) {
                return function () {
                    return _values[field];
                }
            };

            /*向外公开获取url各个部分信息的方法*/
            for (var f in _fields) {
                this['get' + f] = _makeGetter(f);
            }

            if (typeof url != 'undefined') {
                _parse(url);
            }

            return this;
        }
    });

    jQuery.fn.simulateKeyPress = function (keyCode) {
        // 内部调用jQuery.event.trigger  
        // 参数有 (Event, data, elem). 最后一个参数是非常重要的的！  
        jQuery(this).trigger({ type: 'keydown', keyCode: keyCode });
    };

})(jQuery)