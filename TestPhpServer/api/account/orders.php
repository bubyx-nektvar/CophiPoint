<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 19:55
 */
require_once __DIR__.'/../../controllers/OrdersController.php';

$requestMethod = $_SERVER["REQUEST_METHOD"];

$c  = new OrdersController($requestMethod);
$c->process();