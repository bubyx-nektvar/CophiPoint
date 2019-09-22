<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 22.09.2019
 * Time: 2:15
 */

class DatabaseConnector
{

    private $dbConnection = null;

    public function __construct()
    {
        $host = 'wm112.wedos.net';
        $db   = 'd167694_cophi';
        $user = 'w167694_cophi';
        $pass = 'rLX_j4_24';

        try {
            $this->dbConnection = new \PDO(
                "mysql:host=$host;dbname=$db",
                $user,
                $pass
            );
            $this->dbConnection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch (\PDOException $e) {
            exit($e->getMessage());
        }
    }

    public function getConnection()
    {
        return $this->dbConnection;
    }
}