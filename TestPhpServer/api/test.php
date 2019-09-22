<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 22.09.2019
 * Time: 2:50
 */


require_once __DIR__.'/../db/OrderDatabase.php';

$connector = new DatabaseConnector();
$dbConnection = $connector->getConnection();
$orderDb = new OrderDatabase($dbConnection);

$user = "asdf12342r";

echo 'balance';
echo json_encode($orderDb->balance($user));

echo 'all';
echo json_encode($orderDb->findAll($user));

$order = new Order();
$order->ProductId = 1;
$order->Size = new Size();
$order->Size->TotalPrice = 12;
$order->Size->UnitsCount= 120;

echo 'order';
$id =$orderDb->insert($order, $user);
echo json_encode($orderDb->get($id,$user));

echo 'balance';
echo json_encode($orderDb->balance($user));

echo 'all';
echo json_encode($orderDb->findAll($user));