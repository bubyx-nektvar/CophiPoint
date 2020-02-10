<?php

require_once __DIR__ . '/../../vendor/autoload.php';
require_once __DIR__.'/../../services/Auth.php';
require_once __DIR__.'/../../controllers/OIDCController.php';
require_once __DIR__.'/classes/AuthorizationRequest.php';

session_start();
$request = AuthorizationRequest::Parse($_SESSION);

$oidc = new OIDCController();

$result = $oidc->login();
if(isset($result->error)) {
    Auth::AuthenticationErrorResponse(
        $request->redirect_uri,
        $result->error,
        $result->error_description,
        $request->state
    );
}
else{
    Auth::redirect($request->redirect_uri, array(
        'code' => $result->code,
        'state'=> $request->state,
        'nonce'=> $request->nonce,
        'scope'=> $request->scope,
        'iss'=>Auth::localUrl('/')
    ));
}
