<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:15
 */

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
if($request->clientId != LocalID::ClientID){
    die(400);
}
// check allowed scopes
if(count(array_diff($request->scope, array_intersect($request->scope,LocalID::Scopes))) != 0){
    die(400);
}

// Store arguments
session_start();
$request->store($_SESSION);
header("Location: ".($_SERVER['HTTPS']?'https':'http').'://'.$_SERVER['HTTP_HOST'].'/api/oidc/connect.php');