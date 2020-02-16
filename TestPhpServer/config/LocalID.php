<?php

// settings for this php as OIDC provider
class LocalID
{
    const AllowedRedirectUrl = 'mff.cophipoint://connect'; //TODO customize
    const ClientID = 'CophiPointAPI'; //TODO customize
    const ClientSecret = 'd763c6623386636b42608de9856731edbca26885540b4de96cdc8d4d';//TODO customize
    
    const Scopes = ['openid', 'email', 'balance'];
    const AccessExpiresInMinutes = 60;
    const RefreshExpiresInMinutes = 60 * 24 * 7; //7 days
    const CodeExpiresInMinutes = 5;
}