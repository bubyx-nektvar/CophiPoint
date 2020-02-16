<?php

// settings for this php as OIDC provider
class LocalID
{
    const AllowedRedirectUrl = 'mff.cophipoint://connect'; //TODO customize
    const ClientID = 'TODO'; //TODO customize
    const ClientSecret = 'TODO';//TODO customize

    const Scopes = ['openid', 'email', 'balance'];
    const AccessExpiresInMinutes = 60;
    const RefreshExpiresInMinutes = 60 * 24 * 7; //7 days
    const CodeExpiresInMinutes = 5;
}