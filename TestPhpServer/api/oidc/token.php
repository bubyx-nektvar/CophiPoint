<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:15
 */

require_once  __DIR__.'/../../services/headers.php';
require_once  __DIR__.'/../../services/Auth.php';
require_once __DIR__.'/../../controllers/OIDCController.php';

$oidc = new OIDCController();
if(Auth::authorizeWithClient()){
    $response = array();
    if($_POST['grant_type']=='authorization_code'){
        $response = $oidc->createTokensWithCode($_POST['code']);

        header('Content-Type: application/json');
        echo json_encode($response);
    }
    elseif($_POST['grant_type'] == 'refresh_token'){
        $response = $oidc->createTokensWithRefresh($_POST['refresh_token']);

        header('Content-Type: application/json');
        echo json_encode($response);
    }else{
        Auth::TokenErrorResponse('unsupported_grant_type', 'Token exchange failed due to grant type');
    }
}else{
    Auth::TokenErrorResponse('invalid_client','Token exchange failed due to client authorization');
}