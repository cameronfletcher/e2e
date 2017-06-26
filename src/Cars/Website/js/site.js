$(document).ready(function () {
    if (window.location.href.endsWith('/')) {
        window.history.replaceState('e2e', 'e2e', window.location.href.slice(0, -1));
        console.log('Removed trailing slash from URL');
    }

    $(':required').one('blur keydown', function () {
        console.log('A required field has been touched.', this);
        $(this).addClass('touched');
    });
});