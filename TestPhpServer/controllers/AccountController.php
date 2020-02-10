<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:39
 */
require_once __DIR__ . '/../services/Auth.php';
require_once __DIR__.'/../db/OrderDatabase.php';
require_once __DIR__.'/../db/UserDatabase.php';
require_once __DIR__.'/OIDCController.php';

class AccountController
{
    /**
     * @var TokenInfo
     */
    private $tokenInfo;
    private $method;

    private $orderDb;
    private $userDb;
    private $oidc;

    function  __construct($requestMethod)
    {
        $this->method = $requestMethod;

        $connector = DatabaseConnector::create();
        $dbConnection = $connector->getConnection();
        $this->orderDb= new OrderDatabase($dbConnection);
        $this->userDb = new UserDatabase($dbConnection);
        $this->oidc = new OIDCController($connector);
    }

    public  function  process(){
        $this->tokenInfo = $this->oidc->authorize();
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
        $uid = $this->tokenInfo->getUserId();

        $userInfo = $this->userDb->getUserById($uid);
        $balance = $this->orderDb->balance($uid);

        return array(
            "balance" => $balance,
            "email" => $userInfo['email']
        );
    }

}