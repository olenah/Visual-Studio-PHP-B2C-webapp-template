<?php

// Turn on error reporting, for debugging
error_reporting(E_ALL);

// Returns true if the user is logged in
function checkUserLoggedIn() {
	return isset($_COOKIE['user']);
}

// Returns true if the user is an admin
function checkUserIsAdmin() {
	require "settings.php";
	if (!isset($_COOKIE['email'])) return false;
	if (!in_array($_COOKIE['email'], $admins)) return false;
	return true;
}

// Returns the given name of the current user
// Returns an empty string if the user is not authenticated
function getGivenName() {
	if (!checkUserLoggedIn()) return "";
	return $_COOKIE['user'];
}

// Login route
if ($_SERVER['REQUEST_URI'] == "/login") {

    // Get login URL for generic login policy
    require_once "EndpointHandler.php";
    $endpoint_handler = new EndpointHandler($generic_policy);
    $authorization_endpoint = $endpoint_handler->getAuthorizationEndpoint()."&state=generic";

    // Redirect to sign up/sign in page
    header('Location: '.$authorization_endpoint);
}

// Logout route
if ($_SERVER['REQUEST_URI'] == "/logout") {

    // Delete cookies
    setcookie("user", "", time() - 3600);
    setcookie("email", "", time() - 3600);

    // Get logout URL
    require_once "EndpointHandler.php";
    $endpoint_handler = new EndpointHandler($generic_policy);
    $end_session_endpoint = $endpoint_handler->getEndSessionEndpoint();

    // Redirect to B2C logout
    header('Location: '.$end_session_endpoint);
}

// Homepage route
if (checkUserLoggedIn()) {

    $given_name = getGivenName();
    $user_logged_in = true;
    require_once "Views/index.html";
}
else if (isset($_POST['id_token']) || isset($_POST['code'])) { // Id Token just posted back by B2C

    require_once 'settings.php';
    require_once "EndpointHandler.php";
    require_once 'TokenChecker.php';

    // Check which authorization policy was used
    $action = $_POST['state'];
    if ($action == "generic") $policy = $generic_policy;
    if ($action == "admin") $policy = $admin_policy;
    if ($action == "edit_profile") $policy = $edit_profile_policy;

    // Check the response type
	if (isset($_POST['code'])) {
		$resp = $_POST['code'];
		$resp_type = "code";
	}
	else if (isset($_POST['id_token'])) {
		$resp = $_POST['id_token'];
		$resp_type = "id_token";
	}

    // Verify token
    $tokenChecker = new TokenChecker($resp, $resp_type, $clientID, $client_secret, $policy);
    $verified = $tokenChecker->authenticate();
    if ($verified == false) echo "Failed to successfully verify ID Token <br>";

    // Fetch user's email and check if admin
	$email = $tokenChecker->getClaim("emails")[0];
	if (in_array($email, $admins)) {
		$acr = $tokenChecker->getClaim("acr");

		// If user did not authenticate with admin_policy, redirect to admin policy
		if ($action == "generic") {
			$endpoint_handler = new EndpointHandler($admin_policy);
			$authorization_endpoint = $endpoint_handler->getAuthorizationEndpoint()."&state=admin";

			// Redirect to sign up/sign in page for admins
			header('Location: '.$authorization_endpoint);
		}
	}

	// Set cookies
	setcookie("email", $email);
	$given_name = $tokenChecker->getClaim("given_name");
	setcookie("user", $given_name);

    // Render view
    $user_logged_in = true;
    require_once "Views/index.html";
}
else { // User not authenticated

    $user_logged_in = false;
    require_once "Views/index.html";
}

?>




