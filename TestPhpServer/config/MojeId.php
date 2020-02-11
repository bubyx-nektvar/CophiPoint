<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 21.09.2019
 * Time: 18:23
 */

class MojeId
{
    //bubyx
    const ClientId = 'Ux9KtBJLZVIn';
    const ClientSecret = '8e683dfabd834846ac00d5c99d9be55271d15e68d89c6089af8ec459';
    //192.168.1.168 (localhost)
//    const ClientId = 'vK3UM0RApi3y';
//    const ClientSecret = 'f265127d430a1b0fc697a592137882bf7660764426e3fcff54e5cae0';

    const ProviderUrl = 'https://mojeid.regtest.nic.cz';
    const Scopes = "email openid";
    const Claims = array(
        "userinfo"=> array(
            "email"=> array(
                "essential"=>true
            ),
            "name"=> null
        )
    );
}