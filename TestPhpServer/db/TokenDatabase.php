<?php

require_once __DIR__.'/DatabaseConnector.php';
require_once __DIR__.'/DO/TokenInfo.php';

class TokenDatabase
{

    private $db = null;

    public function __construct(PDO $db)
    {
        $this->db = $db;
    }

    /**
     * @param $token
     * @param $tokenType
     * @return TokenInfo|bool
     */
    public function getTokenActive($token, $tokenType){
        $statement = "
            SELECT 
               id, user_id, created_at, expires_at, token, token_type
            FROM
                tokens
            WHERE
              token = :token
              AND token_type = :token_type
              AND CURRENT_TIMESTAMP() >= created_at 
              AND expires_at > CURRENT_TIMESTAMP()
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "token" => $token,
                "token_type"=>$tokenType
            ));

            $result = $statement->fetch(PDO::FETCH_ASSOC);

            if($result) {
                return new TokenInfo($result);
            }else{
                return $result;
            }
        } catch (PDOException $e) {
            exit($e->getMessage());
        }

    }

    public function removeToken($id){

        $statement = "
            DELETE FROM 
                tokens
            WHERE 
                id = :id
                OR expires_at < CURRENT_TIMESTAMP()
        ";

        try {
            $statement = $this->db->prepare($statement);
            return $statement->execute(array(
                "id" => $id
            ));
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }

    public function addToken($token, $userId, $expiresInMinutes, $tokenType){

        $statement = "
            INSERT INTO 
                tokens (user_id, token, expires_at, token_type)
            VALUES
                (:user_id, :token, TIMESTAMPADD(MINUTE, :expires_at_min, CURRENT_TIMESTAMP() ), :token_type);
        ";

        try {
            $statement = $this->db->prepare($statement);
            return $statement->execute(array(
                "token" => $token,
                "user_id"=>$userId,
                "expires_at_min" =>$expiresInMinutes,
                "token_type" => $tokenType
            ));
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }

}