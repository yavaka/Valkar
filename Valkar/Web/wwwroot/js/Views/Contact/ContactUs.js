window.ContactUs = (function () {
    var defaults = {
        googleCaptchaKeyId: '',
        invokedFrom: ''
    };

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);

            debugger;

            if (defaults.invokedFrom == 'ContactUsPage') {
                $('#btnContactNavbar').addClass('active');
                $('#btnHomeNavbar').removeClass('active');
                $('#btnAboutNavbar').removeClass('active');
            }
            else if (defaults.invokedFrom == 'HomePage') {
                $('#btnHomeNavbar').addClass('active');
                $('#btnAboutNavbar').removeClass('active');
                $('#btnContactNavbar').removeClass('active');
            }
            
            $('#submit-btn').on('click', function (e) {
                e.preventDefault();

                // generate captcha token on contact form submission
                grecaptcha.enterprise.ready(function () {
                    grecaptcha.enterprise.execute(defaults.googleCaptchaKeyId, { action: 'SendContactForm' })
                        .then(function (token) {
                            $('#captcha-token').val(token);
                            $('#contact-form').submit();
                        }
                    );
                });
            });
        }
    };
})();