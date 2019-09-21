<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:15
 */
require_once __DIR__ .'/../controllers/AccountController.php';

$requestMethod = $_SERVER["REQUEST_METHOD"];

$c  = new AccountController($requestMethod);
$c->process();