<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:48
 */
require_once __DIR__ .'/../config/Auth.php';
require_once __DIR__.'/../db/OrderDatabase.php';

class OrdersController
{
    private $user;
    private $uid;
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
        $this->uid = Auth::getId($this->user);
        switch ($this->method) {
            case 'GET':
                $response = $this->getOrders();
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

    private function getOrders(){
        return $this->orderDb->findAll($this->uid );
    }

    private function addOrder(){
        /** @var Order $input*/
        $input = json_decode(file_get_contents('php://input'), FALSE);

        $order = new Order();
        $order->parse($input);

        $newId = $this->orderDb->insert($order, $this->uid);

        return $this->orderDb->get($newId,$this->uid);
    }

}