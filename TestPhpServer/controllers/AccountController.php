<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:39
 */
require_once __DIR__ .'/../config/Auth.php';

class AccountController
{
    private $user;
    private $method;

    function  __construct($requestMethod)
    {
        $this->method = $requestMethod;
    }

    public  function  process(){
        $this->user = Auth::authorize();
        switch ($this->method) {
            case 'GET':
                $response = $this->getBalance();
                break;
            default:
                http_response_code(404);
                return;
        }

        if(isset($response)){
            header("Content-Type: application/json; charset=UTF-8");
            echo json_encode($response);
        }
    }
    private function  getBalance(){
        //TODO get balance

        $email = $this->user['email'];
        return array(
            "balance" => -100.2,
            "email" => $email
        );
    }

}