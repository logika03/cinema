$(function () {
    $(document).on('input', '[limit]', function () {
        let $item = $(this),
            value = $item.val(),
            limit = Number($item.attr("limit"));

        if(value.length > limit)
            $item.val(value.substr(0, limit));
    });

    $.validator.addMethod("symbolCheck", function(value) {
        return /^[A-Za-z0-9\d!\-@._*]*$/.test(value);
    });

    $.validator.addMethod("hasDiffCase", function(value) {
        return /[a-z]/.test(value) && /[A-Z]/.test(value);
    });

    $.validator.addMethod("hasDigit", function(value) {
        return /\d/.test(value);
    });

    let $validate = $('.validate');

    $validate.validate({
            rules: {
                name: "required",
                surname: "required",
                login: "required",
                terms: "required",
                password:{
                    required: true,
                    symbolCheck: true,
                    hasDiffCase: true,
                    hasDigit: true,
                    minlength: 4,
                    maxlength: 32
                },
                confirmationPassword: {
                    equalTo: "#registerPassword"
                },
                email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                name: "Это поле является обязательным",
                surname: "Это поле является обязательным",
                login: "Это поле является обязательным",
                terms: "",
                email: {
                    required: "Это поле является обязательным",
                    email: "Email должен быть в формате name@domain.com"
                },
                password: {
                    required: "Это поле является обязательным",
                    minlength: "Слишком короткий пароль!",
                    maxlength: "Слишком длинный пароль!",
                    symbolCheck: "Пароль должен содержать только символы латинского алфавита, цифры и символы @ * _ - . !",
                    hasDiffCase: "Пароль должен содержать символы верхнего и нижнего регистров",
                    hasDigit: "Пароль должен содержать хотя бы одну цифру"
                },
                confirmationPassword: {
                    equalTo: "Пароли не совпадают"
                }
            },
            errorPlacement: function(error, element){
                if($(element).hasClass("terms"))
                    error.appendTo($('#alert'));
                else
                    error.appendTo($(element).parent().parent());
            },
            highlight: function (element, errorClass, validClass) {
                if($(element).hasClass("terms")) {
                    $('#termsError').removeClass('d-none');
                    $('#alert').removeClass('d-none');
                }
                else {

                    let input_group = $(element).parent();
                    let addon = input_group.find('.addon-dark-transparent');

                    input_group.addClass(errorClass);
                    $(element).addClass(errorClass);
                    addon.addClass(errorClass);
                }
            },
            unhighlight: function(element, errorClass, validClass) {
                if($(element).hasClass("terms")) {
                    $('#termsError').addClass('d-none');
                    if(IsNoAlerts())
                        $('#alert').addClass('d-none');
                }
                else {
                    let input_group = $(element).parent();
                    let addon = input_group.find('.addon-dark-transparent');

                    input_group.removeClass(errorClass);
                    $(element).removeClass(errorClass);
                    addon.removeClass(errorClass);
                }
            }
        }
    );

    if($validate.hasClass('validate-immediately'))
        $validate.valid();
});

function IsNoAlerts(){
    return !$('#alert').children().is('.d-none');
}