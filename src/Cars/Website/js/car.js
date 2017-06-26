$(document).ready(function () {

    var host = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
    var urlTemplate = earl(host + '/e2e/cars/{registration}');
    var params = urlTemplate.extract(window.location.href);

    loadPage(params.registration);

    $('#record').click(function (event) {
        event.preventDefault();
        updateDistance(params.registration, $('#carDistance').val());
    });

    $('#scrap').click(function (event) {
        event.preventDefault();
        scrapCar(params.registration);
    });

    function loadPage(registration) {
        console.log('Sending GET request to API at "/e2e/api/cars/' + params.registration + '.');
        $.get('/e2e/api/cars/' + registration, function (car) {
            console.log('Successful GET response from API at "/e2e/api/cars/' + registration + '".');
            $('#registration').html(car.registration);
            $('#distance').html(car.total_distance_travelled);
            $('#error').hide();
            $('#carDistance').val('').focus();
            $.each($(':required'), function () {
                $(this).removeClass('touched');
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failed GET response from API at "/e2e/api/cars/' + registration + '" (' + jqXHR.status + ' ' + errorThrown + ').');
            if (jqXHR.status === 404) {
                $("body").load("/e2e/404.html");
            }
        });
    }

    function updateDistance(registration, distance) {
        var data = JSON.stringify({
            distance_travelled: distance
        });
        console.log('Sending PATCH request to API at "/e2e/api/cars/' + registration + '" with data: ' + data + '.');
        $.ajax({
            url: '/e2e/api/cars/' + registration,
            type: 'PATCH',
            data: data,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                console.log('Successful PATCH response from API at "/e2e/api/cars/' + registration + '".');
                loadPage(registration);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failed PATCH response from API at "/e2e/api/cars/' + registration + '" (' + jqXHR.status + ' ' + errorThrown + ').');
            if (jqXHR.status === 404) {
                $("body").load("/e2e/404.html");
            }
            else if (jqXHR.responseJSON) {
                $('#error').show().html(jqXHR.responseJSON.details);
                $('#carDistance').focus().get(0).setCustomValidity(jqXHR.responseJSON.details);
            }
        });
    }

    function scrapCar(registration) {
        console.log('Sending DELETE request to API at "/e2e/api/cars/' + registration + '".');
        $.ajax({
            url: '/e2e/api/cars/' + registration,
            type: 'DELETE',
            success: function (data) {
                console.log('Successful DELETE response from API at "/e2e/api/cars/' + registration + '".');
                window.location.href = host + '/e2e/cars';
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failed DELETE response from API at "/e2e/api/cars/' + registration + '" (' + jqXHR.status + ' ' + errorThrown + ').');
            if (jqXHR.status === 404) {
                $("body").load("/e2e/404.html");
            }
            else if (jqXHR.responseJSON) {
                $('#error').show().html(jqXHR.responseJSON.details);
                $('#carDistance').focus();
            }
        });
    }

});

