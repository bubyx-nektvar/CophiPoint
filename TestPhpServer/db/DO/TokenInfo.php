<?php


class TokenInfo
{
    const idCol = 'id';
    const userIdCol = 'user_id';
    const createdAtCol = 'created_at';
    const expiresAtCol = 'expires_at';
    const token = 'token';
    const tokenType = 'token_type';

    private $arr;

    public function __construct($arr)
    {
        $this->arr = $arr;
    }

    public function getId(){
        return $this->arr[self::idCol];
    }

    /**
     * @return string
     */
    public function getUserId()
    {
        return $this->arr[self::userIdCol];
    }

    /**
     * @return mixed
     */
    public function getCreatedAt()
    {
        return $this->arr[self::createdAtCol];
    }

    /**
     * @return mixed
     */
    public function getExpiresAt()
    {
        return $this->arr[self::expiresAtCol];
    }
    /**
     * @return mixed
     */
    public function getToken()
    {
        return $this->arr[self::token];
    }

    /**
     * @return mixed
     */
    public function getTokenType()
    {
        return $this->arr[self::tokenType];
    }
}