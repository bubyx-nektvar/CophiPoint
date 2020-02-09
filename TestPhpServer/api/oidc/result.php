<?php
session_start();

$authRequest = AuthorizationRequest::Parse($_SESSION);

//TODO: generate code
//TODO: auth store scope

header('Location: '.$authRequest->redirect_uri);

