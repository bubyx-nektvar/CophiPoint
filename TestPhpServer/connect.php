<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 12:22
 */
$fullUrl = 'mff.cophipoint://connect';
header("Location: ".$fullUrl."?".$_SERVER['QUERY_STRING']);