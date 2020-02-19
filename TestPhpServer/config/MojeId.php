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
    const ClientId = '$MOJEID_CLIENT'; //TODO customize
    const ClientSecret = '$MOJEID_SECRET'; //TODO customize
    const ProviderUrl = '$MOJEID_URL'; //TODO customize
    
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