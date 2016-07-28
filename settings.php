<?php

// Application settings
$tenant = "$tenant$";
$clientID = "$clientId$";
$client_secret = "$clientSecret$"; // only needed if Confidential Client Flow
$redirect_uri = urlencode("http://localhost:10266/");

// Decide which authentication flow you would like to follow
$response_type = "id_token"; // either id_token or code, depending on whether your application has enabled/disabled implicit flow
$response_mode = "form_post"; // can also be query_string or fragment, but this code works with form_post
$scope = "openid"; // currently, just openid supported

// Create one or more B2C policies in the Azure Portal.
// This example code can use up to 3 policies -
// 1. a generic sign in or sign up policy (no MFA)
// 2. an optional administrator sign in or sign up policy (with MFA)
// 3. a profile editing policy
$generic_policy = "$genericPolicy$";
$admin_policy = "";
$edit_profile_policy = "";

// List of admins' email addresses. You can also leave this empty.
$admins = array("");

// END OF CONFIGURABLE SETTINGS /////////////////////////////////////////////////////////////////////////////
$metadata_endpoint_begin = 'https://login.microsoftonline.com/'.
                     $tenant.
                     '.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=';


?>