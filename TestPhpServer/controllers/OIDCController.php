<?php
require __DIR__ . '/../vendor/autoload.php';
require_once __DIR__ . '/../services/Auth.php';
require_once __DIR__.'/../db/TokenDatabase.php';
require_once __DIR__.'/../db/UserDatabase.php';
require_once __DIR__.'/../config/LocalID.php';

use Jumbojett\OpenIDConnectClient;

class AuthResult{
    public $code;
    public $error;
    public $error_description;
}

class OIDCController
{
    private const code = 'code';
    private const accessToken = 'access_token';
    private const refreshToken = 'refresh_token';

    private $tokenDb;
    private $userDb;

    function  __construct($connector = null)
    {
        if(!isset($connector)) {
            $connector = DatabaseConnector::create();
        }

        $dbConnection = $connector->getConnection();

        $this->userDb = new UserDatabase($dbConnection);
        $this->tokenDb= new TokenDatabase($dbConnection);

    }

    private function generate_token($userId, $expiresInMinutes, $tokenType){
        $token = base64_encode(random_bytes(128));
        $this->tokenDb->addToken($token, $userId, $expiresInMinutes, $tokenType);
        return $token;
    }

    private function generate_code($userId){
        return $this->generate_token($userId, LocalID::CodeExpiresInMinutes, OIDCController::code);
    }

    private function generate_access_token($userId){
        return $this->generate_token($userId, LocalID::AccessExpiresInMinutes, OIDCController::accessToken);
    }
    private function generate_refresh_token($userId){
        return $this->generate_token($userId,LocalID::RefreshExpiresInMinutes,OIDCController::refreshToken);
    }



    function authorize(){
        $headers = getallheaders();
        if(!isset($headers['Authorization'])) {
            Auth::die_unauthorize();
        }
        $auth_header = $headers['Authorization'];
        $hh = explode(' ',$auth_header);

        if($hh[0] != 'Bearer'){
            Auth::die_unauthorize();
        }

        $tokenInfo = $this->tokenDb->getTokenActive($hh[1], self::accessToken);
        if(!$tokenInfo){
            Auth::die_unauthorize();
        }
        return $tokenInfo;
    }

    function login(): AuthResult{
        $result = new AuthResult();

        $oidc = new OpenIDConnectClient(MojeId::ProviderUrl,
            MojeId::ClientId,
            MojeId::ClientSecret);
        $oidc->addScope(MojeId::Scopes);
        $oidc->addAuthParam(array("prompt" => "consent"));
        $oidc->addAuthParam(array("claims" => json_encode(MojeId::Claims)));

        $_SERVER['HTTP_UPGRADE_INSECURE_REQUESTS'] = 0; //to disable https upgrade

        try {
            if (!$oidc->authenticate()) {
                $result->error = 'access_denied';
                $result->error_description = 'OIDC provider denied access';
            } else {
                $userInfo = $oidc->requestUserInfo();
                if (!isset($userInfo->email)) {
                    $result->error = 'invalid_request';
                    $result->error_description = 'Email not provided';
                } else {
                    $this->userDb->updateUser($userInfo->sub, $userInfo->email, isset($userInfo->name) ? $userInfo->name : null);
                    $userId = $this->userDb->getUserBySub($userInfo->sub)['id'];
                    $result->code = $this->generate_code($userId);
                }
            }
        }catch (\Jumbojett\OpenIDConnectClientException $ex){
            $result->error = 'access_denied';
            $result->error_description = $ex->getMessage();
        }
        return $result;
    }

    private function createTokensWithToken($token, $tokenType){
        $tokenInfo = $this->tokenDb->getTokenActive($token, $tokenType);
        if(isset($tokenInfo) && $tokenInfo) {
            $result = array(
                'token_type'=>'Bearer',
                'expires_in'=> LocalID::AccessExpiresInMinutes * 60,
                'access_token' => $this->generate_access_token($tokenInfo->getUserId()),
                'refresh_token' => $this->generate_refresh_token($tokenInfo->getUserId()),
                'id_token'=> null
            );

            $this->tokenDb->removeToken($tokenInfo->getId());
            return $result;
        }else Auth::TokenErrorResponse('invalid_grant', 'Token exchange failed due to missing grant type value');
    }

    function createTokensWithCode($code){
        return $this->createTokensWithToken($code, self::code);
    }
    function createTokensWithRefresh($refresh_token){
        return $this->createTokensWithToken($refresh_token, self::refreshToken);

    }
}