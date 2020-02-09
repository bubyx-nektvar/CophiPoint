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

    public static function Parse($arr){
        $request =new AuthorizationRequest();
        $request->clientId = $arr['client_id'];
        $request->scope = explode(' ', $arr['scope']);
        $request->response_type = $arr['response_type'];
        $request->redirect_uri = $arr['redirect_uri'];
        return $request;
    }
    public function store($arr){
        $arr['client_id'] = $this->clientId;
        $arr['scope'] = $this->scope;
        $arr['response_type'] = $this->response_type;
        $arr['redirect_uri'] = $this->redirect_uri;
    }
}