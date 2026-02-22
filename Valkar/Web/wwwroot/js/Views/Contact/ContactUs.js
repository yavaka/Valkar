window.ContactUs = (function () {
    var defaults = {
        googleCaptchaKeyId: '',
        invokedFrom: ''
    };
    var recaptchaLoadPromise = null;

    function loadRecaptchaScript(keyId) {
        if (recaptchaLoadPromise) return recaptchaLoadPromise;
        if (typeof grecaptcha !== 'undefined' && grecaptcha.enterprise) return Promise.resolve();
        recaptchaLoadPromise = new Promise(function (resolve, reject) {
            var s = document.createElement('script');
            s.src = 'https://www.google.com/recaptcha/enterprise.js?render=' + encodeURIComponent(keyId);
            s.async = true;
            s.onload = function () { resolve(); };
            s.onerror = function () { reject(new Error('reCAPTCHA script failed to load')); };
            document.head.appendChild(s);
        });
        return recaptchaLoadPromise;
    }

    return {
        onInit: function (obj) {
            $.extend(defaults, obj);

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

                // When reCAPTCHA key is not configured, submit form directly (e.g. local dev)
                if (!defaults.googleCaptchaKeyId) {
                    $('#contact-form').submit();
                    return;
                }

                // Load reCAPTCHA dynamically (after Permissions API patch in layout) then get token and submit
                loadRecaptchaScript(defaults.googleCaptchaKeyId)
                    .then(function () {
                        if (typeof grecaptcha === 'undefined' || !grecaptcha.enterprise) {
                            throw new Error('reCAPTCHA Enterprise not loaded. Check GoogleCaptchaKeyId and domain. See docs/Google-Service-Account-Setup.md');
                        }
                        return new Promise(function (resolve, reject) {
                            grecaptcha.enterprise.ready(function () {
                                grecaptcha.enterprise.execute(defaults.googleCaptchaKeyId, { action: 'SendContactForm' })
                                    .then(resolve)
                                    .catch(reject);
                            });
                        });
                    })
                    .then(function (token) {
                        $('#captcha-token').val(token);
                        $('#contact-form').submit();
                    })
                    .catch(function (err) {
                        console.error('reCAPTCHA failed:', err);
                        alert('Security check failed. If you are the site owner, use a reCAPTCHA Enterprise key from Google Cloud Console and add this site\'s domain to the key. See docs/Google-Service-Account-Setup.md.');
                    });
            });
        }
    };
})();