$(function (){
    $('#shadow-navbar-container').addClass("shadow-xl");
    $('#search-container').removeClass('collapse').css('position', 'relative');
    $('#header').css('box-shadow', '0 1rem 3rem rgba(0,0,0,.2)');
    $('#search-collapse-button').removeAttr('data-toggle').attr('href', "");
});