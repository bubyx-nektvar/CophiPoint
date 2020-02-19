<?php

// settings for this php as OIDC provider
class LocalID
{
    const AllowedRedirectUrl = '$CUSTOM_SCHEME://connect'; //TODO customize
    const ClientID = '$CLIENT_ID'; //TODO customize
    const ClientSecret = '$CLIENT_SECRET';//TODO customize

    const Scopes = ['openid', 'email', 'balance'];
    const AccessExpiresInMinutes = 60;
    const RefreshExpiresInMinutes = 60 * 24 * 7; //7 days
    const CodeExpiresInMinutes = 5;
}