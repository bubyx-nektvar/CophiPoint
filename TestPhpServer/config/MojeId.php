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
    const ClientId = 'TODO'; //TODO customize
    const ClientSecret = 'TODO'; //TODO customize
    const ProviderUrl = 'TODO'; //TODO customize
    
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