<?php


class AuthorizationRequest
{
    /**
     * @var array(string)
     */
    public $scope;
    /**
     * @var string
     */
    public $response_type;
    /**
     * @var string
     */
    public $clientId;
    /**
     * @var string
     */
    public $redirect_uri;

    public $state;
    public $nonce;
    public $codeChallenge;
    public $codeChallendeMethod;

    private static function trySet(&$target, &$source){
        if(isset($source)){
            $target = $source;
        }
    }

    public static function Parse($arr){
        $request =new AuthorizationRequest();

        self::trySet($request->clientId,  $arr['client_id']);
        if(isset($arr['scope'])) {
            $request->scope = explode(' ', $arr['scope']);
        }
        self::trySet($request->response_type, $arr['response_type']);
        self::trySet($request->redirect_uri, $arr['redirect_uri']);
        self::trySet($request->state, $arr['state']);
        self::trySet($request->nonce, $arr['nonce']);
        self::trySet($request->codeChallenge, $arr['code_challenge']);
        self::trySet($request->codeChallendeMethod, $arr['code_challende_method']);

        return $request;
    }
    public function store(&$arr){
        self::trySet($arr['client_id'],$this->clientId);
        if(isset($this->scope)){
            $arr['scope'] = implode(' ', $this->scope);
        }
        self::trySet($arr['response_type'],$this->response_type);
        self::trySet($arr['redirect_uri'],$this->redirect_uri);
        self::trySet($arr['state'],$this->state);
        self::trySet($arr['nonce'],$this->nonce);
        self::trySet($arr['code_challenge'],$this->codeChallenge);
        self::trySet($arr['code_challende_method'],$this->codeChallendeMethod);
    }
    private function invalidRequest($error, $error_description){
        Auth::AuthenticationErrorResponse(
            $this->redirect_uri,
            $error_description,
            $error_description,
            $this->state
        );
    }
    public function check():bool {
        if($this->clientId != LocalID::ClientID){
            $this->invalidRequest('unauthorized_client','Invalid client_id');
        }
        else if($this->response_type != 'code'){
            $this->invalidRequest('invalid_request', 'Unsupported response_type');
        }
        else if($this->redirect_uri != LocalID::AllowedRedirectUrl){
            $this->invalidRequest('invalid_request','Invalid redirect_url');
        }
        else if(count(array_diff($this->scope, array_intersect($this->scope,LocalID::Scopes))) != 0){
            $this->invalidRequest('invalid_scope','Some scopes are not allowed');
        }else{
            return true;
        }
    }
}