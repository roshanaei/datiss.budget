
(function ($) {
    $.fn.datepicker = function (options) {
        var defaults = {
            isRTL: true,
            dateFormat: "yy/m/d",
            showTimer: false
        };

        options = $.extend(defaults, options);
        $(this).persianDatepicker({
            "inline": false,
            "format": (options.showTimer == false ? "YYYY/MM/DD" : "YYYY/MM/DD hh:mm:ss a"),
            "viewMode": "day",
            "initialValue": false,
            "initialValueType": 'persian',
            //"minDate": 1526276213699,
            //"maxDate": 1527226613711,
            "autoClose": true,
            "position": "auto",
            "altFormat": "lll",
            "altField": "#altfieldExample",
            "onlyTimePicker": false,
            "onlySelectOnDate": false,
            "calendarType": "persian",
            "inputDelay": "800",
            "observer": false,
            "calendar": {
                "persian": {
                    "locale": "fa",
                    "showHint": true,
                    "leapYearMode": "algorithmic"
                },
                "gregorian": {
                    "locale": "en",
                    "showHint": false
                }
            },
            "navigator": {
                "enabled": true,
                "scroll": {
                    "enabled": true
                },
                "text": {
                    "btnNextText": "<",
                    "btnPrevText": ">"
                }
            },
            "toolbox": {
                "enabled": true,
                "calendarSwitch": {
                    "enabled": false,
                    "format": "MMMM"
                },
                "todayButton": {
                    "enabled": true,
                    "text": {
                        "fa": "امروز",
                        "en": "Today"
                    }
                },
                "submitButton": {
                    "enabled": true,
                    "text": {
                        "fa": "تایید",
                        "en": "Submit"
                    }
                },
                "text": {
                    "btnToday": "امروز"
                }
            },
            "timePicker": {
                "enabled": options.showTimer,
                "step": "1",
                "hour": {
                    "enabled": true,
                    "step": ""
                },
                "minute": {
                    "enabled": true,
                    "step": null
                },
                "second": {
                    "enabled": true,
                    "step": null
                },
                "meridian": {
                    "enabled": true
                }
            },
            "dayPicker": {
                "enabled": true,
                "titleFormat": "YYYY MMMM"
            },
            "monthPicker": {
                "enabled": true,
                "titleFormat": "YYYY"
            },
            "yearPicker": {
                "enabled": true,
                "titleFormat": "YYYY"
            },
            "responsive": false
        });

        return this;
    }
}(jQuery))