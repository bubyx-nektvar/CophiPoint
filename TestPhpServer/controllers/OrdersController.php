<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:48
 */
require_once __DIR__ .'/../config/Auth.php';

class OrdersController
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
        return [];
    }

    private function addOrder(){
        $input = (array) json_decode(file_get_contents('php://input'), TRUE);
        return $input;
    }

}