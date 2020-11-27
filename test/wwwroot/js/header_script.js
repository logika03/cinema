$(document).ready(function (){
    $('#login-form').submit(function (event){
        event.preventDefault();
        $('#loginForm-submit').prop("disabled", true);
        $.post(window.location.origin + '/login', $('#login-form').serialize())
            .done(function (){
                document.location.reload();
            })
            .fail(function (){
                $('#login-alert').removeClass('d-none');
                $('#loginForm-submit').prop("disabled", false);
            })
    });
    
    $('div').on('click', '[href]', function (event){
        let $item = $(this);
        
        if(!$(event.target).hasClass('badge-outline-white'))
            window.location = $item.attr("href");
    });
    
    $('#search-collapse-button[data-toggle="collapse"]').click(function (){
        let target = '#' + $(this).attr('data-target');
        $(target).collapse('toggle');
    })
    
    $('#search-container').on('show.bs.collapse', function (){
        $('#shadow-navbar-container').addClass("shadow-xl");
    });

    $('#search-container').on('hide.bs.collapse', function (){
        $('#shadow-navbar-container').removeClass("shadow-xl");
    });
});
