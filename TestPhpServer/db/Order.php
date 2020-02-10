<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 22.09.2019
 * Time: 2:18
 */

class Size{
    public $UnitsCount;
    public $TotalPrice;
}

class Order
{
    public $ProductId;
    public $Size;

    public function parse($obj){
        $this->ProductId = $obj->ProductId;
        $this->Size = new Size();
        $this->Size->UnitsCount = $obj->Size->UnitsCount ;
        $this->Size->TotalPrice = $obj->Size->TotalPrice;
    }
}