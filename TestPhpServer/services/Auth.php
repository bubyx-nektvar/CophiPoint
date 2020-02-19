<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:25
 */
require_once __DIR__ . '/../config/MojeId.php';
require_once __DIR__ . '/headers.php';
require_once __DIR__.'/../db/TokenDatabase.php';

class Auth
{
    public static function  die_unauthorize(){
        http_response_code(401);
        exit('unauthorized');
    }

    public static function authorizeWithClient(){
        if (
            $_SERVER["PHP_AUTH_USER"] == LocalID::ClientID
            && $_SERVER["PHP_AUTH_PW"] == LocalID::ClientSecret
        ){
            return true;
        }else{
            return false;
        }
    }

    /**
     * @param $path string path relative to localhost (start with '/')
     * @return string
     */
    public static function localUrl($path){
        return (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] ? 'https':'http').'://'.$_SERVER['HTTP_HOST'].$path;
    }
    public static function redirect($redirectUri, array $query = array()){
        $q = '';
        if(!empty($query)){
            $q = '?'.http_build_query($query);
        }
        header("Location: ".$redirectUri.$q);
    }

    public static function AuthenticationErrorResponse(string $redirect_uri,string $error,string $error_description,string $state){
        Auth::redirect($redirect_uri,array(
            'error' => $error,
            'error_description' => $error_description,
            'state'=>$state
        ));
    }
    public static function TokenErrorResponse(string $error, string  $errorDecription = null){

        http_response_code(400);
        header('Content-Type: application/json');
        echo json_encode(array(
            'error'=>$error,
            'error_description'=>$errorDecription
        ));
        exit();
    }
}