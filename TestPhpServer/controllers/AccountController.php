<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:39
 */
require_once __DIR__ .'/../config/Auth.php';
require_once __DIR__.'/../db/OrderDatabase.php';

class AccountController
{
    private $user;
    private $method;
    private $orderDb;

    function  __construct($requestMethod)
    {
        $this->method = $requestMethod;

        $connector = new DatabaseConnector();
        $dbConnection = $connector->getConnection();
        $this->orderDb= new OrderDatabase($dbConnection);
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
        $uid = Auth::getId($this->user);

        if(!isset($this->user['email'])){
            $email= $this->orderDb->getUser($uid)->email;
        }else {
            $email = $this->user['email'];
            $this->orderDb->updateUser($this->user);
        }

        $balance = $this->orderDb->balance($uid);

        return array(
            "balance" => $balance,
            "email" => $email
        );
    }

}