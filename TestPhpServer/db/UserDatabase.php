<?php


class UserDatabase
{

    private $db = null;

    public function __construct(PDO $db)
    {
        $this->db = $db;
    }

    public function getUser($userId){
        $statement = "
            SELECT 
               user_id, email, full_name
            FROM
                users
            WHERE
              user_id = :uid
        ";

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "uid" => $userId
            ));
            $result = $statement->fetch(\PDO::FETCH_ASSOC);

            return $result;
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
    }

    public function updateUser($user){
        $uid = Auth::getId($user);
        $userDb = $this->getUser($uid);
        if(!$userDb) {
            $statement = "
            INSERT INTO users 
                (user_id, email, full_name)
            VALUES
                (:uid, :email, :full_name);";
        }else{
            $statement = "
            UPDATE users
            SET 
                email = :email,
                full_name  = :full_name
            WHERE user_id = :uid;
            ";
        }

        try {
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "uid" => $uid,
                "email" => $user['email'],
                "full_name" => $user['name']
            ));
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
    }
}