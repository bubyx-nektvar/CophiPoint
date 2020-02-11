<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:48
 */
require_once __DIR__ . '/../services/Auth.php';
require_once __DIR__.'/../db/OrderDatabase.php';
require_once __DIR__.'/OIDCController.php';

class OrdersController
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

        $this->userDb = new UserDatabase($dbConnection);
        $this->orderDb= new OrderDatabase($dbConnection);
        $this->oidc = new OIDCController($connector);
    }

    public function process(){
        $this->tokenInfo = $this->oidc->authorize();

        switch ($this->method) {
            case 'GET':
                $response = $this->getUserInfo();
                break;
            case 'POST':
                $response = $this->addOrder();
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
    private function  getUserInfo(){
        $uid = $this->tokenInfo->getUserId();

        $userInfo = $this->userDb->getUserById($uid);
        $balance = $this->orderDb->balance($uid);
        $orders = $this->orderDb->findAll($uid);

        return array(
            "balance" => $balance,
            "email" => $userInfo['email'],
            "orders" => $orders,
            "dataVersion" => '1'
        );
    }

    private function addOrder(){
        if(true && http_response_code(409))
        {//test
            $result = new Order();
            $result->ProductId = 1;
            $result->Size = new Size();
            $result->Size->UnitsCount =1;
            $result->Size->TotalPrice = 1000;
            return $result;
        }

        /** @var Order $input*/
        $input = json_decode(file_get_contents('php://input'), FALSE);

        $order = new Order();
        $order->parse($input);

        $uid = $this->tokenInfo->getUserId();

        $newId = $this->orderDb->insert($order, $uid);

        return $this->orderDb->get($newId,$uid);
    }

}