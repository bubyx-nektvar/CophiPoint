<?php

require_once __DIR__.'/DatabaseConnector.php';

class UserDatabase
{

    private $db = null;

    public function __construct(PDO $db)
    {
        $this->db = $db;
    }

    public function getUserById($userId){
        $statement = "
            SELECT 
               id, sub, email, full_name
            FROM
                users
            WHERE
              id = :uid
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "uid" => $userId
            ));
            return $statement->fetch(PDO::FETCH_ASSOC);
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }
    public function getUserBySub($userId){
        $statement = "
            SELECT 
               id, sub, email, full_name
            FROM
                users
            WHERE
              sub = :sub
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "sub" => $userId
            ));
            return $statement->fetch(PDO::FETCH_ASSOC);
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }

    public function updateUser($sub, $email, $fullName = null){
        $userDb = $this->getUserBySub($sub);
        if(!$userDb) {
            $statement = "
            INSERT INTO users 
                (sub, email, full_name)
            VALUES
                (:sub, :email, :full_name);";
        }else{
            $statement = "
            UPDATE users
            SET 
                email = :email,
                full_name  = :full_name
            WHERE sub = :sub;
            ";
        }

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "sub" => $sub,
                "email" => $email,
                "full_name" => $fullName
            ));
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }
}