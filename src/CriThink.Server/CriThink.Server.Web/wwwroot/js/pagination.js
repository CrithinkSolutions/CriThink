$(function () {
    let pageSize = parseInt(GetParameterValues('PageSize'));
    let pageIndex = parseInt(GetParameterValues('PageIndex'));

    if (isNaN(pageSize) || isNaN(pageIndex) || pageIndex < 0 || pageSize < 1) {
        pageSize = 20;
        pageIndex = 0;
    }

    let url = window.location.pathname.concat('?PageSize=' + pageSize + '&PageIndex=');
    $('#nextPage').children().attr('href', url + (pageIndex + 1));
    $('#prevPage').children().attr('href', url + (pageIndex - 1));

    $('#currentPage').html(pageIndex + 1);

    if (pageIndex == 0) {
        $('#prevPage').children().removeAttr('href');
        $('#prevPage').toggleClass('disabled');
    }

    if ($('#hasNextPage').text() == 'False') {
        $('#nextPage').children().removeAttr('href');
        $('#nextPage').toggleClass('disabled');
    }

    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    };
});