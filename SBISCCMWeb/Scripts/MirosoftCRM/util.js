var user, authContext, errorMessage;
var organizationURI = ""; // TODO: Add your organizationURI // http://praansh.crm.dynamics.com/
var tenant = "";
//var organizationURI = "https://praansh.crm.dynamics.com"; // TODO: Add your organizationURI // http://praansh.crm.dynamics.com/
var pageData = [];
(function () {
         // TODO: add your tenant // 32744126-3175-4ceb-9c3f-83eea3009124
    var clientId = "e262f9a6-3221-4eb8-a86a-a57aa7a77687"; // TODO: Add your Client Id
    var pageUrl = "http://localhost:9092/Data/SyncCRMData";// TODO: Add your Reply URL

    var endpoints = {
        orgUri: organizationURI
    };

    window.config = {
        tenant: tenant,
        clientId: clientId,
        postLogoutRedirectUri: pageUrl,
        endpoints: endpoints,
        popUp: true
    };
    authContext = new AuthenticationContext(config);
    authenticate();
    document.getElementById('btnMicrosoftCRMSubmit').addEventListener('click', function () {
        tenant = $("#TenantId").val();
        organizationURI = $("#OrganizationUrl").val();
        var EnvironmentName = $("#EnvironmentName").val();
        if (EnvironmentName == "") {
            $("#spnEnvironmentName").show();
            return false;
        }
        else
        {
            $("#spnEnvironmentName").hide();
        }

        var endpoints = {
            orgUri: organizationURI
        };

        window.config = {
            tenant: tenant,
            clientId: clientId,
            postLogoutRedirectUri: pageUrl,
            endpoints: endpoints,
            popUp: true
        };
        authContext = new AuthenticationContext(config);
        authenticate();
        login();
    })
})();



function authenticate() {
        var isCallback = authContext.isCallback(window.location.hash);
    if (isCallback) {
        authContext.handleWindowCallback();
    }
    var loginError = authContext.getLoginError();

    if (isCallback && !loginError) {
        window.location = authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST);
    }
    if (authContext._loginInProgress == true) {
        return;
    }
    user = authContext.getCachedUser();
    var token = authContext.getCachedToken(organizationURI);
    var hasToken = true;
    if (authContext._getItem(authContext.CONSTANTS.STORAGE.EXPIRATION_KEY + organizationURI) == 0 ||
        authContext._getItem(authContext.CONSTANTS.STORAGE.RENEW_STATUS + window.config.clientId) == authContext.CONSTANTS.TOKEN_RENEW_STATUS_COMPLETED) {
        authContext.acquireToken(organizationURI,
            function (error, token) {
                if (!token) {
                    console.warn("Cannot find token");
                    hasToken = false;
                    return false;
                }
                if (isCallback) {
                    authContext.handleWindowCallback();
                }
            });
    }
    if (hasToken == false)
        return;


    console.log(token)
    if (user && token != null) {
        var req = new XMLHttpRequest()
        req.open("GET", encodeURI(organizationURI + "/api/data/v8.2/accounts?$select=name,accountnumber,address1_city,address1_country,address1_name,address1_line1,address1_stateorprovince,address1_line2,address2_line1,emailaddress1,address1_telephone2&$top=2&$orderby=modifiedon desc"), true);
        req.onreadystatechange = function () {

            if (req.readyState == 4 && req.status == 200) {
                var response = JSON.parse(req.responseText);
                $.ajax({
                    type: "POST",
                    url: "/Data/setCRMDataTable",
                    data: JSON.stringify({ json: req.responseText }),
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        parent.CloseImportPanel();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        };
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Authorization", "Bearer " + token);
        req.send();
    }
}
function login() {
    authContext.login();
}


