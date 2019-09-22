<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 22.09.2019
 * Time: 2:16
 */
require_once __DIR__.'/DatabaseConnector.php';
require_once  __DIR__.'/Order.php';
require_once  __DIR__.'/Purchase.php';

class OrderDatabase
{

    private $db = null;
    private $transform;

    public function __construct(PDO $db)
    {
        $this->db = $db;

        $this->transform = function ($row){
            return OrderDatabase::transform($row);
        };
    }

    private static function transform($row){
        $purchuase = new Purchase();

        $purchuase->ProductId = $row['product_id'];
        $purchuase->ProductName = $row['product'];
        $purchuase->TotalPrice = $row['price'];
        $purchuase->Date = $row['purchuase_date'];

        return $purchuase;
    }

    public function get($id, $userId){

        //columns: [id, user_id, product_id, purchuase_date, product, price]
        $statement = "
            SELECT 
                product_id, purchuase_date, product, price
            FROM
                purchases
            WHERE
              user_id = :uid 
              AND id = :id
        ";

        try {
            //$this->db = new PDO();//TODO remove
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "uid" => $userId,
                "id" => $id
            ));
            $result = $statement->fetch(\PDO::FETCH_ASSOC);

            return OrderDatabase::transform($result);
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
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
        if($this->getUser($uid)) {
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

    public function findAll($userId)
    {
        //columns: [id, user_id, product_id, purchuase_date, product, price]
        $statement = "
            SELECT 
                product_id, purchuase_date, product, price
            FROM
                purchases
            WHERE
              user_id = :uid
        ";

        try {
            //$this->db = new PDO();//TODO remove
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "uid" => $userId
            ));
            $result = $statement->fetchAll(\PDO::FETCH_ASSOC);

            return array_map($this->transform,$result);
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
    }
    public  function balance($userId)
    {

        $statement = "
            SELECT 
                SUM(price) as total
            FROM
                purchases
            WHERE 
                user_id = :uid;
        ";

        try {
            //$this->db = new PDO();//TODO remove
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                "uid" => $userId
            ));
            $result = $statement->fetch(\PDO::FETCH_ASSOC);

            return $result['total'] ?? 0;
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
    }


    public function insert(Order $order, $userId)
    {
        $statement = "
            INSERT INTO purchases 
                (user_id, product_id, product, price)
            VALUES
                (:uid, :productId, :productName, :price);
        ";

        try {
            //$this->db = new PDO();//TODO remove
            $statement = $this->db->prepare($statement);
            $statement->execute(array(
                'uid' => $userId,
                'productId'  => $order->ProductId,
                'productName' => $order->ProductId == 1 ? "Coffee": "Tea",
                'price' => ( -$order->Size->TotalPrice),
            ));
            return $this->db->lastInsertId();
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
    }

}