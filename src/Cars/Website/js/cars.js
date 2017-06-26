$(document).ready(function () {

    var source = $('#cars-template').html();
    var template = Handlebars.compile(source);

    loadPage();

    $('#register').click(function (event) {
        event.preventDefault();
        registerCar($('#carRegistration').val());
    });

    function loadPage() {
        console.log('Sending GET request to API at "/e2e/api/cars".');
        $.get('/e2e/api/cars', function (cars) {
            console.log('Successful GET response from API at "/e2e/api/cars".');
            var html = template(cars);
            $('#cars').replaceWith(html);
            $('#error').hide();
            $('#carRegistration').val('').focus();
            $.each($(':required'), function () {
                $(this).removeClass('touched');
            });
        });
    }

    function registerCar(registration) {
        var data = JSON.stringify({
            registration: registration
        });
        console.log('Sending POST request to API at "/e2e/api/cars" with data: ' + data + '.');
        $.ajax({
            url: '/e2e/api/cars/',
            type: 'POST',
            data: data,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                console.log('Successful POST response from API at "/e2e/api/cars".');
                loadPage();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('Failed POST response from API at "/e2e/api/cars" (' + jqXHR.status + ' ' + errorThrown + ').');
            if (jqXHR.status === 404) {
                $("body").load("/e2e/404.html");
            }
            else if (jqXHR.responseJSON) {
                $('#error').show().html(jqXHR.responseJSON.details);
                $('#carRegistration').focus();
            }
        });
    }

});