<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:15
 */

require_once __DIR__.'/classes/AuthorizationRequest.php';
require_once __DIR__.'/../../services/Auth.php';
require_once __DIR__.'/../../config/LocalID.php';

$request = new AuthorizationRequest();

// Parse arguments
if($_SERVER["REQUEST_METHOD"] == "POST") {
    $request = AuthorizationRequest::Parse($_POST);
}else if($_SERVER["REQUEST_METHOD"] == "GET"){
    $request = AuthorizationRequest::Parse($_GET);
}else{
    die(400);
}

// Assert arguments
if($request->check()) {

    session_start();
    $request->store($_SESSION);
    session_commit();

    Auth::redirect(Auth::localUrl('/api/oidc/connect.php'));
}