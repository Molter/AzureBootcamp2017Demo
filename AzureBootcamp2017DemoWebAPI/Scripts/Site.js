var isPoolingEnabled = false;

$().ready(function () {
    

    // Closes the sidebar menu
    $("#menu-close").click(function (e) {
        e.preventDefault();
        $("#sidebar-wrapper").toggleClass("active");
    });
    // Opens the sidebar menu
    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#sidebar-wrapper").toggleClass("active");
    });
    // Scrolls to the selected menu item on the page
    $(function () {
        $('a[href*=#]:not([href=#],[data-toggle],[data-target],[data-slide])').click(function () {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') || location.hostname == this.hostname) {
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top
                    }, 1000);
                    return false;
                }
            }
        });
    });
    //#to-top button appears after scrolling
    var fixed = false;
    $(document).scroll(function () {
        if ($(this).scrollTop() > 250) {
            if (!fixed) {
                fixed = true;
                // $('#to-top').css({position:'fixed', display:'block'});
                $('#to-top').show("slow", function () {
                    $('#to-top').css({
                        position: 'fixed',
                        display: 'block'
                    });
                });
            }
        } else {
            if (fixed) {
                fixed = false;
                $('#to-top').hide("slow", function () {
                    $('#to-top').css({
                        display: 'none'
                    });
                });
            }
        }
    });
    // Disable Google Maps scrolling
    // See http://stackoverflow.com/a/25904582/1607849
    // Disable scroll zooming and bind back the click event
    var onMapMouseleaveHandler = function (event) {
        var that = $(this);
        that.on('click', onMapClickHandler);
        that.off('mouseleave', onMapMouseleaveHandler);
        that.find('iframe').css("pointer-events", "none");
    }
    var onMapClickHandler = function (event) {
        var that = $(this);
        // Disable the click handler until the user leaves the map area
        that.off('click', onMapClickHandler);
        // Enable scrolling zoom
        that.find('iframe').css("pointer-events", "auto");
        // Handle the mouse leave event
        that.on('mouseleave', onMapMouseleaveHandler);
    }
    // Enable map zooming with mouse scroll when the user clicks the map
    $('.map').on('click', onMapClickHandler);

    function StartPolling() {

        if (isPoolingEnabled == true) {
            window.setTimeout(function () {
                $.ajax({
                    url: "home/getCPU",
                    type: "POST",
                    success: function (result) {
                        console.log(result);

                        //var inf = JSON.parse(result);
                        $('#availableMB').html(result.RAM);
                        $('#availableCPU').html(result.CPU);
                        //SUCCESS LOGIC
                        StartPolling();
                    },
                    error: function () {
                        //ERROR HANDLING
                        StartPolling();
                    }
                });
            }, 1000);
        }

    }


    $('#startMonitoring').on('click', function () {

        if (isPoolingEnabled == true) {
            $('#startMonitoring').html('Start Monitoring.');
            isPoolingEnabled = false;

        } else {
            $('#startMonitoring').html('Stop Monitoring.');
            isPoolingEnabled = true;
            StartPolling();
            
        }

    });
    $('#increaseCPU').on('click', function () {
        $.ajax({
            url: "home/increaseCPU",
            type: "POST",
            success: function (result) {
                if (result)
                {
                    console.log(result);
                }
            },
            error: function () {
                //ERROR HANDLING
            }
        });

    });
    $('#resetCPU').on('click', function () {

        $.ajax({
            url: "home/resetCPU",
            type: "POST",
            success: function (result) {
                if (result) {
                    console.log(result);
                }
            },
            error: function () {
                //ERROR HANDLING
            }
        });

    });
    
});