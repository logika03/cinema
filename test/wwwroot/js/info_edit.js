$(function () {
    $(document).on('input', '[limit]', function () {
        let $item = $(this),
            value = $item.val(),
            limit = Number($item.attr("limit"));

        if(value.length > limit)
            $item.val(value.substr(0, limit));
    });
    
    let $validate = $('.validate');

    $validate.validate({
            rules: {
                name: "required",
                surname: "required",
                login: "required",
                email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                name: "Это поле является обязательным",
                surname: "Это поле является обязательным",
                login: "Это поле является обязательным",
                email: {
                    required: "Это поле является обязательным",
                    email: "Email должен быть в формате name@domain.com"
                }
            },
            errorPlacement: function(error, element){
                error.appendTo($(element).parent());
            }
        }
    );

    if($validate.hasClass('validate-immediately'))
        $validate.valid();
});