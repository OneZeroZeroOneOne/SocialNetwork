(function () {


    function getJwt(loginData, success, error, complete) {
        $.ajax({
            url: loginData.path,
            type: 'GET',
            data: {
                //grant_type: 'password',
                email: loginData.user,
                password: loginData.pass

            },
            success: function (data) {
                success && success(data.access_token);
            },
            error: function (jqXhr, err, msg) {
                error && error(JSON.parse(jqXhr.responseText).error_description);

            },
            complete: complete
        });
    }

    function setJwt(key) {
        /*console.log(key);
        //swaggerUi.api.clientAuthorizations.authz = {};
        //swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization", "Bearer " + key, "header"));
        var apiKeyAuth = new window.SwaggerClient.ApiKeyAuthorization("Authorization", key, "header");
        window.swaggerUi.api.clientAuthorizations.add("bearer", apiKeyAuth);*/

        const constantMock = window.fetch;
        window.fetch = function () {
            // Get the parameter in arguments
            // Intercept the parameter here 
            console.log(arguments);
            console.log(arguments[1].headers);
            arguments[1].headers["Authorization"] = key;
            console.log(arguments[1].headers);
            var origin = window.location.origin;
            console.log(origin);
            var pathname = window.location.pathname;
            console.log(pathname);
            pathname = pathname.replace("/swagger/index.html", "");
            console.log(pathname);
            var new_origin = origin + pathname;
            arguments[1].url = arguments[1].url.replace(origin, new_origin);

            return constantMock.apply(this, arguments);
        }

    }


    $(function () {


        (function () {
            var styles = `
                <style>
                #sa-setting{
                    display: inline-block !important;
                    position: absolute;
                    background: #89BF04;
                    right: 20px;
                    top: 48px;
                    padding: 10px;
                    box-shadow: 0 2px 1px rgba(0,0,0,0.5);
                    display: none;
                }
                 .sa-btn{
                    display: inline-block !important;
                    color: #FFF;
                    font-weight: bold;
                    background: #547F00;
                    border-radius: 3px;
                    padding: 6px 8px;
                    font-family: "Droid Sans", sans-serif;
                    font-size: 0.9em;
                    cursor: pointer;
                }
                #sa-setting input{
                    margin-bottom: 5px;
                    padding: 3px;
                    border: 2px inset;
                }
            </style>
            `;
            $('head').append(styles);
            var settingTemplate = `
                <div id="sa-setting">
                    <input id="sa-path" placeholder="Path" value="/Token">
                    <br>
                    <input id="sa-username" placeholder="Username">
                    <br>
                    <input id="sa-password" type="password" placeholder="Password">
                    <br>
                    <span id="sa-btn-login" class="sa-btn">Login</span>
                    <span id="sa-btn-logout" class="sa-btn" style="display: none">Logout</span>
                </div>
            `;
            $('body').append(settingTemplate);



            $('<div id="sa-btn-setting" class="sa-btn">Login</div>')
                .click(function () {
                    $('#sa-setting').fadeToggle();
                })
                .appendTo('#api_selector');

        })();


        function login(loginData) {
            $('#sa-btn-setting').text('Working...');
            getJwt(loginData,
                function (jwt) {
                    $('#sa-btn-setting').text('Hi ' + loginData.user).css('background', '#0f6ab4');
                    $('#sa-setting').fadeOut();
                    $('#sa-btn-logout').fadeIn();
                    setJwt(jwt);
                },
                function (err) {
                    $('#sa-btn-setting').text('Failed').css('background', '#a41e22');

                    setJwt('');
                    alert(err);
                }, function () {
                    // $('#sa-btn-setting').text('Login');

                });
        }

        $('#sa-btn-logout').click(function () {
            setJwt('');
            window.localStorage.removeItem('sa-login-data');
            $('#sa-username').val('');
            $('#sa-password').val('');
            $(this).fadeOut();
        });

        $('#sa-btn-login').click(function () {


            var loginData = {
                path: $('#sa-path').val(),
                user: $('#sa-username').val(),
                pass: $('#sa-password').val()
            };
            window.localStorage.setItem('sa-login-data', JSON.stringify(loginData));

            login(loginData);


        });


        //Auto login
        (function () {
            var oldLoginData = window.localStorage.getItem('sa-login-data');
            if (oldLoginData) {
                oldLoginData = JSON.parse(oldLoginData);
                $('#sa-path').val(oldLoginData.path);
                $('#sa-username').val(oldLoginData.user);
                $('#sa-password').val(oldLoginData.pass);
                login(oldLoginData);
            }

        })();

    });
})();