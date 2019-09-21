<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:25
 */
require_once __DIR__ .'/MojeId.php';
require_once __DIR__ .'/headers.php';

class Auth
{
    private static function  die_unauthorize(){
        var_dump(http_response_code(401));//TODO not working
        exit('unauthorized');
    }
    public static function authorize()
    {
        $headers = getallheaders();
        if(!isset($headers['Authorization'])) {
            Auth::die_unauthorize();
        }
        $auth_header = $headers['Authorization'];

        $ch = curl_init();

        curl_setopt($ch, CURLOPT_URL, MojeId::UserInfoEndpoint);
        curl_setopt($ch, CURLOPT_HEADER, TRUE);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, TRUE);
        curl_setopt($ch, CURLOPT_HTTPHEADER, [
            "Authorization: ".$auth_header
        ]);


        $response = curl_exec($ch);
        $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
        if($httpCode ==401){
            Auth::die_unauthorize();
        }else if($httpCode != 200){
            var_dump($httpCode);
            var_dump($response);
            var_dump($auth_header);
            var_dump($headers);
            exit($httpCode);
        }

        $header_size = curl_getinfo($ch,CURLINFO_HEADER_SIZE);
        $result['header'] = substr($response, 0, $header_size);
        $response = substr( $response, $header_size );
        //var_dump($response);

        $userInfo = json_decode($response, true);

        curl_close($ch);

        return $userInfo;
    }
    public static function getId($userInfo)
    {
        return $userInfo['sub'];
    }
}